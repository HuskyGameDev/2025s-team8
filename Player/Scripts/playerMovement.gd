class_name Player extends CharacterBody2D
#note this is a script that will eventually get replaced by the state machine. The state machine
#will store the previous statement and remember it to make transitions in between actions smoother

var cardinal_direction : Vector2 = Vector2.DOWN #part of sprite movement
var direction : Vector2 = Vector2.ZERO

@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D
@onready var state_machine : PlayerStateMachine = $StateMachine

#health here
signal player_damaged(hurt_box: Hurtbox)
var invincible : bool = false
var hp : int = 10
var max_hp: int = 10
@onready var hitbox: Hitbox = $interactions/Hitbox
@onready var effect_player: AnimationPlayer = $EffectPlayer

 #references the hitbox of the player 

signal DirectionChanged( _new_direction: Vector2)
	
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	state_machine.Initialize(self)
	hitbox.Damaged.connect(TakeDamage)
	update_hp(10)
	pass # Replace with function body.

func TakeDamage(hurt_box: Hurtbox) -> void:
	if hp > 0:
		player_damaged.emit(hurt_box)
	else:
		player_damaged.emit(hurt_box)
	
	#queue_free()
	if hp <= 0:
		queue_free()
	pass
	
func update_hp(delta:int) -> void:
	hp = hp + delta
	if hp > max_hp:
		hp = max_hp
		

	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	
	direction.x = Input.get_action_strength("right") - Input.get_action_strength("left");
	direction.y = Input.get_action_strength("down") - Input.get_action_strength("up");
	direction = direction.normalized();
	pass

# function to show movement
func _physics_process(delta: float ):
	move_and_slide()

#adding in functions to match up with sprites
#detects the direction
func SetDirection() -> bool:
	var new_dir: Vector2 = cardinal_direction
	if direction == Vector2.ZERO:
		return false
	
	if direction.y == 0:
		new_dir = Vector2.LEFT if direction.x < 0 else Vector2.RIGHT
	elif direction.x == 0:
		new_dir = Vector2.UP if direction.y < 0 else Vector2.DOWN

		
	if new_dir == cardinal_direction:
		return false
	
	cardinal_direction = new_dir
	DirectionChanged.emit(new_dir)
	#this swithces the way the player is facing rather than create a new sprite animation
	sprite.scale.x = -1 if cardinal_direction == Vector2.LEFT else 1
	return true

#function to change update the animation



#function to change update the animation
func UpdateAnimation(state: String) -> void: 
	animation_player.play(state + "_" + AnimDirection())
	#print(state + "_" + AnimDirection() )
	pass
	
#helper function for animation
func AnimDirection() -> String:
	if cardinal_direction == Vector2.DOWN:
		return "down"
	elif cardinal_direction == Vector2.UP:
		return "up"
	else:
		return "side"
		
func _on_detection_area_body_entered(_body: Node2D) -> void:
	
	pass # Replace with function body.

func MakeInvincible ( _duration : float = 1.0) -> void:
	invincible = true
	
	await get_tree().create_timer(_duration).timeout # wait 1 seconf before code continues
	
	invincible = false
	pass

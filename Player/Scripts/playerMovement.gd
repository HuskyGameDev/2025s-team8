class_name Player extends CharacterBody2D
#note this is a script that will eventually get replaced by the state machine. The state machine
#will store the previous statement and remember it to make transitions in between actions smoother

var cardinal_direction : Vector2 = Vector2.DOWN #part of sprite movement
var direction : Vector2 = Vector2.ZERO

@onready var animation_player : AnimationPlayer = $AnimationPlayer
@onready var sprite : Sprite2D = $Sprite2D
@onready var state_machine : PlayerStateMachine = $StateMachine

signal DirectionChanged( _new_direction: Vector2)

#stuff related to stun state

signal player_damaged(hurt_box: Hurtbox)
var invulnerable : bool = false
var hp : int = 10
var max_hp : int = 10
@onready var hitbox: Hitbox = $interactions/Hitbox
@onready var hurtbox: Hurtbox = $interactions/Weapons/Sword/Hurtbox



# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	state_machine.Initialize(self)
	hitbox.Damaged.connect(take_damage)
	update_hp(10)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	
	direction.x = Input.get_action_strength("Right") - Input.get_action_strength("Left");
	direction.y = Input.get_action_strength("Down") - Input.get_action_strength("Up");
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

#after the build test this code resembles code closer to tutorial video
func take_damage(hurt_box: Hurtbox) -> void:
	if invulnerable == true:
		return
	update_hp(-hurt_box.damage)
	if(hp > 0):
		player_damaged.emit(hurt_box)
	else:
		player_damaged.emit(hurt_box)
		update_hp(10);
		print("player has died")
		
	pass
	
func update_hp(delta: int) -> void:
	hp = hp + delta
	if(hp > max_hp):
		hp = max_hp
	
	pass

func MakeInvulnerable(_duration : float = 1.0) -> void:
	invulnerable = true
	hitbox.monitorable = false
	hurtbox.monitoring = false
	
	await get_tree().create_timer(_duration).timeout
	
	invulnerable = false
	hitbox.monitoring = true
	hurtbox.monitoring = true
	pass

#func MakeInvincible ( _duration : float = 1.0) -> void:
#	invincible = true
#	mainhitbox.monitorable = false
#	mainhitbox.monitoring = false
#	await get_tree().create_timer(_duration).timeout # wait 1 seconf before code continues
#	
#	invincible = false
#	mainhitbox.monitorable = true
#	mainhitbox.monitoring = true
#	pass

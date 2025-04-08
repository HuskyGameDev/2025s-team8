class_name state_attack extends State

#tutorial is using animatino player
#consider switching to time

var attacking: bool = false
@onready var animation_player: AnimationPlayer = $"../../AnimationPlayer"

@onready var idle: State = $"../Idle"

@onready var walk: State = $"../Walk"

@onready var hitbox: Hitbox = $"../../interactions/Hitbox"
@onready var hurtbox: Hurtbox = $"../../interactions/Hurtbox"

func Enter() -> void:
	player.UpdateAnimation("attack")
	animation_player.animation_finished.connect(EndAttack)
	attacking = true
	
	#this essentially delays the animation for turning on the hurt
	await get_tree().create_timer(.075).timeout
	hurtbox.monitoring = true
	pass
	
func Exit() -> void:
	animation_player.animation_finished.disconnect( EndAttack)
	attacking = false
	
	hurtbox.monitoring = true
	pass
	
func Process(_delta : float) -> State:
	player.velocity = Vector2.ZERO
	
	
	if attacking == false:
		if player.direction == Vector2.ZERO:
			return idle
		else:
			return walk
	
	return null
	
func Physics(_delta: float) -> State:
	return null
	
func HandleInput(_event: InputEvent) -> State:
	return null
	
	#
func EndAttack(_newAnimName : String) -> void:
	attacking = false
	hurtbox.monitoring = false;

	

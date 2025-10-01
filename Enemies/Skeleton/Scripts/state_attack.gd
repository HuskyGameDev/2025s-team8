class_name skeleton_state_attack extends skeleton_state

#tutorial is using animatinon player
#consider switching to time

var attacking: bool = false
@onready var animation_player: AnimationPlayer = $"../../AnimationPlayer"
@onready var attack: Node = $"."


#@onready var idle: State = $"../Idle"
@onready var idle: skeleton_state_idle = $"../idle"

#@onready var walk: State = $"../Walk"
@onready var walk: skeleton_state_walk = $"../walk"

#@onready var hitbox: Hitbox = $"../../interactions/Hitbox"
@onready var hit_box: Hitbox = $"../../hit_box"


#@onready var hurtbox: Hurtbox = $"../../interactions/Hurtbox"
@onready var hurt_box: Hurtbox = $"../../hurt_box"

func Enter() -> void:
	skeleton.UpdateAnimation("attack")
	animation_player.animation_finished.connect(EndAttack)
	attacking = true
	
	#this essentially delays the animation for turning on the hurt
	await get_tree().create_timer(.075).timeout
	hurt_box.monitoring = true
	pass
	
func Exit() -> void:
	animation_player.animation_finished.disconnect(EndAttack)
	attacking = false
	
	hurt_box.monitoring = false
	pass
	
func Process(_delta : float) -> skeleton_state:
	skeleton.velocity = Vector2.ZERO
	
	
	if attacking == false:
		if skeleton.direction == Vector2.ZERO:
			return idle
		else:
			return walk
	
	return null
	
func Physics(_delta: float) -> skeleton_state:
	return null
	
func HandleInput(_event: InputEvent) -> skeleton_state:
	return null
	
	#
func EndAttack(_newAnimName : String) -> void:
	attacking = false
	hurt_box.monitoring = false;

	

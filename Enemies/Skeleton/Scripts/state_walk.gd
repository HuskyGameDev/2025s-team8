class_name skeleton_state_walk extends skeleton_state

@export var move_speed : float = 125
#@onready var idle : State = $"../Idle"
@onready var idle: skeleton_state_idle = $"../idle"

#@onready var attack: State = $"../attack"
@onready var attack: skeleton_state_attack = $"../attack"


func Enter() -> void:
	skeleton.UpdateAnimation("walk")
	pass
	
func Exit() -> void:
	pass


func Process(_delta : float) -> skeleton_state:
	if skeleton.direction == Vector2.ZERO:
		return idle
	
	skeleton.velocity = skeleton.direction * move_speed
	
	if skeleton.SetDirection():
		skeleton.UpdateAnimation("walk")
	return null


func Physics(_delta: float) -> skeleton_state:
	return null


func HandleInput(_event: InputEvent) -> skeleton_state:
	#repeat code from idle for attack
	if _event.is_action_pressed("attack"):
			return attack
	return null

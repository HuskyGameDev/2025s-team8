class_name state_walk extends State

@export var move_speed : float = 100
@onready var idle : State = $"../Idle"
@onready var attack: State = $"../attack"

func Enter() -> void:
	player.UpdateAnimation("walk")
	pass
	
func Exit() -> void:
	pass


func Process(_delta : float) -> State:
	if player.direction == Vector2.ZERO:
		return idle
	
	player.velocity = player.direction * move_speed
	
	if player.SetDirection():
		player.UpdateAnimation("walk")
	return null


func Physics(_delta: float) -> State:
	return null


func HandleInput(_event: InputEvent) -> State:
	#repeat code from idle for attack
	if _event.is_action_pressed("Attack"):
			return attack
	return null
	

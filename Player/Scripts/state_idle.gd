class_name state_idle extends State

@onready var walk: State = $"../Walk"
@onready var attack: State = $"../attack"

func Enter() -> void:
	player.UpdateAnimation("Idle")
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> State:
	if player.direction != Vector2.ZERO:
		return walk
	player.velocity = Vector2.ZERO
	return null
	
func Physics(_delta: float) -> State:
	return null
	
func HandleInput(_event: InputEvent) -> State: #lmb for attack
	if _event.is_action_pressed("attack"):
			return attack
		
	return null
	

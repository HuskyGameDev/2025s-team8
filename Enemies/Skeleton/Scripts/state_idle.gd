class_name skeleton_state_idle extends skeleton_state

@onready var walk: skeleton_state = $"../Walk"
@onready var attack: skeleton_state = $"../attack"

func Enter() -> void:
	skeleton.UpdateAnimation("idle")
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> skeleton_state:
	if skeleton.direction != Vector2.ZERO:
		return walk
	skeleton.velocity = Vector2.ZERO
	return null
	
func Physics(_delta: float) -> State:
	return null
	
func HandleInput(_event: InputEvent) -> skeleton_state: #lmb for attack
	if _event.is_action_pressed("attack"):
			return attack
		
	return null
	

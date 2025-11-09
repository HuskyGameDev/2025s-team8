class_name knightIdle extends knight_state

@export var anim_name: String = "idle"
@export_category("AI")
@export var state_duration_min: int = 0.5
@export var state_duration_max: int = 1.5
@export var after_idle_state : knight_state

var _timer: float = 1.0

func init() -> void:
	pass

func _ready():
	pass
	
func Enter() -> void:
	knight.velocity = Vector2.ZERO
	_timer = randf_range(state_duration_min, state_duration_max)
	knight.updateAnimation(anim_name)
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> knight_state:
	#process should always be subtracting delta. can hardcode if needed since its separate script
	_timer -= _delta
	if _timer <= 0:
		return after_idle_state
		
	return null
	
func Physics(_delta: float) -> knight_state:
	return null
	
	

class_name KnightStateIdle extends KnightState

@export var anim_name : String = "idle"

@export_category("AI")
@export var state_duration_min: float = .5
@export var state_duration_max: float = 1.5
@export var after_idle_state: KnightState

var _timer:float = 0

func enter() -> void:
	knight.velocity = Vector2.ZERO
	_timer = randf_range( state_duration_min, state_duration_max)
	knight.UpdateAnimation(anim_name)
	pass
	
func exit() ->void:
	pass
# 


# Called every frame. 'delta' is the elapsed time since the previous frame.
func process(_delta: float) -> KnightState:
	_timer -= _delta
	if _timer < 0:
		return after_idle_state
	return null

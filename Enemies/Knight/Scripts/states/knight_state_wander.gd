class_name KnightStateWander extends KnightState

@export var anim_name : String = "walk"
@export var walk_speed : float = 50

@export_category("AI")
@export var state_animation_duration: float = .8
@export var state_cycles_min : int = 1
@export var state_cycles_max : int = 3

@export var next_state: KnightState

var _timer:float = 0
var _direction : Vector2

func enter() -> void:
	_timer = randi_range(state_cycles_min, state_cycles_max) * state_animation_duration
	var rand = randi_range( 0, 3)
	_direction = knight.DIR_4[rand]
	knight.veloctiy = _direction * walk_speed
	knight.set_direction(_direction)
	knight.update_animation(anim_name)
	pass
	
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func process(delta: float) -> KnightState:
	_timer -= delta
	if _timer < 0:
		return next_state
	return null

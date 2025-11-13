class_name knightWalk extends knight_state

@export var anim_name: String = "walk"
@export_category("AI")
@export var state_duration_min: int = 0.5
@export var state_duration_max: int = 1.5
@export var after_idle_state : knight_state

@export var move_speed : float = 60
@onready var idle: knightIdle = $"../idle"

var _timer: float = 1.0

func init() -> void:
	pass

func _ready():
	pass
	
func Enter() -> void:
	knight.updateAnimation(anim_name)
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> knight_state:
	#process should always be subtracting delta. can hardcode if needed since its separate script
	if knight.direction == Vector2.ZERO:
		return idle
	
	knight.velocity = knight.direction * move_speed
	
	if knight.SetDirection():
		knight.UpdateAnimation("walk")
	return null
	
func Physics(_delta: float) -> knight_state:
	return null
	

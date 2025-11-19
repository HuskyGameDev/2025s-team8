class_name state_walk extends State

@export var move_speed : float = 150
@onready var idle : State = $"../Idle"
@onready var attack: State = $"../attack"

@export var ghost_scene: PackedScene

var can_dash: bool = true
var dash_mult: float = 1

func Enter() -> void:
	player.UpdateAnimation("walk")
	pass
	
func Exit() -> void:
	pass


func Process(_delta : float) -> State:
	if player.direction == Vector2.ZERO:
		return idle
	
	player.velocity = player.direction * move_speed * dash_mult
	
	if player.SetDirection():
		player.UpdateAnimation("walk")
		
	if Input.is_action_just_pressed("Dash") and can_dash:
		can_dash = false
		dash_mult = 5
		var ghost = ghost_scene.instantiate()
		get_parent().add_child(ghost)
		ghost.global_position = player.global_position
		await get_tree().create_timer(0.1).timeout
		dash_mult = 1
		await get_tree().create_timer(0.1).timeout
		ghost.queue_free()
		await get_tree().create_timer(0.1).timeout
		can_dash = true
		
		
	return null


func Physics(_delta: float) -> State:
	return null


func HandleInput(_event: InputEvent) -> State:
	#repeat code from idle for attack
	if _event.is_action_pressed("Attack"):
			return attack
	return null
	

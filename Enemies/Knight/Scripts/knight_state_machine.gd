class_name KnightStateMachine extends Node

var states : Array[KnightState]
var prev_state : KnightState
var current_state : KnightState


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	process_mode = Node.PROCESS_MODE_DISABLED
	pass # Replace with function body.

func init() -> void:
	pass
	
func enter() -> void:

	pass
	
func exit() -> void:
	pass
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func process(delta: float) -> KnightState:
	change_state(current_state.process(delta))
	return null
	
func _physics_process(delta: float) -> void:
	change_state(current_state.physics(delta))
	pass
	
func initialize(_knight : Knight) -> void:
	states = []
	
	for c in get_children():
		if c is KnightState:
			states.append(c)
			
	for s in states:
		s.knight = _knight
		s.knight_state_machine = self
		s.init()
		
	if states.size() > 0:
		change_state( states[0] )
		process_mode = Node.PROCESS_MODE_INHERIT
	pass
	
func change_state(new_state : KnightState) -> void:
	if new_state == null || new_state == current_state:
		return
		
	if current_state: 
		current_state.exit()
		
	prev_state = current_state
	current_state = new_state
	current_state.enter()

func physics(_delta : float) -> KnightState:
	return null

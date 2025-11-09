class_name knight_state_machine extends Node
#notes for future bryson: process state can be adjusted and tied to velocity for state changes
var enemy: Enemy

var states: Array[knight_state]
var prev_state: knight_state
var curr_state: knight_state

func _ready():
	process_mode = Node.PROCESS_MODE_DISABLED
	pass
	
func _process(delta:float):
	change_state(curr_state.process(delta))
	pass
	
func _physics_process(delta:float):
	change_state(curr_state.physics(delta))
	pass

	
func initialize(_enemy:Enemy) -> void:
	states = []
	
	for c in get_children():
		if c is knight_state:
			states.append(c)
	
	for s in states:
		s.enemy = _enemy
		s.state_machine = self
		s.init()
		
	if states.size() > 0:
		change_state(states[0])
		process_mode = Node.PROCESS_MODE_INHERIT
	pass
	
	

func change_state(new_state:knight_state) -> void:
	if new_state == null || new_state == curr_state:
		return
	
	if curr_state:
		curr_state.Exit
	
	prev_state = curr_state
	curr_state = new_state
	curr_state.Enter()

class_name knight_state_machine extends Node

var states : Array[ knight_state ]
var prev_state : knight_state
var current_state : knight_state



# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	process_mode = Node.PROCESS_MODE_DISABLED
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
#	ChangeState( await current_state.Process(delta) ) #(Derek): I'm not sure if having an await here is a bad idea, but it works
	pass
	
func _physics_process(delta: float) -> void:
#	ChangeState( current_state.Physics(delta) )
	pass
	
func _unhandled_input(event):
#	ChangeState(current_state.HandleInput(event) )
	pass


func Initialize(_knight : Knight) -> void:
	states = []
	
	for c in get_children(): 
		if c is State:
			states.append(c)
	
	if states.size() == 0:
		return
	
	states[0].knight = _knight
	states[0].state_machine = self
	
	for state in states:
		state.init()
	
	ChangeState(states[0])
	process_mode = Node.PROCESS_MODE_INHERIT


func ChangeState(new_state: knight_state) -> void:
	if new_state == null || new_state == current_state:
		return
	if current_state:
		current_state.Exit()
		
	prev_state = current_state
	current_state = new_state
	current_state.Enter()

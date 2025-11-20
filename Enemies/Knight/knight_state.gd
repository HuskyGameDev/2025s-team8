class_name knight_state extends Node

#stores a reference to the player
static var knight: Knight
static var state_machine: knight_state_machine

func _ready():
	pass
	
func Enter() -> void:
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> State:
	return null
	
func Physics(_delta: float) -> State:
	return null
	
func HandleInput(_event: InputEvent) -> State:
	return null
	

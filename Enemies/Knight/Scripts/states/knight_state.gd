class_name KnightState extends Node

var knight : Knight
var knight_state_machine = KnightStateMachine
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

func init() -> void:
	
	pass
	
func enter() -> void:
	pass
	
func exit() -> void:
	pass
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func process(delta: float) -> KnightState:
	return null

func physics(_delta : float) -> KnightState:
	return null

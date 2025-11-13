class_name knight_state extends Node

#stores a reference to the player
static var knight: Knight
static var state_machine: knight_state_machine #dont delete not a redundant var

func init() -> void:
	pass

func _ready():
	pass
	
func Enter() -> void:
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> knight_state:
	return null
	
func physics(_delta: float) -> knight_state:
	return null
	
	

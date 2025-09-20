class_name skeleton_state extends Node

#stores a reference to the skeleton
#skeleton state should be done
static var skeleton: Skeleton
@onready var skeleton_state_machine: skeleton_state_machine = $SkeletonStateMachine


func _ready():
	pass
	
func init() -> void:
	pass
	
func Enter() -> void:
	pass
	
func Exit() -> void:
	pass
	
func Process(_delta : float) -> skeleton_state:
	return null
	
func Physics(_delta: float) -> skeleton_state:
	return null
	
func HandleInput(_event: InputEvent) -> skeleton_state:
	return null
	

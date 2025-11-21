#this node is going to adjust some of the stuff such as positioning of the hurtbox
#in relation to the player

class_name InteractionsFix extends Node2D

@onready var player: Player = $".."

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	player.DirectionChanged.connect(_updateDirection)
	pass # Replace with function body.

func _updateDirection(new_direction : Vector2) -> void: #this is for the hurt box probably not needed for hitbox
	match new_direction: #changing where the hurtbox is along with the player
		Vector2.DOWN:
			rotation_degrees = 0
		Vector2.UP:
			rotation_degrees = 180
		Vector2.RIGHT:
			rotation_degrees = 270
		Vector2.LEFT:
			rotation_degrees = 90
		_:
			rotation_degrees = 0
	pass
	

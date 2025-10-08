extends Area2D

# Simple script to use collisions for changing scenes
# May be useful later, but now just for leaving tutorial

# Should allow for the script to be reused
@export var newScene: PackedScene = null

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	self.body_entered.connect(_on_body_entered)


func _on_body_entered(body):
	if body.name == "Player":
		await get_tree().create_timer(1).timeout
		get_tree().change_scene_to_file(newScene.resource_path)

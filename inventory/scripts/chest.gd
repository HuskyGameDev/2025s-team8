extends Area2D

@onready var chest_inventory := $CanvasLayer/Chest
var player_nearby := false

func _ready():
	self.body_entered.connect(_on_body_entered)
	self.body_exited.connect(_on_body_exited)
	chest_inventory.hide()

func _on_body_entered(body):
	if body.name == "Player":
		player_nearby = true

func _on_body_exited(body):
	if body.name == "Player":
		player_nearby = false
		chest_inventory.hide()

func _input(event):
	if event is InputEventKey and Input.is_action_just_released("Interact"):
		if player_nearby:
			chest_inventory.visible = not chest_inventory.visible

class_name healthBar extends ProgressBar

@export var player: Player 
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	player.update_hp_progress.connect(updateHp)
	updateHp()
	print("Hp updated")
	# Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func updateHp():
	value = player.hp * 100/player.max_hp

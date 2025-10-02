class_name Sword extends Weapon


@onready var hurtbox: Hurtbox = $Hurtbox

#temporary addition just to visualize the hurtbox area
@onready var attackSprite: Sprite2D = $Sprite2D


func attack() -> void:
	hurtbox.monitoring = true
	attackSprite.visible = true
	await get_tree().create_timer(0.1).timeout
	attackSprite.visible = false
	hurtbox.monitoring = false

class_name Spear extends Weapon


@onready var hurtbox: Hurtbox = $Slider/Hurtbox
@onready var slider: Node2D = $Slider

#temporary addition just to visualize the hurtbox area
@onready var attackSprite: Sprite2D = $Slider/Sprite2D

func attack() -> void:
	hurtbox.monitoring = true
	attackSprite.visible = true
	for i in range(0,5):
		await get_tree().create_timer(0.025).timeout
		slider.position += Vector2(0,10)
		
		
	slider.position = Vector2(0,0)
	await get_tree().create_timer(0.05).timeout
	
	attackSprite.visible = false
	hurtbox.monitoring = false

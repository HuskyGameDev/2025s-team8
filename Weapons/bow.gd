class_name Bow extends Weapon


@onready var attackSprite: Sprite2D = $Sprite2D
@export var projectile_scene: PackedScene

@onready var player = get_parent().get_parent().get_parent() # idk if there's a better way to access player


func attack() -> void:
	attackSprite.visible = true
	
	var projectile = projectile_scene.instantiate() as Projectile
	if(projectile != null):
		get_parent().get_parent().get_parent().get_parent().add_child(projectile)
	
	#var dir = (global_position - player.global_position).normalized()
	var dir = (get_global_mouse_position() - global_position).normalized()
	projectile.global_position = global_position
	projectile.global_rotation = global_rotation
	projectile.direction = dir
	projectile.look_at(get_global_mouse_position())
	
	await get_tree().create_timer(0.1).timeout
	attackSprite.visible = false

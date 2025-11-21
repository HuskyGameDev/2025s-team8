class_name Wand extends Weapon


@onready var attackSprite: Sprite2D = $Sprite2D
@export var projectile_scene: PackedScene

@onready var player = get_parent().get_parent().get_parent() # idk if there's a better way to access player

var spread: float = 40


func attack() -> void:
	attackSprite.visible = true
	
	for i in range(3):
		await get_tree().create_timer(0.1).timeout
		spawn_projectile()
	
	
	await get_tree().create_timer(0.1).timeout
	attackSprite.visible = false
	

func spawn_projectile() -> void:
	var projectile = projectile_scene.instantiate() as Projectile
	if(projectile != null):
		get_parent().get_parent().get_parent().get_parent().add_child(projectile)
	
	var target_pos = get_global_mouse_position() + Vector2(randf_range(-spread,spread),randf_range(-spread,spread))
	
	var dir = (target_pos - global_position).normalized()
	projectile.global_position = global_position
	projectile.global_rotation = global_rotation
	projectile.direction = dir
	projectile.look_at(target_pos)
	
	

class_name Enemy extends CharacterBody2D


@export var player_found : bool = false
@export var player: Node2D = null
#@onready var mc: Player = $"."

@export var hp: int = 3

@export var enemy_name : String


func _ready():
	$HitBox.Damaged.connect( TakeDamage )
	
		
#bryson note this does not have signals for getting damaged state
func TakeDamage(hurt_box: Hurtbox) -> void:
	hp = hp - hurt_box.damage#underscore means optional
	flash_red()
	print(enemy_name + " took " + str(hurt_box) + " damage.")
	if hp <= 0:
		queue_free()
		
		
func flash_red():
	$Sprite2D.modulate = Color(200,0,0) # red
	await get_tree().create_timer(0.1).timeout
	$Sprite2D.modulate = Color(1,1,1,1) # back to normal
		

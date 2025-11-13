class_name Enemy extends CharacterBody2D


@export var player_found : bool = false
@export var player: Node2D = null
#@onready var mc: Player = $"."

@export var hp: int = 3
var max_hp: int;

@export var enemy_name : String

var dead = false

@onready var sprite : Sprite2D = $Sprite2D

func _ready():
	$HitBox.Damaged.connect( TakeDamage )
	max_hp = hp;
	if(dead): Die() # for enemies spawned as dead


func TakeDamage(_damage:int) -> void:
	hp = hp - _damage#underscore means optional
	flash_red()
	print(enemy_name + " took " + str(_damage) + " damage.")
	if hp <= 0 and !dead:
		Die()
		#await get_tree().create_timer(2).timeout;
		#Revive()

func Die():
	#queue_free()
	dead = true
	visible = false

func Revive():
	hp = max_hp;
	dead = false;
	visible = true;



func flash_red():
	sprite.modulate = Color(200,0,0) # red
	await get_tree().create_timer(0.1).timeout
	sprite.modulate = Color(1,1,1,1) # back to normal
		

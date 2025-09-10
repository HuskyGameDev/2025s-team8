class_name Enemy extends CharacterBody2D


@export var chase_player : bool = false
@export var player: Node2D = null
#@onready var mc: Player = $"."

# Called when the node enters the scene tree for the first time.
var hp: int = 3

@export var enemy_name : String


func _ready():
	$HitBox.Damaged.connect( TakeDamage )
	
		
		
func TakeDamage(_damage:int) -> void:
	hp = hp - _damage#underscore means optional
	print(enemy_name + " took " + str(_damage) + " damage.")
	if hp <= 0:
		queue_free()
		

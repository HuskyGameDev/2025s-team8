class_name Enemy extends CharacterBody2D


@export var sight : float = 100
@export var chase_player : bool = false
@export var player: Node2D = null
#@onready var mc: Player = $"."

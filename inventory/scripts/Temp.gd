class_name Stats
extends Label

#Temp script to ensure that the inventory items would correctly affect stats
@export var def: int = 0
@export var dam: int = 0

func _process(_delta):
	text = ("Damage: " + str(dam) + "\nDefense: " + str(def))

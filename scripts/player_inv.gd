extends Control

#Use this template for loading instances of a resource
var items_to_load := [
	preload("res://inventory/items/coin.tres").duplicate(),
	preload("res://inventory/items/armor.tres").duplicate(),
	preload("res://inventory/items/bow.tres").duplicate(),
	preload("res://inventory/items/potion.tres").duplicate(),
	preload("res://inventory/items/staff.tres").duplicate(),
	preload("res://inventory/items/sword.tres").duplicate()
]

func _ready():
	#creates a number of slots
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(32, 32))
		%Grid.add_child(slot)
	for i in items_to_load.size():
		var item = InventoryItem.new(null)
		item._init(items_to_load[i])
		%Grid.get_child(i).add_child(item)

extends Control

"""
#Use this template for loading instances of a resource
var items_to_load := [
	preload("res://Inventory/items/coin.tres").duplicate(),
	preload("res://Inventory/items/armor.tres").duplicate(),
	preload("res://Inventory/items/bow.tres").duplicate(),
	preload("res://Inventory/items/potion.tres").duplicate(),
	preload("res://Inventory/items/staff.tres").duplicate(),
	preload("res://Inventory/items/sword.tres").duplicate()
]


func _ready():
	#creates a number of slots
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(64, 64))
		self.add_child(slot)
	for i in items_to_load.size():
		var item = InventoryItem.new(null)
		item._init(items_to_load[i])
		self.get_child(i).add_child(item)
		
func _input(event):
	if event is InputEventKey and Input.is_action_just_released("Inventory"):
		if visible:
			hide()
		else:
			show()
"""
extends Control

var items_to_load := [
	"res://inventory/items/coin.tres",
	"res://inventory/items/armor.tres",
	"res://inventory/items/bow.tres",
	"res://inventory/items/potion.tres",
	"res://inventory/items/staff.tres",
	"res://inventory/items/sword.tres"
]

func _ready():
	#creates a number of slots
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(32, 32))
		%Grid.add_child(slot)
	for i in items_to_load.size():
		var item = InventoryItem.new(null)
		item._init(load(items_to_load[i]))
		%Grid.get_child(i).add_child(item)

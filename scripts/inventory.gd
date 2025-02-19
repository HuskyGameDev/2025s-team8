extends Control

var items_to_load := [
	"res://inventory/items/coin.tres"
]

func _ready():
	#creates a number of slots
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(32, 32))
		%Grid.add_child(slot)
	for i in items_to_load.size():
		#Needs data to be passed to it, needs fix
		var item = InventoryItem.new(null)
		item._init(load(items_to_load[i]))
		%Grid.get_child(i).add_child(item)

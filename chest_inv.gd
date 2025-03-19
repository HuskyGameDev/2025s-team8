extends Control

#Will need way for player to interact so that it shows on screen
#but this can be worried about a bit later, will also need to be able to differentiate
#between different chests

var pos_items := [
	#Add items that can appear in chest
	"res://inventory/items/staff.tres",
	"res://inventory/items/sword.tres",
	"res://inventory/items/potion.tres"
]

var rand = RandomNumberGenerator.new()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	#Should create different seed on each run
	rand.randomize()
	
	#Creates the amount of space in the chest
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(32, 32))
		%Chest.add_child(slot)
	#This will most likely change, should fill chest randomly on each run
	#Will need way to tie rarity to the rand nums so that higher rarity = less spawn chance
	for i in pos_items.size():
		#var item = InventoryItem.new(null)
		#item._init(load(pos_items[i]))
		#%Chest.get_child(i).add_child(item)
		print(rand.randi_range(1, 100))

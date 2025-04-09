extends Control


#Will need way for player to interact so that it shows on screen
#but this can be worried about a bit later, will also need to be able to differentiate
#between different chests

var pos_items := [
	#Add items that can appear in chest
	preload("res://inventory/items/staff.tres").duplicate(),
	preload("res://inventory/items/sword.tres").duplicate(),
	preload("res://inventory/items/potion.tres").duplicate(),
	preload("res://inventory/items/coin.tres").duplicate()
]
#Min number of items that should be in the chest
var min_items: int = 2

var rand = RandomNumberGenerator.new()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	#Should create different seed on each run
	rand.randomize()
	hide()
	
	#Creates the amount of space in the chest
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(64, 64))
		#%Chest.add_child(slot)
		self.add_child(slot)
		
	var cur_items: int = 0
	var rand_value: int = 0
	var added: bool = false
	
	while cur_items < min_items:
		for i in pos_items.size():
			var item = InventoryItem.new(null)
			item._init(pos_items[i])
			
			rand_value = rand.randi_range(1, 100)
			
			if item.data.rarity == ItemData.Rarity.Common and rand_value > 20:
				#%Chest.get_child(i).add_child(item)
				added = true
			elif item.data.rarity == ItemData.Rarity.Uncommon and rand_value > 40:
				#%Chest.get_child(i).add_child(item)
				added = true
			elif item.data.rarity == ItemData.Rarity.Rare and rand_value > 60:
				#%Chest.get_child(i).add_child(item)
				added = true
			elif item.data.rarity == ItemData.Rarity.Legendary and rand_value > 80:
				#%Chest.get_child(i).add_child(item)
				added = true
				
			if added:
				self.get_child(cur_items).add_child(item)
				cur_items += 1

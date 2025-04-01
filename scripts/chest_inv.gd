extends Control


#Will need way for player to interact so that it shows on screen
#but this can be worried about a bit later, will also need to be able to differentiate
#between different chests

var pos_items := [
	#Add items that can appear in chest
	preload("res://inventory/items/staff.tres").duplicate(),
	preload("res://inventory/items/sword.tres").duplicate(),
	preload("res://inventory/items/potion.tres").duplicate()
]
#Min number of items that should be in the chest
var min_items: int = 1

var rand = RandomNumberGenerator.new()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	#Should create different seed on each run
	rand.randomize()
	
	#Creates the amount of space in the chest
	for i in 24:
		var slot := InventorySlot.new()
		slot.init(ItemData.Type.MAIN, Vector2(32, 32))
		#%Chest.add_child(slot)
		self.add_child(slot)
		
	var cur_items: int = 0
	var rand_value: int = 0
	
	while cur_items < min_items:
		for i in pos_items.size():
			var item = InventoryItem.new(null)
			item._init(pos_items[i])
			
			rand_value = rand.randi_range(1, 100)
			
			if item.data.Rarity.Common and rand_value > 20:
				#%Chest.get_child(i).add_child(item)
				self.get_child(i).add_child(item)
				cur_items += 1
			elif item.data.Rarity.Uncommon and rand_value > 40:
				#%Chest.get_child(i).add_child(item)
				self.get_child(i).add_child(item)
				cur_items += 1
			elif item.data.Rarity.Rare and rand_value > 60:
				#%Chest.get_child(i).add_child(item)
				self.get_child(i).add_child(item)
				cur_items += 1
			elif item.data.Rarity.Legendary and rand_value > 80:
				#%Chest.get_child(i).add_child(item)
				self.get_child(i).add_child(item)
				cur_items += 1
				
func _input(event):
	if event is InputEventKey and event.is_released():
		if event.keycode == KEY_G:
			if visible:
				hide()
				#print("Here")
			else:
				show()
				#print("Not here")

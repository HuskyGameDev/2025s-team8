class_name InventorySlot
extends PanelContainer

@export var type: ItemData.Type

func init(t: ItemData.Type, vec: Vector2) -> void:
	type = t
	custom_minimum_size = vec
	
func _can_drop_data(_at_position: Vector2, data: Variant) -> bool:
	if data is InventoryItem:
		if type == ItemData.Type.MAIN:
			if get_child_count() == 0:
				return true
			else:
				if type == data.get_parent().type:
					return true
				return get_child(0).data.type == data.data.type
		else:
			return data.data.type == type
	return false

#https://www.youtube.com/watch?v=UUzuUzPVNrE
#All prints are currently used for testing
#Data is the item being dragged, item is the item its being swapped with
func _drop_data(_at_position: Vector2, data: Variant) -> void:
	#if slot already has an item
	if get_child_count() > 0:
		var item := get_child(0)
		
		#If trying to place item back in slot that it is in
		if item == data:
			return
			
		#This is fine as there should only be 1 of each slot type other than main
		#The only feasible change to this would be for RANDOM for things like potions
		
		#If swapping with currently equipped item, reduce by the items stats
		if item.data.slot_type != ItemData.Type.MAIN:
			Player_Stats.dam -= item.data.damage
			Player_Stats.def -= item.data.defense
			item.data.slot_type = ItemData.Type.MAIN
		
		#If swapping with currently unequipped item, increase by data's stats
		if data.data.slot_type != ItemData.Type.MAIN:
			Player_Stats.dam += item.data.damage
			Player_Stats.def += item.data.defense
			item.data.slot_type = item.data.type
		
		#Put the item in data's place
		item.reparent(data.get_parent())
		
		#print("Damage: %d\nDefense: %d\nSlot: %s\n" % [Player_Stats.dam, Player_Stats.def, item.data.slot_type])
		
	#If equipping data, increase by data's stats
	if type != ItemData.Type.MAIN:
		Player_Stats.dam += data.data.damage
		Player_Stats.def += data.data.defense
		data.data.slot_type = data.data.type
		
		#print("Damage: %d\nDefense: %d\nSlot: %s\n" % [Player_Stats.dam, Player_Stats.def, data.data.slot_type])
		
	#If unequipping data, decrease by data's stats
	else:
		if data.data.slot_type != ItemData.Type.MAIN:
			Player_Stats.dam -= data.data.damage
			Player_Stats.def -= data.data.defense
			data.data.slot_type = ItemData.Type.MAIN
		
		#print("Damage: %d\nDefense: %d\nSlot: %s\n" % [Player_Stats.dam, Player_Stats.def, data.data.slot_type])
	print("Damage: %d\nDefense: %d\n" % [Player_Stats.dam, Player_Stats.def])
	data.reparent(self)
	
#Most likely a temp function but allows for the inventory to have its visibility changed
#When actual input is decided, this function will most likely be moved to the individual inventories
#Currently each inventory is set to a different key
func _input(event):
	if event is InputEventKey and event.is_released():
		if event.keycode == KEY_E and type != ItemData.Type.MAIN:
			if visible:
				hide()
				#print("Here")
			else:
				show()
				#print("Not here")

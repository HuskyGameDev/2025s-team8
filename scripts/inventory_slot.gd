class_name InventorySlot
extends PanelContainer

@export var type: ItemData.Type
var shown: bool = true

func init(t: ItemData.Type, vec: Vector2) -> void:
	type = t
	custom_minimum_size = vec
	
func _can_drop_data(at_position: Vector2, data: Variant) -> bool:
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
func _drop_data(at_position: Vector2, data: Variant) -> void:
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
			Temp.dam -= item.data.damage
			Temp.def -= item.data.defense
			item.data.slot_type = ItemData.Type.MAIN
		
		#If swapping with currently unequipped item, increase by data's stats
		if data.data.slot_type != ItemData.Type.MAIN:
			Temp.dam += item.data.damage
			Temp.def += item.data.defense
			item.data.slot_type = item.data.type
		
		#Put the item in data's place
		item.reparent(data.get_parent())
		
		#print("Damage: %d\nDefense: %d\nSlot: %s\n" % [Temp.dam, Temp.def, item.data.slot_type])
		
	#If equipping data, increase by data's stats
	if type != ItemData.Type.MAIN:
		Temp.dam += data.data.damage
		Temp.def += data.data.defense
		data.data.slot_type = data.data.type
		
		#print("Damage: %d\nDefense: %d\nSlot: %s\n" % [Temp.dam, Temp.def, data.data.slot_type])
		
	#If unequipping data, decrease by data's stats
	else:
		if data.data.slot_type != ItemData.Type.MAIN:
			Temp.dam -= data.data.damage
			Temp.def -= data.data.defense
			data.data.slot_type = ItemData.Type.MAIN
		
		#print("Damage: %d\nDefense: %d\nSlot: %s\n" % [Temp.dam, Temp.def, data.data.slot_type])
	print("Damage: %d\nDefense: %d\n" % [Temp.dam, Temp.def])
	data.reparent(self)
	
#Most likely a temp function but allows for the inventory to have its visibility changed
func _input(event):
	if event is InputEventKey and event.is_released():
		if event.keycode == KEY_E:
			if shown:
				hide()
				shown = false
				#print("Here")
			else:
				show()
				shown = true
				#print("Not here")

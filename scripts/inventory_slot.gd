class_name InventorySlot
extends PanelContainer

@export var type: ItemData.Type
var shown: bool = true

#Following are temps to test changes to player attack and defense
#Each slot has own stats for now, should not be this way
var dam: int
var def: int

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
#Issues with properly changing stats, unsure of exact reason currently
#This is using values for each slot indiviualy, not overall
#Need checks for the types of slots

"""
Current description of issues with stats:
	Replacing an item in the same spot will decrease stats (may be fixed)
	Moving item in unequipped slots will still decreas stats
	Swapping items produces a few errors that seem to work together
		Will double reduce in slot being replaced
	
	Main issue seems to be in regard to reducing when moving among unequipped
	May need to store the current type of slot that the item is in to better determine when to decrease
	Increasing seems to work fine currently
"""

func _drop_data(at_position: Vector2, data: Variant) -> void:
	if get_child_count() > 0:
		var item := get_child(0)
		
		if item == data:
			return
			
		#decrease player stats, when item in slot
		dam -= item.data.damage
		def -= item.data.defense
		
		print("Damage: %d\nDefense: %d\nItem: %s\n" % [dam, def, item.data.name])
		
		item.reparent(data.get_parent())
	if type != ItemData.Type.MAIN:
		#increase player stats, when placing into equipped slots
		dam += data.data.damage
		def += data.data.defense
		
		print("Damage: %d\nDefense: %d\nItem: %s\n" % [dam, def, data.data.name])
		
	else:
		#again decreas player stats, when placing into empty slot
		
		dam -= data.data.damage
		def -= data.data.defense
		
		print("Damage: %d\nDefense: %d\nItem: %s\n" % [dam, def, data.data.name])
		
	data.reparent(self)
	
#Most likely a temp function but allows for the inventory to have its visibility changed
func _input(event):
	if event is InputEventKey and event.is_released():
		if event.keycode == KEY_E:
			if shown:
				hide()
				shown = false
				print("Here")
			else:
				show()
				shown = true
				print("Not here")

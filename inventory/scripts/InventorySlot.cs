using Godot;
using System;

public partial class InventorySlot : TextureRect
{
	[Export]
	private Item.Type type = Item.Type.INVALID;

    public override void _Ready()
	{
		
        if (type != Item.Type.INVALID)
        {
            UpdateImage();
        }
    }

    public void Init(Item.Type t, Vector2 v)
    {
        if (t == Item.Type.INVALID) return; // Not allowed, keep what we had before

        type = t;
        this.CustomMinimumSize = v;

        UpdateImage();
    }

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		InventoryItem otherItem = (InventoryItem)data;
		if (type == Item.Type.MAIN)
		{
			if (this.GetChildCount() == 0)
			{
				return true;
			}
			else
			{
				if (type == ((InventorySlot)((otherItem).GetParent())).type)
				{
					return true;
				}
				return ((InventoryItem)this.GetChild(0)).data.GetItemType() == otherItem.data.GetItemType();
			}
		} else {
			return (otherItem.data.GetItemType() == type);
		}
	}

	// https://www.youtube.com/watch?v=UUzuUzPVNrE
	// All prints are currently used for testing
	// Data is the item being dragged, item is the item its being swapped with
	public override void _DropData(Vector2 atPositition, Variant data)
	{
		InventoryItem otherItem = (InventoryItem)data;
		InventorySlot otherSlot = (InventorySlot)otherItem.GetParent();
		Node playerStats = this.GetTree().GetRoot().GetNode("./Player_Stats");

		// If we already have an item
		if (this.GetChildCount() > 0)
		{
			InventoryItem ourItem = (InventoryItem)this.GetChild(0);

			// Trying to place item back in its original slot?
			if (ourItem == otherItem)
			{
				return;
			}

			// This is fine, as there should only be 1 of each slot type other than main
			// The only feasible change to this would be RANDOm for things like potions
			// maybe its fine?

			// Is this an equipment slot?
			// (Remove our stats from our slot?)
			if (this.type != Item.Type.MAIN)
			{
				if (ourItem.data.GetItemType() == Item.Type.WEAPON)
					playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)ourItem.data).GetDamage());
				if (ourItem.data.GetItemType() == Item.Type.ARMOR)
					playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)ourItem.data).GetDefense());
			}

			// Is the other item in an equipment slot?
			// (Add our stats to the other slot?)
			if (otherSlot.GetSlotType() != Item.Type.MAIN)
			{
				if (ourItem.data.GetItemType() == Item.Type.WEAPON)
					playerStats.Set("dam", (int)playerStats.Get("dam") + ((WeaponItem)ourItem.data).GetDamage());
				if (ourItem.data.GetItemType() == Item.Type.ARMOR)
					playerStats.Set("def", (int)playerStats.Get("def") + ((ArmorItem)ourItem.data).GetDefense());
			}

			// Put our item in the other item's place
			ourItem.Reparent(otherSlot);
			ourItem.Position = new Vector2(0, 0);
		}

		// Is this an equipment slot?
		// (Add the other stats to our slot?)
		if (this.type != Item.Type.MAIN)
		{
			if (otherItem.data.GetItemType() == Item.Type.WEAPON)
				playerStats.Set("dam", (int)playerStats.Get("dam") + ((WeaponItem)otherItem.data).GetDamage());
			if (otherItem.data.GetItemType() == Item.Type.ARMOR)
				playerStats.Set("def", (int)playerStats.Get("def") + ((ArmorItem)otherItem.data).GetDefense());
		}

		// If the other item in an equipment slot?
		// (Remove the other stats from the other slot?)
		if (otherSlot.GetSlotType() != Item.Type.MAIN)
		{
			if (otherItem.data.GetItemType() == Item.Type.WEAPON)
				playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)otherItem.data).GetDamage());
			if (otherItem.data.GetItemType() == Item.Type.ARMOR)
				playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)otherItem.data).GetDefense());
		}

		GD.Print($"Damage: {playerStats.Get("dam")}\nDefense: {playerStats.Get("def")}\n");

		// put the other item here
		otherItem.Reparent(this);
		otherItem.Position = new Vector2(0, 0);
		// Make sure both slots have the correct bkg
		UpdateImage();
		otherSlot.UpdateImage();
	}

	// Sets this slot using an item directly, instead of swapping
	// with another InventorySlot
	public void SetItem(Item newItem)
	{
		if (this.type == Item.Type.INVALID)
		{
			GD.Print("InventorySlot has no type!");
			return; // do not allow
		}
		if (newItem.GetItemType() != Item.Type.MAIN && this.type != newItem.GetItemType())
		{
			GD.Print("Item cannot go in this slot!");
			return; // do not allow
		}

		Node playerStats = this.GetTree().GetRoot().GetNode("./Player_Stats");

		// If slot already has an item
		if (this.GetChildCount() > 0)
		{
			InventoryItem ourItem = (InventoryItem)this.GetChild(0);

			// Is this an equipment slot? (need to adjust stats?)
			if (this.type != Item.Type.MAIN)
			{
				// decrease the player's stats by the item being replaced
				if (ourItem.data.GetItemType() == Item.Type.WEAPON)
					playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)ourItem.data).GetDamage());
				if (ourItem.data.GetItemType() == Item.Type.ARMOR)
					playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)ourItem.data).GetDefense());
			}

			// Destroy the current item
			ourItem.QueueFree();
		}


		// Is this an equipment slot? (need to adjust stats?)
		if (this.type != Item.Type.MAIN)
		{
			if (newItem.GetItemType() == Item.Type.WEAPON)
				playerStats.Set("dam", (int)playerStats.Get("dam") + ((WeaponItem)newItem).GetDamage());
			if (newItem.GetItemType() == Item.Type.ARMOR)
				playerStats.Set("def", (int)playerStats.Get("def") + ((ArmorItem)newItem).GetDefense());
		}
		GD.Print($"Damage: {playerStats.Get("dam")}\nDefense: {playerStats.Get("def")}\n");


		// Add the new item to this Slot
		InventoryItem invItem = new InventoryItem();
		invItem.Init(newItem);
		this.AddChild(invItem);
		invItem.Position = new Vector2(0, 0);

		// Make sure both we have the correct bkg
		UpdateImage();
	}
	
	public Item.Type GetSlotType()
    {
		return type;
    }
    
    private void UpdateImage()
	{
		if (this.GetChildCount() > 0)
		{
			// Item is already here, do not show an outline
			this.Texture = (Texture2D)(GD.Load("res://Assets/Inventory/InventorySlot.png"));
			return;
		}
		// ...else, depends on our slot type
        switch (type)
        {
            case Item.Type.WEAPON:
                this.Texture = (Texture2D)(GD.Load("res://Assets/Inventory/InventorySlot_Weapon.png"));
                break;
            case Item.Type.ARMOR:
                this.Texture = (Texture2D)(GD.Load("res://Assets/Inventory/InventorySlot_Armor.png"));
                break;
            case Item.Type.RANDOM:
                this.Texture = (Texture2D)(GD.Load("res://Assets/Inventory/InventorySlot_Consumable.png"));
                break;
            default:
                this.Texture = (Texture2D)(GD.Load("res://Assets/Inventory/InventorySlot.png"));
                break;
        }
    }
}

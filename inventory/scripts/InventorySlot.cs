using Godot;
using System;

public partial class InventorySlot : PanelContainer
{
    [Export]
    public Item.Type type;

    public void Init(Item.Type t, Vector2 v)
    {
        type = t;
        this.CustomMinimumSize = v;
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
        Node playerStats = this.GetTree().GetRoot().GetNode("./Player_Stats");

        // If slot already has an item
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

            // If swapping with currently equipped item, reduce by item's stats
            if (ourItem.data.GetSlotType() != Item.Type.MAIN)
            {
                if (ourItem.data.GetItemType() == Item.Type.WEAPON)
                    playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)ourItem.data).GetDamage());
                if (ourItem.data.GetItemType() == Item.Type.ARMOR)
                    playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)ourItem.data).GetDefense());
                ourItem.data.SetSlotType(Item.Type.MAIN);
            }

            // If swapping with currently unequipped item, increase by data's stats
            if (otherItem.data.GetSlotType() != Item.Type.MAIN)
            {
                if (ourItem.data.GetItemType() == Item.Type.WEAPON)
                    playerStats.Set("dam", (int)playerStats.Get("dam") + ((WeaponItem)ourItem.data).GetDamage());
                if (ourItem.data.GetItemType() == Item.Type.ARMOR)
                    playerStats.Set("def", (int)playerStats.Get("def") + ((ArmorItem)ourItem.data).GetDefense());
                ourItem.data.SetSlotType(Item.Type.MAIN);
            }

            // Put our item in the other item's place
            ourItem.Reparent(otherItem.GetParent());
        }

        // If equpping other item, increase by other item's stats
        else if (type != Item.Type.MAIN)
        {
            if (otherItem.data.GetItemType() == Item.Type.WEAPON)
                playerStats.Set("dam", (int)playerStats.Get("dam") + ((WeaponItem)otherItem.data).GetDamage());
            if (otherItem.data.GetItemType() == Item.Type.ARMOR)
                playerStats.Set("def", (int)playerStats.Get("def") + ((ArmorItem)otherItem.data).GetDefense());
            otherItem.data.SetSlotType(otherItem.data.GetItemType());
        }

        // If unequpping other item, reduce by other item's stats
        else if (otherItem.data.GetSlotType() != Item.Type.MAIN)
        {
            if (otherItem.data.GetItemType() == Item.Type.WEAPON)
                playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)otherItem.data).GetDamage());
            if (otherItem.data.GetItemType() == Item.Type.ARMOR)
                playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)otherItem.data).GetDefense());
            otherItem.data.SetSlotType(Item.Type.MAIN);
        }
        GD.Print($"Damage: {playerStats.Get("dam")}\nDefense: {playerStats.Get("def")}\n");

        // put the other item here
        otherItem.Reparent(this);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey && Input.IsActionJustReleased("Inventory") && type != Item.Type.MAIN)
        {
            this.Visible = !this.Visible;
        }
    }
}

using Godot;
using System;

public partial class InventorySlot : PanelContainer
{
    [Export]
    public Item.Type type;

    public void Init(Item.Type t, Vector2 v)
    {
        type = t;
        this.CustomMininumSize = v;
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data is InventoryItem) return false;
        if (type == Item.Type.MAIN)
        {
            if (this.GetChildCount() == 0)
            {
                return true;
            }
            else
            {
                if (type == ((InventorySlot)(((InventoryItem)data).GetParent())).type)
                {
                    return true;
                }
                return ((InventoryItem)this.GetChild(0)).data.GetItemType() == ((InventoryItem)data).data.GetItemType();
            }
        }
        return false;
    }

    // https://www.youtube.com/watch?v=UUzuUzPVNrE
    // All prints are currently used for testing
    // Data is the item being dragged, item is the item its being swapped with
    public override void _DropData(Vector2 atPositition, Variant data)
    {
        InventoryItem iiData = (InventoryItem)data;
        GDScript playerStats = this.GetTree().GetRoot().GetNode<GDScript>("./Player_Stats");

        // If slot already has an item
        if (this.GetChildCount() > 0)
        {
            InventoryItem item = (InventoryItem)this.GetChild(0);

            // Trying to place item back in its original slot?
            if (item == iiData)
            {
                return;
            }

            // This is fine, as there should only be 1 of each slot type other than main
            // The only feasible change to this would be RANDOm for things like potions

            // If swapping with currently equipped item, reduce by item's stats
            if (item.data.GetSlotType() != Item.Type.MAIN)
            {
                if (item.data.GetItemType() == Item.Type.WEAPON)
                    playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)item.data).GetDamage());
                if (item.data.GetItemType() == Item.Type.ARMOR)
                    playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)item.data).GetDefense());
                item.data.SetSlotType(Item.Type.MAIN);
            }

            // If swapping with currently unequipped item, increase by data's stats
            if (iiData.data.GetSlotType() != Item.Type.MAIN)
            {
                if (item.data.GetItemType() == Item.Type.WEAPON)
                    playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)item.data).GetDamage());
                if (item.data.GetItemType() == Item.Type.ARMOR)
                    playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)item.data).GetDefense());
                item.data.SetSlotType(Item.Type.MAIN);
            }

            // Put the item in data's place
            ((InventoryItem)item).Reparent(((InventoryItem)data).GetParent());
        }

        // If equpping data, increase by data's stats
        if (type != Item.Type.MAIN)
        {
            if (iiData.data.GetItemType() == Item.Type.WEAPON)
                playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)iiData.data).GetDamage());
            if (iiData.data.GetItemType() == Item.Type.ARMOR)
                playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)iiData.data).GetDefense());
            iiData.data.SetSlotType(iiData.data.GetItemType());
        }

        // If unequpping data, reduce by data's stats
        else
        {
            if (iiData.data.GetItemType() == Item.Type.WEAPON)
                playerStats.Set("dam", (int)playerStats.Get("dam") - ((WeaponItem)iiData.data).GetDamage());
            if (iiData.data.GetItemType() == Item.Type.ARMOR)
                playerStats.Set("def", (int)playerStats.Get("def") - ((ArmorItem)iiData.data).GetDefense());
            iiData.data.SetSlotType(Item.Type.MAIN);
        }
        GD.Print($"Damage: {playerStats.Get("dam")}\nDefense: {playerStats.Get("def")}\n");

        ((InventoryItem)data).Reparent(this);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey && Input.IsActionJustReleased("Inventory") && type != Item.Type.MAIN)
        {
            this.Visible = !this.Visible;
        }
    }
}

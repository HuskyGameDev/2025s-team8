using Godot;
using System;

public partial class PlayerInv : Control
{
    Item[] itemsToLoad = {
        Items.coin,
        Items.armor,
        Items.bow,
        Items.potion,
        Items.staff,
        Items.wooden_sword
    };

    public override void _Ready()
    {
        // Generate the inventory slots
        for (int i = 0; i < 24; i++)
        {
            InventorySlot slot = new InventorySlot();
            slot.Init(Item.Type.MAIN, new Vector2(64, 64));
            this.AddChild(slot);
        }

        for (int i = 0; i < itemsToLoad.Length; i++)
        {
            InventoryItem item = new InventoryItem();
            item.Init(itemsToLoad[i]);
            this.GetChild(i).AddChild(item);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey && Input.IsActionJustReleased("Inventory"))
        {
            this.Visible = !this.Visible;
        }
    }
}

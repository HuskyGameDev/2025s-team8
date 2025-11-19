using Godot;
using System;

public partial class PlayerInv : Control
{
    private InventorySlot wpnSlot;
    private InventorySlot wpnSlot2;
    private InventorySlot armSlot;
    private InventorySlot rndSlot;
    private InventorySlot rndSlot2;
    private Control invGrid;

    public override void _Ready()
    {
        // Get the refs to children set up
        wpnSlot = this.GetNode<InventorySlot>("./WeaponSlot");
        wpnSlot2 = this.GetNode<InventorySlot>("./WeaponSlot2");
        armSlot = this.GetNode<InventorySlot>("./ArmorSlot");
        rndSlot = this.GetNode<InventorySlot>("./RandomSlot");
        rndSlot2 = this.GetNode<InventorySlot>("./RandomSlot2");
        invGrid = this.GetNode<Control>("./Player_inv");

        // Generate the inventory slots
        for (int i = 0; i < 24; i++)
        {
            InventorySlot slot = new InventorySlot();
            slot.Init(Item.Type.MAIN, new Vector2(64, 64));
            invGrid.AddChild(slot);
        }

        // Get the PlayerData
        Node root = this.GetTree().GetRoot();
        PlayerData pd = root.GetNode<PlayerData>("./PlayerData");
        string[] inv = pd.save.inv;

        // Set all the equipment slots
        Item wpnItem = Items.GetItem(pd.save.weaponId);
        if (wpnItem != null)
            wpnSlot.SetItem(wpnItem);
        Item wpnItem2 = Items.GetItem(pd.save.weapon2Id);
        if (wpnItem2 != null)
            wpnSlot2.SetItem(wpnItem);
        Item armItem = Items.GetItem(pd.save.armorId);
        if (armItem != null)
            armSlot.SetItem(armItem);
        Item rndItem = Items.GetItem(pd.save.consumableId);
        if (rndItem != null)
            rndSlot.SetItem(rndItem);
        Item rndItem2 = Items.GetItem(pd.save.consumable2Id);
        if (rndItem2 != null)
            rndSlot2.SetItem(rndItem);

        // Load the inventory from player data
        for (int i = 0; i < inv.Length; i++)
        {
            Item item = Items.GetItem(inv[i]);
            if (item == null) continue;
            InventoryItem invItem = new InventoryItem();
            invItem.Init(item);
            invGrid.GetChild(i).AddChild(invItem);
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

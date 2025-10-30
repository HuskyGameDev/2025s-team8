using Godot;
using System;

public partial class ChestInv : Area2D
{
    private Control invRoot;
    private bool playerNearby = false;

    [Export]
    private string lootTableName;

    // Mininum amount of items that should be in the chest
    private int min_items = 2;

    private RandomNumberGenerator rand = new RandomNumberGenerator();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get the player inventory location
        invRoot = this.GetNode<Control>("./CanvasLayer/Chest");
        if (invRoot == null)
        {
            GD.PrintErr("Cannot find chest inventory location? (Did the name change?)");
            return;
        }
        invRoot.Hide();

        // Set the Area2D enter/exit triggers
        this.BodyEntered += (body) =>
        {
            if (body.Name == "Player")
            {
                playerNearby = true;
            }
        };
        this.BodyExited += (body) =>
        {
            if (body.Name == "Player")
            {
                playerNearby = false;
            }
            invRoot.Hide();
        };
        
        // Set up the InventorySlots
        for (int i = 0; i < 24; i++)
        {
            InventorySlot s = new InventorySlot();
            s.Init(Item.Type.MAIN, new Vector2(64, 64));
            invRoot.AddChild(s);
        }

        // Get loot from given loot table
        if (lootTableName == null)
        {
            GD.PrintErr("Chest has no loot table to generate from!");
            return;
        }
        Item[] loot = LootGenerator.GenerateLoot(lootTableName);

        // Add InventoryItems into the inventory
        for (int i = 0; i < loot.Length; i++)
        {
            InventoryItem item = new InventoryItem();
            item.Init(loot[i]);
            invRoot.GetChild(i).AddChild(item);
        }
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey && Input.IsActionJustPressed("Interact"))
        {
            if (playerNearby)
            {
                invRoot.Visible = !invRoot.Visible;
            }
        }
    }
}

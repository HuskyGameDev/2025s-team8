using Godot;
using System;

public partial class ChestInv : Control
{

    private Item[] item_pool = {
        Items.staff,
        Items.sword,
        Items.potion,
        Items.coin
    };

    // Mininum amount of items that should be in the chest
    private int min_items = 2;

    private RandomNumberGenerator rand = new RandomNumberGenerator();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        rand.Randomize(); // Create a different seed each run
        this.Hide();

        // Create the amount of space in the chest
        for (int i = 0; i < 24; i++) {
            InventorySlot s = new InventorySlot();
            s.Init(Item.Type.MAIN, new Vector2(64, 64));
            this.AddChild(s);
        }

        int cur_items = 0;
        int rand_val = 0;
        bool added = false;

        while (cur_items < min_items)
        {
            for (int i = 0; i < item_pool.Length; i++)
            {
                InventoryItem item = new InventoryItem();
                item.Init(item_pool[i]);

                rand_val = rand.RandiRange(1, 100);

                if (rand_val > 80 && item.data.GetRarity() == Item.Rarity.Legendary)
                {
                    added = true;
                }
                else if (rand_val > 60 && item.data.GetRarity() == Item.Rarity.Rare)
                {
                    added = true;
                }
                else if (rand_val > 40 && item.data.GetRarity() == Item.Rarity.Uncommon)
                {
                    added = true;
                }
                else if (rand_val > 20 && item.data.GetRarity() == Item.Rarity.Common)
                {
                    added = true;
                }

                if (added)
                {
                    this.GetChild(cur_items).AddChild(item);
                    cur_items++;
                }
            }
        }
    }
}

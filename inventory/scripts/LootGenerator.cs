using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

public class LootGenerator
{
    public static Item[] GenerateLoot(string name)
    {
        // Get the JSON file
        using var f = FileAccess.Open("res://Data/LootTables/" + name + ".json", FileAccess.ModeFlags.Read);
        string fData = f.GetAsText();

        LootTable table = JsonSerializer.Deserialize<LootTable>(fData);
        Random rnd = new Random();
        List<Item> lootResults = new List<Item>();

        foreach (LootPool pool in table.pools)
        {
            // Get the amount of rolls we will be performing
            int rolls = pool.rolls;
            if (rolls == 0) // using min-max mode instead
            {
                // minRolls and maxRolls are not being fully validated lmao
                if (pool.maxRolls == 0)
                {
                    GD.Print("'" + name + ".json' loot table is improperly formatted!");
                    return new Item[0];
                }

                // Get the amount of rolls we will be doing
                rolls = rnd.Next(pool.minRolls, pool.maxRolls + 1); // excl upper bound
            }
            int minRolls = pool.minRolls;
            int maxRolls = pool.maxRolls;

            // Get the total weight
            int maxWeight = 0;
            foreach (LootEntry item in pool.items)
            {
                maxWeight += item.weight;
            }

            // Choose a random number, and find the item at that weighted position
            for (int i = 0; i < rolls; i++)
            {
                int val = rnd.Next(0, maxWeight);
                foreach (LootEntry item in pool.items)
                {
                    val -= item.weight;
                    if (val < 0)
                    {
                        // also no validation that the id is valid (there may be nulls)
                        lootResults.Add(Items.GetItem(item.id));
                        break;
                    }
                }
            }
        }

        // Convert to an array (hopefully array instead of list helps w/ GDScript compat)
        Item[] itemResults = new Item[lootResults.Count];
        lootResults.CopyTo(itemResults);
        return itemResults;
    }

    private class LootTable
    {
        public LootPool[] pools { get; set; }
    }

    private class LootPool
    {
        public LootEntry[] items { get; set; }
        public int rolls { get; set; }
        public int minRolls { get; set; }
        public int maxRolls { get; set; }
    }
    
    private class LootEntry
    {
        public string id { get; set; }
        public int weight { get; set; }
    }
}



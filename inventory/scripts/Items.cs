using Godot;
using System;
using System.Collections.Generic;

public class Items
{
    private static Dictionary<string, Item> items = new Dictionary<string, Item>();

    // Swords
    public static WeaponItem wooden_sword = (WeaponItem)RegisterItem(new WeaponItem("wooden_sword", "Wooden Sword", Item.Rarity.Common,
        "A wooden sword! It's not much, but it's something.", 1, WeaponItem.WeaponType.SWORD));
    public static WeaponItem rusty_sword = (WeaponItem)RegisterItem(new WeaponItem("rusty_sword", "Rusty Sword", Item.Rarity.Common,
        "Looks sturdy enough.", 2, WeaponItem.WeaponType.SWORD));
    public static WeaponItem iron_sword = (WeaponItem)RegisterItem(new WeaponItem("iron_sword", "Iron Sword", Item.Rarity.Uncommon,
        "Sturdy and well forged. The blade is still relatively sharp.", 3, WeaponItem.WeaponType.SWORD));
    public static WeaponItem ice_sword = (WeaponItem)RegisterItem(new WeaponItem("ice_sword", "Ice Blade", Item.Rarity.Epic,
        "Cold to the touch.", 5, WeaponItem.WeaponType.SWORD));
    public static WeaponItem lava_sword = (WeaponItem)RegisterItem(new WeaponItem("lava_sword", "Magmatic Blade", Item.Rarity.Legendary,
        "Almost hurts to hold.", 9, WeaponItem.WeaponType.SWORD));


    // Spears
    public static WeaponItem stick = (WeaponItem)RegisterItem(new WeaponItem("stick", "A Big Stick", Item.Rarity.Common,
        "Plenty long enough to poke your enemies from a distance.", 1, WeaponItem.WeaponType.SPEAR));
    public static WeaponItem iron_spear = (WeaponItem)RegisterItem(new WeaponItem("iron_spear", "Iron Spear", Item.Rarity.Uncommon,
        "The tip is surprisingly sharp.", 4, WeaponItem.WeaponType.SPEAR));
    public static WeaponItem icicle = (WeaponItem)RegisterItem(new WeaponItem("icicle", "Icicle", Item.Rarity.Rare,
        "Somehow, it isn't melting.", 7, WeaponItem.WeaponType.SPEAR));

    // Bows
    public static WeaponItem crude_bow = (WeaponItem)RegisterItem(new WeaponItem("crude_bow", "Crude Bow", Item.Rarity.Common,
        "Made from a flexible stick and cordage. It isn't great, but it works.", 1, WeaponItem.WeaponType.BOW));
    public static WeaponItem bow = (WeaponItem)RegisterItem(new WeaponItem("bow", "Bow", Item.Rarity.Uncommon,
        "A standard bow. It's fairly weathered.", 5, WeaponItem.WeaponType.BOW));

    // Staves
    public static WeaponItem staff = (WeaponItem)RegisterItem(new WeaponItem("staff", "Staff", Item.Rarity.Rare,
        "A magical staff! I wonder what you can do with this.", 7, WeaponItem.WeaponType.STAFF));

    // Armors
    public static ArmorItem armor = (ArmorItem)RegisterItem(new ArmorItem("armor", "Armor", Item.Rarity.Common,
        "A base set of armor. It isn't much but it will get the job done!", 10));

    // Misc
    public static Item coin = RegisterItem(new Item("coin", "Coin", Item.Type.RANDOM, Item.Rarity.Common,
        "It's from a currency you don't recognize."));
    // Misc: Healing Items
    public static Item bandage = RegisterItem(new HealingItem("bandage", "Bandage", Item.Rarity.Common,
        "Some basic first aid can do a lot for an adventurer.", 2));
    public static Item small_potion = RegisterItem(new HealingItem("small_potion", "Small Potion", Item.Rarity.Epic,
        "It's quite common to see potions be rationed like this, with how expensive they can get.", 5));
    public static Item potion = RegisterItem(new HealingItem("potion", "Potion", Item.Rarity.Legendary,
        "The pinnacle of magical medicine.", 15));


    // Methods to create & store items in items dict
    protected static Item RegisterItem(Item i)
    {
        items.Add(i.GetId(), i);
        return i;
    }

    public static Item GetItem(string id)
    {
        Item item;
        bool found = items.TryGetValue(id, out item);
        if (!found) return null;
        return item;
    }

}
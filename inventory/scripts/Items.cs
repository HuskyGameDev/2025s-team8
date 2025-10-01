using Godot;
using System;
using System.Collections.Generic;

public class Items
{
    public static Dictionary<string, Item> items = new Dictionary<string, Item>();

    public static WeaponItem sword = (WeaponItem)RegisterItem(new WeaponItem("sword", "Sword", Item.Type.WEAPON, Item.Rarity.Rare,
        "A wooden sword! It's not much, but it's something.", 5));

    public static ArmorItem armor = (ArmorItem)RegisterItem(new ArmorItem("armor", "Armor", Item.Type.ARMOR, Item.Rarity.Common,
        "A base set of armor. It isn't much but it will get the job done!", 10));

    public static WeaponItem bow = (WeaponItem)RegisterItem(new WeaponItem("bow", "Bow", Item.Type.WEAPON, Item.Rarity.Uncommon,
        "A basic bow. It looks fairly weathered.", 8));

    public static Item coin = RegisterItem(new Item("coin", "Coin", Item.Type.RANDOM, Item.Rarity.Common,
        "This is a coin!"));

    public static Item potion = RegisterItem(new Item("potion", "Potion", Item.Type.RANDOM, Item.Rarity.Legendary,
        "This is a potion! It has no effect!"));

    public static WeaponItem staff = (WeaponItem)RegisterItem(new WeaponItem("staff", "Staff", Item.Type.WEAPON, Item.Rarity.Rare,
        "A magical staff! I wonder what you can do with this.", 7));


    // Methods to create & store items in items dict
    public static Item RegisterItem(Item i)
    {
        items.Add(i.GetId(), i);
        return i;
    }

}
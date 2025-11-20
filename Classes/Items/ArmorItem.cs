using Godot;
using System;

public class ArmorItem : Item
{
    protected int defense;

    public ArmorItem(string i, string n, Rarity r, string desc, int def) : base(i, n, Item.Type.ARMOR, r, desc)
    {
        defense = def;
    }

    // ArmorItem Methods
    public override string GetTooltip()
    {
        return $"{this.name}\n{this.description}\nDefense: {this.defense}";
    }

    // ArmorItem Getters

    public int GetDefense()
    {
        return defense;
    }
}

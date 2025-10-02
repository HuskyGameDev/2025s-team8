using Godot;
using System;

public partial class ArmorItem : Item
{
    protected int defense;

    public ArmorItem(string i, string n, Type t, Rarity r, string desc, int def) : base(i, n, t, r, desc)
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

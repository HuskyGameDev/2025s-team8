using Godot;
using System;

public partial class WeaponItem : Item
{
    protected int damage;

    public WeaponItem(string i, string n, Type t, Rarity r, string desc, int dmg) : base(i, n, t, r, desc)
    {
        damage = dmg;
    }

    // WeaponItem Methods
    public override string GetTooltip()
    {
        return $"{this.name}\n{this.description}\nDamage: {this.damage}";
    }

    // WeaponItem Getters
    public int GetDamage()
    {
        return damage;
    }
}

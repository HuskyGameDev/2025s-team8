using Godot;
using System;

public class WeaponItem : Item
{
    protected int damage;
    protected WeaponType wpnType;

    public WeaponItem(string i, string n, Rarity r, string desc, int dmg, WeaponType wt) : base(i, n, Item.Type.WEAPON, r, desc)
    {
        damage = dmg;
        wpnType = wt;
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
    public WeaponType GetWeaponType()
    {
        return wpnType;
    }

    public enum WeaponType
    {
        SWORD,
        SPEAR,
        BOW,
        STAFF,
        INVALID = -1
    }
}

using Godot;
using System;

public class HealingItem : Item
{
    protected int healAmt;

    public HealingItem(string i, string n, Rarity r, string desc, int h) : base(i, n, Item.Type.RANDOM, r, desc)
    {
        healAmt = h;
    }
    
    // HealingItem Getters
    public int GetHealingAmount()
    {
        return healAmt;
    }
}
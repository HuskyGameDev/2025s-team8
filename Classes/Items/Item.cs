using Godot;
using System;

public class Item
{
    protected string id;
    protected string name;
    protected Type type;
    protected Rarity rarity;
    protected string description;
    protected Texture2D tex;
    protected Type slotType;

    public Item(string i, string n, Type t, Rarity r, string desc)
    {
        id = i;
        name = n;
        type = t;
        rarity = r;
        description = desc;
        this.tex = (Texture2D)GD.Load("res://Assets/Item/" + id + ".png");
        slotType = Item.Type.MAIN;
    }

    // Item Methods
    public virtual string GetTooltip()
    {
        return $"{this.name}\n{this.description}";
    }

    public void SetSlotType(Type t)
    {
        slotType = t;
    }

    // Item Getters

    public string GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public Type GetItemType() {
        return type;
    }

    public Rarity GetRarity()
    {
        return rarity;
    }

    public string GetDesc()
    {
        return description;
    }

    public Texture2D GetTex()
    {
        return tex;
    }

    public Type GetSlotType()
    {
        return slotType;
    }

    // Item Enums
    public enum Type
    {
        MAIN,
        WEAPON,
        ARMOR,
        RANDOM,
        INVALID = -1
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}

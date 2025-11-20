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

    public Item(string i, string n, Type t, Rarity r, string desc)
    {
        id = i;
        name = n;
        type = t;
        rarity = r;
        description = desc;
        this.tex = (Texture2D)GD.Load("res://Assets/Item/" + id + ".png");
    }

    // Item Methods
    public virtual string GetTooltip()
    {
        return $"{this.name}\n{this.description}";
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

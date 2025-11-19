using Godot;
using System;
// Sfx subclass for grouping multiple sounds under one id
public class SfxGroup : Sfx {
    protected string[] fns = null;
    public SfxGroup(string[] fns) : base("") {
        this.fns = fns;
    }

    public override AudioStream Get() {
        if (fns.Length == 0) return null;
        int rand = new Random().Next(0, fns.Length);
        return GD.Load<AudioStream>(SFX_LOC + fp + "/" + fns[rand]);
    }
}
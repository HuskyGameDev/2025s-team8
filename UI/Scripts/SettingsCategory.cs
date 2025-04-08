using Godot;
using System;
using System.Collections.Generic;

public abstract partial class SettingsCategory : Control {
    public abstract void ApplyChanges();

    public abstract void DiscardChanges();

    // protected void OnChangeMade() {
    //     SettingsMenu sMenu = this.GetNode<SettingsMenu>("../../");
    //     sMenu.WhenChangeMade();
    // }

    public abstract void RevertToDefaults();

    public abstract void CfgInit();
}
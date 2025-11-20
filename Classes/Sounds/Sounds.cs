using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

// Global reference class for all audio
public class Sounds {
    // Since no sounds actually exist yet, theese are simply examples
    public static readonly Sfx UI_Click = new Sfx("UI/Click.wav");
    public static readonly Sfx UI_Confirm = new Sfx("UI/Confirm.wav");
    public static readonly Sfx UI_Back = new Sfx("UI/Back.wav");

    public static readonly Sfx Player_Attack = new Sfx("Player/Temp_Atk.wav");
    public static readonly Sfx Player_Hit = new SfxGroup(new string[]{"Player/Hurt/Hurt1.wav", "Player/Hurt/Hurt2.wav"});
    // SfxGroups are stored as an Sfx so that the SFXManager can use them.
    // They will act like SfxGroups when using Get() (we love Dynamic Binding).

    // There was also a music component for this, but Mack has already made a new system, so
    // I did not bring that over.
}

using Godot;
using System;

public partial class LoadScreen : Control
{
    // "Loading..." variables
    private Label l;
    private int dotCount = 0;
    private double curTime = 0;
    private const double MAX_TIME = 0.5;

    // Load process vairables
    private string scenePath = "";
    private Mode loadMode = Mode.INVALID;

    public override void _Ready()
    {
        l = this.GetNode<Label>("./Label");
    }

    public void Init(string loadScene, Mode loadMode)
    {
        scenePath = loadScene;
    }

    public override void _Process(double delta)
    {
        // Handles the "Loading..." text
        curTime += delta;
        if (curTime > MAX_TIME)
        {
            curTime -= MAX_TIME;
            dotCount = (dotCount + 1) % 4;

            string s = "Loading";
            for (int i = 0; i < dotCount; i++)
            {
                s += ".";
            }
            l.Text = s;
        }
    }

    public enum Mode
    {
        LOAD,
        SAVE_LOAD,
        LOAD_SAVE,
        INVALID = -1
    }
}

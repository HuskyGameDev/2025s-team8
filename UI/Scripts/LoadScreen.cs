using Godot;
using System;

public partial class LoadScreen : Control {
    private Label l;
    private int dotCount = 0;
    private double curTime = 0;
    private const double MAX_TIME = 0.5;

    public override void _Ready() {
        l = this.GetNode<Label>("./Label");
    }

    public override void _Process(double delta) {
        curTime += delta;
        if (curTime > MAX_TIME) {
            curTime -= MAX_TIME;
            dotCount = (dotCount + 1) % 4;

            string s = "Loading";
            for (int i = 0; i < dotCount; i++) {
                s += ".";
            }
            l.Text = s;
        }
    }
}

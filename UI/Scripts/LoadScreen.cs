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
    private int loadState = 0;
    // 0: Uninitialized
    // 1: Initialized (Loading...)
    // 2: Loaded (Fading...)
    // 3: Fading Done
    private LoadData[] datas;
    private float fadeProg = 0;
    private float fadeTime = 0.5f;

    public override void _Ready()
    {
        l = this.GetNode<Label>("./Label");
    }

    public void Init(LoadData[] ds, Mode m) // Mode is currently unused
    {
        if (loadState > 0) return;

        this.datas = ds;
        for (int i = 0; i < datas.Length; i++)
        {
            ResourceLoader.LoadThreadedRequest(datas[i].path);
        }

        loadState = 1;
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

        if (loadState == 0) return; // Uninit: do nothing
        else if (loadState == 1)
        {
            // Check load status
            bool okay = true;
            for (int i = 0; i < datas.Length; i++)
            {
                ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(datas[i].path);
                if (status != (ResourceLoader.ThreadLoadStatus)3)
                {
                    okay = false;
                    break;
                }
            }

            if (okay)
            {
                // Load up each scene
                for (int i = 0; i < datas.Length; i++)
                {
                    PackedScene loadScene = (PackedScene)ResourceLoader.LoadThreadedGet(datas[i].path);
                    Node lNode = loadScene.Instantiate();

                    // Attach transition to the desired location
                    if (datas[i].type == Type.GAME)
                    {
                        Node GAME_ROOT = this.GetTree().GetNodesInGroup("GAME_ROOT")[0];
                        GAME_ROOT.AddChild(lNode);
                    }
                    else if (datas[i].type == Type.UI)
                    {
                        Node UI_ROOT = this.GetTree().GetNodesInGroup("UI_ROOT")[0];
                        UI_ROOT.AddChild(lNode);
                    }
                    else
                    {
                        GD.PrintErr("LoadScreen has failed!");
                        // "crash" the game
                        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest); // send notif that we are about to quit
                        GetTree().Quit();
                    }

                    ((CanvasItem)lNode).Visible = datas[i].show;
                }

                // Prepare for fade transition
                // - can't use actual Transition scene between Game&UI, as transition puts both items on the same parent node.
                // - It also doesn't support fade transitions, which would be significantly different from how the slide system
                // - works.
                Label l = this.GetNode<Label>("./Label");
                l.Visible = false;

                loadState = 2;
            } // else, continue waiting
        }
        else if (loadState == 2)
        {
            // Fade more
            fadeProg += ((float)delta) / fadeTime;
            if (fadeProg >= 1)
            {
                fadeProg = 1;
                loadState = 3;
            }
            this.Modulate = new Color(1, 1, 1, 1 - fadeProg); // linear fade
        }
        else if (loadState == 3)
        {
            // Done!
            this.QueueFree();
        }
    }

    public struct LoadData
    {
        public string path;
        public Type type;
        public bool show;

        public LoadData(string p, Type t)
        {
            path = p;
            type = t;
            show = true;
        }
        public LoadData(string p, Type t, bool s)
        {
            path = p;
            type = t;
            show = s;
        }
    }

    public enum Mode
    {
        LOAD,
        SAVE_LOAD,
        LOAD_SAVE,
        INVALID = -1
    }

    public enum Type
    {
        GAME,
        UI,
        INVALID = -1
    }
}

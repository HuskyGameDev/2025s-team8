using Godot;
using System;

// This exists as an Autoload (Singleton), and will run any needed setups
//   when the game launches
public partial class Init : Node
{
    const string GAME_ROOT_SCENE_PATH = "res://Scenes/GameRoot.tscn";

    public override void _Ready()
    {
        ResourceLoader.LoadThreadedRequest(GAME_ROOT_SCENE_PATH);

        Node root = this.GetTree().GetRoot();
        
        PackedScene gameRootScene = (PackedScene) ResourceLoader.LoadThreadedGet(GAME_ROOT_SCENE_PATH);
        Node grNode = gameRootScene.Instantiate();
        root.CallDeferred("add_child", grNode);

        // Init is done, remove ourselves (permanent)
        this.QueueFree();
    }
}

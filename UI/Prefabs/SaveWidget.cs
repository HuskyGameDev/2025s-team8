using Godot;
using System;

public partial class SaveWidget : Button
{
	[Export]
	private Label playerName;
	public override void _Ready()
	{
		this.Pressed += () =>
		{
			GD.Print("test");
		};
	}

	// Setters
	public void SetPlayerName(string s)
	{
		if (playerName != null)
		{
			playerName.Text = s;
		}
	}

	public void SetSaveName(string s)
	{
		this.SetMeta("saveName", s);
	}

    // public void SetButtonGroup(ButtonGroup b)
    // {
	// 	this.ButtonGroup = b;
    // }

	// Getters (only saveName is needed)
    public string GetSaveName()
    {
        return (string)this.GetMeta("saveName");
    }
}

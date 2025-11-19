using Godot;
using System;

// Class for handling Button / NineTextureRect combinations
// Specifically handles textures for different Button states
// CANNOT HANDLE Disabled OR Hovered + Focused BUTTON STATE!
public partial class NPRButton : Button {
    // Assumed to be Button -> NinePatchRect
    // If a section is left blank, it will default to normalTex
    // [Export]
    private Texture2D normalTex = null;
    // [Export]
    private Texture2D pressedTex = null;
    // [Export]
    private Texture2D focusedTex = null;
    // [Export]
    private Texture2D hoveredTex = null;
    // [Export]
    private Texture2D disabledTex = null;

    private bool isPressed = false;
    private bool isFocused = false;
    private bool isHovered = false;

    private NinePatchRect npr;

    public override void _Ready() {
        // Init
        normalTex = (Texture2D)GD.Load("res://UI/Assets/Button_Normal.png");
        pressedTex = (Texture2D)GD.Load("res://UI/Assets/Button_Pressed.png");
        // focusedTex = (Texture2D)GD.Load("res://UI/Assets/Button_Normal.png");
        // hoveredTex = (Texture2D)GD.Load("res://UI/Assets/Button_Normal.png");
        disabledTex = (Texture2D)GD.Load("res://UI/Assets/Button_Disabled.png");

        npr = (NinePatchRect)this.GetChild(0);

        // // Update Size/Scale
        // double xscale = this.Size.X / 170.0;
        // double yscale = this.Size.Y / 170.0;
        // npr.Scale = new Vector2((float)xscale, (float)yscale);
        // npr.Size = new Vector2(170, 170); // anchors instead?
        // This is not the fix I wanted... But it will do for now
        npr.Scale *= 4;
        // npr.Size /= 4;
        npr.SetDeferred("size", npr.Size / 4);
        

        // Attach to Signals
        this.ButtonDown += () => {
            isPressed = true;
            UpdateState();
        };
        this.ButtonUp += () => {
            isPressed = false;
            UpdateState();
        };
        this.FocusEntered += () => {
            isFocused = true;
            UpdateState();
        };
        this.FocusExited += () => {
            isFocused = false;
            UpdateState();
        };
        this.MouseEntered += () => {
            isHovered = true;
            UpdateState();
        };
        this.MouseExited += () => {
            isHovered = false;
            UpdateState();
        };

        UpdateState();
    }

    private void UpdateState() {
        if (this.Disabled == true) {
            if (disabledTex != null) {
                npr.Texture = disabledTex;
                return;
            }
        } else if (isPressed) {
            if (pressedTex != null) {
                npr.Texture = pressedTex;
                return;
            }
        } else if (isFocused) {
            if (focusedTex != null) {
                npr.Texture = focusedTex;
                return;
            }
        } else if (isHovered) {
            if (hoveredTex != null) {
                npr.Texture = hoveredTex;
                return;
            }
        }
        npr.Texture = normalTex;
    }
}

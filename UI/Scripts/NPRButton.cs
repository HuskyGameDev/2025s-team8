using Godot;
using System;

// Class for handling Button / NineTextureRect combinations
// Specifically handles textures for different Button states
// CANNOT HANDLE Disabled OR Hovered + Focused BUTTON STATE!
public partial class NPRButton : Button {
    // Assumed to be Button -> NinePatchRect
    // If a section is left blank, it will default to normalTex
    [Export]
    private Texture2D normalTex = null;
    [Export]
    private Texture2D pressedTex = null;
    [Export]
    private Texture2D focusedTex = null;
    [Export]
    private Texture2D hoveredTex = null;

    private bool isPressed = false;
    private bool isFocused = false;
    private bool isHovered = false;

    private NinePatchRect npr;

    public override void _Ready() {
        // Init
        if (normalTex == null) {
            GD.PushWarning("NPRButton does not have a default texture!");
            return;
        }
        npr = (NinePatchRect)this.GetChild(0);

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

        // disable, focus, hover, normal, pressed
    }

    private void UpdateState() {
        if (isPressed) {
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

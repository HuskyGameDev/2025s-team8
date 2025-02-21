using Godot;
using System;

public partial class Transition : Control {
	// Gobjects
	private TextureRect toRect = null;
	private Control toOffset = null;
	private TextureRect fromRect = null;
	private Control fromOffset = null;

	// Transition Controls
	private const double SCROLL_TIME = 1; // In seconds
	private int toScrollOffset = -4080; // for toScroll only; fromScroll starts at x0
    private int fromScrollOffset = -1080;
	private bool isScrolling = false;

	private int scrollDist = 3000; // 1920 (width) + 1080 (height for 45deg diag)
	private double curScrollDist = 0;

	public override void _Ready() {
		// Get Gobject refs
		toRect = this.GetNode<TextureRect>("./To");
		toOffset = toRect.GetNode<Control>("./ToOffset");
		fromRect = this.GetNode<TextureRect>("./From");
		fromOffset = fromRect.GetNode<Control>("./FromOffset");
	}

	public override void _Process(double delta) {
		if (!isScrolling) return;
		// Update Scroll Amount
		curScrollDist += (delta / SCROLL_TIME) * scrollDist;
		if (Math.Abs(curScrollDist) >= Math.Abs(scrollDist)) { // If Done
			isScrolling = false;
			// Move the child of toOffset (the new thing) to the parent of this node...
            toOffset.GetChild(0).Reparent(this.GetParent());

            // ...so that it survives the deletion of this Transition instance
            this.QueueFree();
		}
		UpdatePositions();
	}

	public void BeginTransition(Node to, Node from, Mode mode) {
		// If Br or tL, need to flip tex so that it begins from the correct corner
		if (mode == Mode.topLeft || mode == Mode.bottomRight) {
			toRect.FlipH = true;
			fromRect.FlipH = true;
		}

		// If scrolling right to left, need to invert scroll direction and correct start positions
		if (mode == Mode.topRight || mode == Mode.bottomRight) {
			scrollDist *= -1;
			toScrollOffset = 1920;
		}

		// Set initial and begin
		if (to.GetParent() != null) to.Reparent(toOffset);
		else toOffset.AddChild(to);
		if (from.GetParent() != null) from.Reparent(fromOffset);
		else fromOffset.AddChild(from);
		UpdatePositions();
		isScrolling = true;
	}

	private void UpdatePositions() {
		toRect.Position = new Vector2((float)(curScrollDist + toScrollOffset), 0);
		toOffset.Position = toRect.Position * -1; // Keep this node at a relative 0,0
		fromRect.Position = new Vector2((float)(curScrollDist + fromScrollOffset), 0);
		fromOffset.Position = fromRect.Position * -1;
	}

	public enum Mode {
		topLeft = 0,
		bottomLeft = 1,
		topRight = 2,
		bottomRight = 3
	}
}

using Godot;
using System;

public partial class InventoryItem : TextureRect
{
    public Item data;

    public override void _Ready() {
        if (data != null)
        {
            this.ExpandMode = (ExpandModeEnum)1; //EXPAND_IGNORE_SIZE
            this.StretchMode = (StretchModeEnum)5; //STRETCH_KEEP_ASPECT_CENTERED
            this.Texture = data.GetTex();
            this.TooltipText = data.GetTooltip();
        }
    }

    public void Init(Item d) {
        this.data = d;
    }

    public override Variant _GetDragData(Vector2 atPosition) {
        this.SetDragPreview(MakeDragPreview(atPosition));
        return this;
    }

    // Allows for dragging of items
    public Control MakeDragPreview(Vector2 atPosition) {
        TextureRect t = new TextureRect();
        t.ExpandMode = (ExpandModeEnum)1; //EXPAND_IGNORE_SIZE
        t.StretchMode = (StretchModeEnum)5; //STRETCH_KEEP_ASPECT_CENTERED
        t.Texture = data.GetTex();
        t.CustomMinimumSize = this.Size;
        t.Modulate = new Color(1f, 1f, 1f, 0.5f);
        t.Position = atPosition * -1;

        Control c = new Control();
        c.AddChild(t);
        return c;
    }
}

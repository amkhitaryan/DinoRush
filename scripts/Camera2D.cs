using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	private float zoomspeed = 10f;
	private float zoomMargin = 0.3f;
	
	private float zoomMin = 0.2f;
	private float zoomMax = 3f;
	
	private Vector2 zoomPos = new Vector2();
	private float zoomFactor = 1.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var vector2 = Zoom;
		vector2 = Zoom.Lerp(Zoom, (float)(zoomspeed * delta));
		// vector2.X = Zoom.Lerp(new Vector2(Zoom.X, Zoom.X * zoomFactor), (float)(zoomspeed * delta)).X;
		// vector2.Y = Zoom.Lerp(new Vector2(Zoom.Y, Zoom.Y * zoomFactor), (float)(zoomspeed * delta)).Y;
		Zoom = vector2;

		if (Zoom.X > zoomMax)
		{
			Zoom = Zoom with { X = zoomMax };
		}
		if (Zoom.Y > zoomMax)
		{
			Zoom = Zoom with { Y = zoomMax };
		}
		if (Zoom.X < zoomMin)
		{
			Zoom = Zoom with { X = zoomMin };
		}
		if (Zoom.Y < zoomMin)
		{
			Zoom = Zoom with { Y = zoomMin };
		}

		if (Input.IsActionPressed("ui_text_scroll_up") || Input.IsActionPressed("ui_page_up"))
		{
			zoomFactor -= 0.01f;
			// zoomPos = new Vector2(GetGlobalTransform().X.X, GetGlobalTransform().X.Y);
		}
		
		if (Input.IsActionPressed("ui_text_scroll_down") || Input.IsActionPressed("ui_page_down"))
		{
			zoomFactor += 0.01f;
			// zoomPos = new Vector2(GetGlobalTransform().X.X, GetGlobalTransform().X.Y);
		}
		
	}
}

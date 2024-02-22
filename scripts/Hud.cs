using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export]
	public TimeBoard timeBoard;

	[Export]
	public ScoreBoard scoreBoard;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
}

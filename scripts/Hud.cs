using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export]
	public TimeBoard timeBoard;

	[Export]
	public ScoreBoard scoreBoard;
	
	[Export]
	public Container healthBoard;

	[Export]
	public TextureRect healthLost;

	public override void _Ready()
	{
		for (int i = 0; i < Globals.DefaultMaxHealth - 1; i++)
		{
			var heart = healthBoard.GetChild(0).Duplicate() as TextureRect;
			healthBoard.AddChild(heart);
		}
	}

	public override void _Process(double delta)
	{
	}
	
	public void SetHearts(int hearts, int maxHearts)
	{
		var heartCount = healthBoard.GetChildCount();
		if (maxHearts > hearts)
		{
			for (var i = heartCount - 1; i >= hearts; i--)
			{
				((TextureRect)healthBoard.GetChild(i)).Texture = (AtlasTexture)healthLost.Texture.Duplicate();
			}
		}
	}
}

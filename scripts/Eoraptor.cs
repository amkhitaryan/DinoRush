using Godot;
using System;

public partial class Eoraptor : CharacterBody2D
{
	public const float _speed = 60.0f;
	public const float _health = 100.0f;

	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
	public override void _Ready()
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}

		Animation.Play("side_walk");

		Position = new Vector2(530, 140);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}

		var position = Position;
		if (position.X <= -43)
		{
			position = new Vector2(530, 140);
			// Free();
			// return;
		}
		
		var newPosition = new Vector2((float)(position.X - (delta * _speed)), position.Y);

		Position = newPosition;
	}
}

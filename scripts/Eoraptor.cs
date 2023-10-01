using Godot;
using System;
using System.Diagnostics;

public partial class Eoraptor : CharacterBody2D
{
	private const float Speed = 100.0f;
	private const float Health = 100.0f;
	private const float Damage = 10.0f;
	private const int ScorePoints = 10;

	private bool _freedMapIndex = false;
	public int IndexOnMap = 0;
	public bool IsHorizontal = true;
	
	[Signal]
	public delegate void HitPlayerEventHandler(float damage, int posX, int posY);

	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	// private CharacterBody2D Player => GetNode<CharacterBody2D>("/root/Main/Player");

	
	public override void _Ready()
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			return;
		}

		Animation.Play("walk");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			Animation.Stop();
			return;
		}

		var position = Position;
		if (IsHorizontal && position.X <= 300 && !_freedMapIndex)
		{
			Globals.DinoSpawnMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}
		else if (!IsHorizontal && position.Y >= 300 && !_freedMapIndex)
		{
			Globals.DinoSpawnVerticalMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}
		
		if ((position.X <= -43 && IsHorizontal) || (position.Y >= 475 && !IsHorizontal))
		{
			Globals.Score += (int)Math.Round(ScorePoints * Globals.Difficulty);
			Free();
			return;
		}

		var newPosition = IsHorizontal
			? new Vector2((float)(position.X - delta * Speed * Globals.Difficulty), position.Y)
			: new Vector2(position.X, (float)(position.Y + delta * Speed * Globals.Difficulty));

		Position = newPosition;
	}
	
	private void OnEoraptorHitboxBodyEntered(Node2D body)
	{
		if (body.HasMethod("player"))
		{
			EmitSignal(SignalName.HitPlayer, Damage * Globals.Difficulty, Position.X, Position.Y);
		}
	}
	
	private void OnEoraptorHitboxBodyExited(Node2D body)
	{
		if (body.HasMethod("player"))
		{
			
		}
	}
}

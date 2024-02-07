using Godot;
using System;
using DinoRush.scripts.Enums;

public partial class Eoraptor : CharacterBody2D
{
	private const float Speed = 100.0f;
	private const float Health = 100.0f;
	private const float Damage = 10.0f;
	private const int ScorePoints = 10;

	private bool _freedMapIndex = false;
	public int IndexOnMap = 0;
	public bool IsHorizontal = true;
	public DinoRunDirection RunDirection;
	
	[Signal]
	public delegate void HitPlayerEventHandler(float damage, int posX, int posY);

	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
	public override void _Ready()
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			return;
		}

		Animation.Play();
		Animation.SpeedScale += Globals.Difficulty / 5;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			Animation.Stop();
			return;
		}

		var position = Position;
		if (RunDirection == DinoRunDirection.Left && position.X <= 300 && !_freedMapIndex)
		{
			Globals.DinoSpawnMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}
		else if (RunDirection == DinoRunDirection.Down && position.Y >= 300 && !_freedMapIndex)
		{
			Globals.DinoSpawnVerticalMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}
		
		if ((position.X <= -43 && RunDirection == DinoRunDirection.Left) || (position.Y >= 475 && RunDirection == DinoRunDirection.Down))
		{
			Globals.Score += (int)Math.Round(ScorePoints * Globals.Difficulty);
			Free();
			return;
		}

		var newPosition = RunDirection switch
		{
			DinoRunDirection.Down => new Vector2(position.X, (float)(position.Y + delta * Speed * Globals.Difficulty)),
			DinoRunDirection.Up => Position,
			DinoRunDirection.Left => new Vector2((float)(position.X - delta * Speed * Globals.Difficulty), position.Y),
			DinoRunDirection.Right => Position,
			_ => Position,
		};

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

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

	[Export]
	private AnimatedSprite2D Animation;
	
	public override void _Ready()
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			return;
		}

		switch (RunDirection)
		{
			case DinoRunDirection.Up:
				Scale = new Vector2(-1, -1);
				break;
			case DinoRunDirection.Right:
				Scale = new Vector2(-1, 1);
				break;
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
		if (!_freedMapIndex && ((RunDirection == DinoRunDirection.Left && position.X <= 300) ||
		                        (RunDirection == DinoRunDirection.Right && position.X >= 600))) 
		{
			Globals.DinoSpawnHMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}
		else if (!_freedMapIndex && ((RunDirection == DinoRunDirection.Down && position.Y >= 300) ||
		                             (RunDirection == DinoRunDirection.Up && position.Y <= 100))) 
		{
			Globals.DinoSpawnVMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}

		if ((position.Y <= -50 && RunDirection == DinoRunDirection.Up) ||
		    (position.Y >= 475 && RunDirection == DinoRunDirection.Down) ||
		    (position.X <= -43 && RunDirection == DinoRunDirection.Left) ||
		    (position.X >= 850 && RunDirection == DinoRunDirection.Right))
		{
			Globals.Score += (int)Math.Round(ScorePoints * Globals.Difficulty);
			Free();
			return;
		}

		var newPosition = RunDirection switch
		{
			DinoRunDirection.Up => new Vector2(position.X, (float)(position.Y - delta * Speed * Globals.Difficulty)),
			DinoRunDirection.Down => new Vector2(position.X, (float)(position.Y + delta * Speed * Globals.Difficulty)),
			DinoRunDirection.Left => new Vector2((float)(position.X - delta * Speed * Globals.Difficulty), position.Y),
			DinoRunDirection.Right => new Vector2((float)(position.X + delta * Speed * Globals.Difficulty), position.Y),
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

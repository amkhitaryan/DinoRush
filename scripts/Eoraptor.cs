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
		if (!_freedMapIndex && ((RunDirection == DinoRunDirection.Left && position.X <= -10.0f) ||
		                        (RunDirection == DinoRunDirection.Right && position.X >= Globals.ViewportSizeX + 10.0f))) 
		{
			Globals.DinoSpawnHMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}
		else if (!_freedMapIndex && ((RunDirection == DinoRunDirection.Down && position.Y >= Globals.ViewportSizeY + 10.0f) ||
		                             (RunDirection == DinoRunDirection.Up && position.Y <= -10))) 
		{
			Globals.DinoSpawnVMap[IndexOnMap] = false;
			_freedMapIndex = true;
		}

		if ((position.Y <= -50 && RunDirection == DinoRunDirection.Up) ||
		    (position.Y >= Globals.ViewportSizeY + 50.0f && RunDirection == DinoRunDirection.Down) ||
		    (position.X <= -50 && RunDirection == DinoRunDirection.Left) ||
		    (position.X >= Globals.ViewportSizeX + 50.0f && RunDirection == DinoRunDirection.Right))
		{
			Globals.Score += (int)Math.Round(ScorePoints * Globals.Difficulty);
			Free();
			return;
		}

		var direction = new Vector2();
		switch (RunDirection)
		{
			case DinoRunDirection.Up:
				direction.Y -= 1;
				break;
			case DinoRunDirection.Down:
				direction.Y += 1f;
				break;
			case DinoRunDirection.Left:
				direction.X -= 1f;
				break;
			case DinoRunDirection.Right:
				direction.X += 1f;
				break;
		}

		direction = direction.Normalized();
		
		MoveAndCollide(direction * Speed * Globals.Difficulty * (float)delta);
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

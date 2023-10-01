using Godot;
using System;
using System.Diagnostics;

public partial class Eoraptor : CharacterBody2D
{
	private const float _speed = 60.0f;
	private const float _health = 100.0f;
	private const float _damage = 10.0f;

	public int IndexOnMap = 0;
	
	[Signal]
	public delegate void HitPlayerEventHandler(float damage, int posX, int posY);

	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	// private CharacterBody2D Player => GetNode<CharacterBody2D>("/root/Main/Player");

	
	public override void _Ready()
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}

		Animation.Play("side_walk");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}

		var position = Position;
		if (position.X <= 300)
		{
			Globals.DinoSpawnMap[IndexOnMap] = false;
		}
		
		if (position.X <= -43)
		{
			Free();
			return;
		}
		
		var newPosition = new Vector2((float)(position.X - delta * _speed), position.Y);

		Position = newPosition;
	}
	
	private void OnEoraptorHitboxBodyEntered(Node2D body)
	{
		if (body.HasMethod("player"))
		{
			Debug.WriteLine("Sending HitPlayer signal from Eoraptor");
			EmitSignal(SignalName.HitPlayer, _damage, Position.X, Position.Y);
		}
	}
	
	private void OnEoraptorHitboxBodyExited(Node2D body)
	{
		if (body.HasMethod("player"))
		{
			
		}
	}
}

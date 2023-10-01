using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float _health = 100.0f;
	private string _currentDirection = "none";
	
	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	private Camera2D MainCamera => GetNode<Camera2D>("MainCamera");
	private ProgressBar HealthBar => GetNode<ProgressBar>("HealthBar");


	public override void _Ready()
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}

		Animation.Play("front_idle");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}
		
		PlayerMovement();
		
	}
	
	private void PlayerMovement()
	{
		if (Input.IsActionPressed("ui_right"))
		{
			_currentDirection = "right";
			PlayAnimation(1);
			Velocity = Velocity with { X = Speed, Y = 0 };
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			_currentDirection = "left";
			PlayAnimation(1);
			Velocity = Velocity with { X = -Speed, Y = 0 };
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			_currentDirection = "down";
			PlayAnimation(1);
			Velocity = Velocity with { X = 0, Y = Speed };
		}
		else if (Input.IsActionPressed("ui_up"))
		{
			_currentDirection = "up";
			PlayAnimation(1);
			Velocity = Velocity with { X = 0, Y = -Speed };
		}
		else
		{
			// _currentDirection = "none";
			PlayAnimation(0);
			Velocity = Velocity with { X = 0, Y = 0 };
		}
		
		MoveAndSlide();
	}
	
	private void PlayAnimation(int movement)
	{
		switch (_currentDirection)
		{
			case "right":
				Animation.FlipH = false;
				if (movement == 1)
				{
					Animation.Play("side_walk");
				}
				else if (movement == 0)
				{
					Animation.Play("side_idle");
				}

				break;
			case "left":
				Animation.FlipH = true;
				if (movement == 1)
				{
					Animation.Play("side_walk");
				}
				else if (movement == 0)
				{
					Animation.Play("side_idle");
				}

				break;
			case "down":
				Animation.FlipH = true;
				if (movement == 1)
				{
					Animation.Play("front_walk");
				}
				else if (movement == 0)
				{
					Animation.Play("front_idle");
				}

				break;
			case "up":
				Animation.FlipH = true;
				if (movement == 1)
				{
					Animation.Play("back_walk");
				}
				else if (movement == 0)
				{
					Animation.Play("back_idle");
				}

				break;
			default:
				Animation.Play("front_idle");
				break;
		}
	}
	
	private void UpdateHealth()
	{
		HealthBar.Value = _health;
		// HealthBar.Visible = _health < 100;
	}
}

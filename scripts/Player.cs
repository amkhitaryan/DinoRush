using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	private const float _gotHitForce = 1.15f;
	public const float Speed = 100.0f;
	public float _health = 100.0f;
	private string _currentDirection = "none";
	private bool _gotHit = false;
	private bool _canMove = true;
	private Vector2 _gotHitVector;
	
	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	private Camera2D MainCamera => GetNode<Camera2D>("MainCamera");
	private ProgressBar HealthBar => GetNode<ProgressBar>("HealthBar");
	private Timer GotHitTimer => GetNode<Timer>("GotHitTimer");

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
		UpdateHealth();
		GotHit();
		
		PlayerMovement();

	}

	private void GotHit()
	{
		if (_gotHit)
		{
			_canMove = false;
			Velocity = Velocity with { X = _gotHitVector.X, Y = _gotHitVector.Y };
			MoveAndSlide();
		}
	}

	private void OnGotHitTimerTimeout()
	{
		_canMove = true;
		_gotHit = false;
	}

	private void PlayerMovement()
	{
		if (!_canMove)
		{
			return;
		}
		
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

	private void OnPlayerHitboxBodyEntered(Node2D body)
	{
		if (body.HasMethod("Enemy"))
		{
			
		}
	}
	
	private void OnPlayerHitboxBodyExited(Node2D body)
	{
		
	}

	private void player()
	{
		
	}

	public void OnEoraptorHitPlayer(float damage,  float posX, float posY)
	{
		Debug.WriteLine($"Player got hit for {damage}hp");

		_gotHitVector = new Vector2(Position.X < posX  ? -100 : 100, Position.Y < posY-20 ? -Speed * _gotHitForce : Speed * _gotHitForce);
		Debug.WriteLine(_gotHitVector);
		_gotHit = true;
		_health -= damage;
		GotHitTimer.Start();
		
		// play sound
	}
	
}

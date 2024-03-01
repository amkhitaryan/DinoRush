using Godot;

public partial class Player : CharacterBody2D
{
	private const float GotHitForce = 1.15f;
	public float Speed = 75.0f;
	private Vector2 _currentDirection = Vector2.Zero;
	private bool _gotHit = false;
	private bool _canMove = true;
	private Vector2 _gotHitVector;
	
	[Export]
	private AnimatedSprite2D Animation;
	[Export]
	private Timer GotHitTimer;
	[Export]
	private AudioStreamPlayer2D GotHitAudio;

	public override void _Ready()
	{
		if (!Globals.IsGameStarted)
		{
			return;
		}

		Animation.Play("Idle");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			return;
		}
		
		GotHit();
		PlayerMovement(delta);

	}

	private void GotHit()
	{
		if (_gotHit)
		{
			_canMove = false;
			Velocity = Velocity with { X = _gotHitVector.X, Y = _gotHitVector.Y };
			PlayAnimation(false);
			MoveAndSlide();
		}
	}

	private void OnGotHitTimerTimeout()
	{
		_canMove = true;
		_gotHit = false;
	}

	private void PlayerMovement(double delta)
	{
		if (!_canMove)
		{
			return;
		}
		
		var direction = new Vector2();
		if (Input.IsActionPressed("ui_right"))
			direction.X += 1;
		if (Input.IsActionPressed("ui_left"))
			direction.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			direction.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			direction.Y -= 1;
		
		if (direction == Vector2.Zero)
		{
			PlayAnimation(false);
		}
		else
		{
			_currentDirection = direction;
			direction = direction.Normalized();
			direction = direction * Speed * (float)delta;
			PlayAnimation(true);

			MoveAndCollide(direction * Speed * (float)delta);
		}
	}
	
	private void PlayAnimation(bool isMoving)
	{
		if (_currentDirection == Vector2.Zero)
		{
			Animation.Play(_gotHit ? "FrontHit" : "Idle");
			return;
		}

		if (Mathf.Abs(_currentDirection.X) > Mathf.Abs(_currentDirection.Y))
		{
			Animation.FlipH = _currentDirection.X < 0;
			Animation.Play(isMoving ? "SideWalk" : _gotHit ? "SideHit" : "SideIdle");
		}
		else
		{
			Animation.FlipH = false;
			if (_currentDirection.Y > 0)
			{
				Animation.Play(isMoving ? "FrontWalk" : _gotHit ? "FrontHit" : "Idle");
			}
			else
			{
				Animation.Play(isMoving ? "BackWalk" : _gotHit ? "BackHit" :"BackIdle");
			}
		}
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
		_gotHitVector = new Vector2(Position.X < posX ? -100 - Globals.Difficulty * 3 : 100 + Globals.Difficulty * 3,
			Position.Y < posY - 10
				? -Speed * GotHitForce - Globals.Difficulty * 3
				: Speed * GotHitForce + Globals.Difficulty * 3);

		_gotHit = true;
		GotHitTimer.Start();
		GotHitAudio.Play();
		
		Globals.CurrentHealth -= 1;

		if (Globals.CurrentHealth <= 0)
		{
			Globals.CurrentHealth = 0;
			Globals.IsGameOver = true;
			Animation.Stop();
		}
	}

	private void OnDifficultyUp()
	{
		Animation.SpeedScale += Globals.Difficulty / 5;
	}
	
}

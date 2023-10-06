using Godot;

public partial class Player : CharacterBody2D
{
	private const float GotHitForce = 1.15f;
	private float _speed = 75.0f;
	private float _health = 110.0f;
	private Vector2 _currentDirection = Vector2.Zero;
	private bool _gotHit = false;
	private bool _canMove = true;
	private Vector2 _gotHitVector;
	
	private AnimatedSprite2D Animation => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	private Camera2D MainCamera => GetNode<Camera2D>("MainCamera");
	private ProgressBar HealthBar => GetNode<ProgressBar>("HealthBar");
	private Timer GotHitTimer => GetNode<Timer>("GotHitTimer");
	private AudioStreamPlayer2D GotHitAudio => GetNode<AudioStreamPlayer2D>("GotHitAudio");

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
		if (!Globals.IsGameStarted || Globals.IsGameOver)
		{
			return;
		}
		
		UpdateHealth();
		GotHit();
		PlayerMovement(delta);

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
			direction = direction * _speed * (float)delta;
			PlayAnimation(true);

			MoveAndCollide(direction * _speed * (float)delta);
		}
	}
	
	private void PlayAnimation(bool isMoving)
	{
		if (_currentDirection == Vector2.Zero)
		{
			Animation.Play("front_idle");
			return;
		}

		if (Mathf.Abs(_currentDirection.X) > Mathf.Abs(_currentDirection.Y))
		{
			Animation.FlipH = (_currentDirection.X < 0);
			Animation.Play(isMoving ? "side_walk" : "side_idle");
		}
		else
		{
			Animation.FlipH = false;
			if (_currentDirection.Y > 0)
			{
				Animation.Play(isMoving ? "front_walk" : "front_idle");
			}
			else
			{
				Animation.Play(isMoving ? "back_walk" : "back_idle");
			}
		}
		
	}
	
	private void UpdateHealth()
	{
		HealthBar.Value = _health;
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
			Position.Y < posY - 20
				? -_speed * GotHitForce - Globals.Difficulty * 3
				: _speed * GotHitForce + Globals.Difficulty * 3);
		_gotHit = true;
		_health -= damage;
		GotHitTimer.Start();
		GotHitAudio.Play();

		if (_health <= 0)
		{
			_health = 0;
			Globals.IsGameOver = true;
			Animation.Stop();
		}
	}

	private void OnDifficultyUp()
	{
		Animation.SpeedScale += Globals.Difficulty / 5;
	}
	
}

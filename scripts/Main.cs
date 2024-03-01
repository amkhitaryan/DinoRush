using Godot;
using System;
using System.Linq;
using DinoRush.scripts.Enums;

public partial class Main : Node2D
{
	[Signal]
	public delegate void DifficultyUpEventHandler();

	[Export]
	private Player _player;
	[Export]
	private Timer _difficultyUpTimer;
	[Export]
	private Timer _enemySpawnTimer;
	[Export]
	private AudioStreamPlayer2D _difficultyUpAudio;
	[Export]
	private AudioStreamPlayer2D _menuAudio;
	[Export]
	private AudioStreamPlayer2D _soundtrackAudio;
	[Export]
	private PackedScene _triceratopsScene;
	[Export]
	private PackedScene _eoraptorVScene;
	[Export]
	private UI _ui;
	
	private Random _random;
	
	public override void _Ready()
	{
		_random = new Random((int)DateTime.Now.Ticks);
	}

	public override void _Process(double delta)
	{
		if (Globals.IsGameOver)
		{
			_ui.OnGameOver();
			_soundtrackAudio.Stop();
		}
	}

	private void OnEnemySpawnTimerTimeout()
	{
		SpawnDino(false);
	}

	private void OnEnemySpawnVerticalTimerTimeout()
	{
		SpawnDino(true);
	}

	private void OnDifficultyUpTimerTimeout()
	{
		if (Globals.IsGameOver || Globals.Difficulty >= 4)
		{
			return;
		}
		
		_difficultyUpAudio.Play();
		Globals.Difficulty += 0.5f;
		_player.Speed += 2.5f;
		_enemySpawnTimer.WaitTime = _enemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		_soundtrackAudio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}

	private void OnUIGameStarted()
	{
		_menuAudio.Stop();
		_soundtrackAudio.Play();
		_difficultyUpTimer.Start();
	}

	private void OnUIGameRestarted()
	{
		_difficultyUpTimer.Stop();
	}
	
	private void SpawnDino(bool isVertical)
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver ||
		    (isVertical && Globals.DinoSpawnVMap.All(x => x.Value)) ||
		    (!isVertical && Globals.DinoSpawnHMap.All(x => x.Value)))
		{
			return;
		}
		
		int rnd;
		if (isVertical)
		{
			do
			{
				rnd = _random.Next(0, Globals.DinoSpawnVMap.Count);
			} while (Globals.DinoSpawnVMap[rnd]);
			Globals.DinoSpawnVMap[rnd] = true;
		}
		else
		{
			do
			{
				rnd = _random.Next(0, Globals.DinoSpawnHMap.Count);
			} while (Globals.DinoSpawnHMap[rnd]);
			Globals.DinoSpawnHMap[rnd] = true;
		}

		var dino = isVertical ? (Node2D)_eoraptorVScene.Instantiate() : (Node2D)_triceratopsScene.Instantiate();
		var dinoClass = (Dino)dino;
		dinoClass.HitPlayer += (damage, x, y) => _player.OnEoraptorHitPlayer(damage, x, y);
		dinoClass.IndexOnMap = rnd;
		dinoClass.RunDirection = isVertical ? (DinoRunDirection)_random.Next(0, 2) : (DinoRunDirection)_random.Next(2, 4);
		dinoClass.Position = dinoClass.RunDirection switch
		{
			DinoRunDirection.Up => new Vector2(rnd == 0 ? 10 : 35.0f * Math.Max(rnd, 1), Globals.ViewportSizeY + 35.0f),
			DinoRunDirection.Down => new Vector2(rnd == 0 ? 10 : 35.0f * Math.Max(rnd, 1), -35.0f),
			DinoRunDirection.Left => new Vector2(Globals.ViewportSizeX + 35.0f, 35.0f * Math.Max(rnd, 1)),
			DinoRunDirection.Right => new Vector2(-35.0f, 35.0f * Math.Max(rnd, 1)),
			_ => Position,
		};
			 
		AddChild(dino);
	}
}

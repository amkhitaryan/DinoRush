using Godot;
using System;
using System.Linq;
using DinoRush.scripts.Enums;

public partial class Main : Node2D
{
	[Signal]
	public delegate void DifficultyUpEventHandler();

	private Player Player => GetNode<Player>("/root/Main/Player");
	private Timer DifficultyUpTimer => GetNode<Timer>("DifficultyUpTimer");
	private Timer EnemySpawnTimer => GetNode<Timer>("EnemySpawnTimer");
	private AudioStreamPlayer2D DifficultyUpAudio => GetNode<AudioStreamPlayer2D>("DifficultyUpAudio");
	private AudioStreamPlayer2D MenuAudio => GetNode<AudioStreamPlayer2D>("MenuAudio");
	private AudioStreamPlayer2D SoundtrackAudio => GetNode<AudioStreamPlayer2D>("Soundtrack");
	private PackedScene _triceratopsScene = GD.Load<PackedScene>("res://scenes/triceratops.tscn");
	private PackedScene _eoraptorVScene = GD.Load<PackedScene>("res://scenes/eoraptor_v.tscn");
	private Random _random;
	
	public override void _Ready()
	{
		_random = new Random((int)DateTime.Now.Ticks);
	}

	public override void _Process(double delta)
	{
		if (Globals.IsGameOver)
		{
			var node = GetNode("UI") as UI;
			node.OnGameOver();
			SoundtrackAudio.Stop();
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
		
		DifficultyUpAudio.Play();
		Globals.Difficulty += 0.5f;
		EnemySpawnTimer.WaitTime = EnemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		SoundtrackAudio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}

	private void OnUIGameStarted()
	{
		MenuAudio.Stop();
		SoundtrackAudio.Play();
		DifficultyUpTimer.Start();
	}

	private void OnUIGameRestarted()
	{
		DifficultyUpTimer.Stop();
	}
	
	private void SpawnDino(bool isVertical)
	{
		if (!Globals.IsGameStarted || Globals.IsGameOver ||
		    (isVertical && Globals.DinoSpawnVerticalMap.All(x => x.Value)) ||
		    (!isVertical && Globals.DinoSpawnMap.All(x => x.Value)))
		{
			return;
		}
		
		int rnd;
		if (isVertical)
		{
			do
			{
				rnd = _random.Next(0, Globals.DinoSpawnVerticalMap.Count);
			} while (Globals.DinoSpawnVerticalMap[rnd]);
			Globals.DinoSpawnVerticalMap[rnd] = true;
		}
		else
		{
			do
			{
				rnd = _random.Next(0, Globals.DinoSpawnMap.Count);
			} while (Globals.DinoSpawnMap[rnd]);
			Globals.DinoSpawnMap[rnd] = true;
		}

		var dino = isVertical ? (Node2D)_eoraptorVScene.Instantiate() : (Node2D)_triceratopsScene.Instantiate();
		var dinoClass = (Eoraptor)dino;
		dinoClass.HitPlayer += (damage, x, y) => Player.OnEoraptorHitPlayer(damage, x, y);
		dinoClass.IndexOnMap = rnd;
		dinoClass.RunDirection = isVertical ? DinoRunDirection.Down : DinoRunDirection.Left;
		dinoClass.Position = isVertical
			? new Vector2(rnd == 0 ? 10 : 35.0f * Math.Max(rnd, 1), -20.0f)
			: new Vector2(880.0f, 35.0f * Math.Max(rnd, 1));
		AddChild(dino);
	}
}

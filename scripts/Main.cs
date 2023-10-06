using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Main : Node2D
{
	[Signal]
	public delegate void DifficultyUpEventHandler();

	private Player Player => GetNode<Player>("/root/Main/Player");
	private Timer Lvl2DifficultyTimer => GetNode<Timer>("Lvl2DifficultyTimer");
	private Timer Lvl3DifficultyTimer => GetNode<Timer>("Lvl3DifficultyTimer");
	private Timer Lvl4DifficultyTimer => GetNode<Timer>("Lvl4DifficultyTimer");
	private Timer Lvl5DifficultyTimer => GetNode<Timer>("Lvl5DifficultyTimer");
	private Timer Lvl6DifficultyTimer => GetNode<Timer>("Lvl6DifficultyTimer");
	private Timer EnemySpawnTimer => GetNode<Timer>("EnemySpawnTimer");
	private AudioStreamPlayer2D Lvl2Audio => GetNode<AudioStreamPlayer2D>("LvlDifficultyChangeAudio");
	private AudioStreamPlayer2D Lvl6Audio => GetNode<AudioStreamPlayer2D>("Lvl6DifficultyChangeAudio");
	private AudioStreamPlayer2D SoundtrackAudio => GetNode<AudioStreamPlayer2D>("Soundtrack");
	private AudioStreamPlayer2D Soundtrack2Audio => GetNode<AudioStreamPlayer2D>("Soundtrack2");
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
			Soundtrack2Audio.Stop();
		}
	}

	private void OnEnemySpawnTimerTimeout()
	{
		if (!Globals.IsGameStarted || Globals.DinoSpawnMap.All(x => x.Value) || Globals.IsGameOver)
		{
			return;
		}
		
		int rnd;
		do
		{
			rnd = _random.Next(0, Globals.DinoSpawnMap.Count);
		} while (Globals.DinoSpawnMap[rnd]);
		
		Globals.DinoSpawnMap[rnd] = true;
		var dino = (Node2D)_triceratopsScene.Instantiate();

		var dinoClass = dino as Eoraptor;
		dinoClass.HitPlayer += (damage, x, y) => Player.OnEoraptorHitPlayer(damage, x, y);
		dinoClass.IndexOnMap = rnd;
		dinoClass.IsHorizontal = true;
		dinoClass.Position = new Vector2(880.0f, 35.0f * Math.Max(rnd, 1));
		AddChild(dino);
	}

	private void OnEnemySpawnVerticalTimerTimeout()
	{
		if (!Globals.IsGameStarted || Globals.DinoSpawnVerticalMap.All(x => x.Value) || Globals.IsGameOver)
		{
			return;
		}
		
		int rnd;
		do
		{
			rnd = _random.Next(0, Globals.DinoSpawnVerticalMap.Count);
		} while (Globals.DinoSpawnVerticalMap[rnd]);
		
		Globals.DinoSpawnVerticalMap[rnd] = true;
		var dino = (Node2D)_eoraptorVScene.Instantiate();

		var dinoClass = dino as Eoraptor;
		dinoClass.HitPlayer += (damage, x, y) => Player.OnEoraptorHitPlayer(damage, x, y);
		dinoClass.IndexOnMap = rnd;
		dinoClass.IsHorizontal = false;
		dinoClass.Position = new Vector2(rnd == 0 ? 10 : 35.0f * Math.Max(rnd, 1), -20.0f);
		AddChild(dino);
	}

	private void OnLvl2DifficultyTimerTimeout()
	{
		Lvl2Audio.Play();
		Globals.Difficulty = 1.5f;
		EnemySpawnTimer.WaitTime = EnemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		Soundtrack2Audio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}
	
	private void OnLvl3DifficultyTimerTimeout()
	{
		Lvl2Audio.Play();
		Globals.Difficulty = 2.0f;
		EnemySpawnTimer.WaitTime = EnemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		Soundtrack2Audio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}
	
	private void OnLvl4DifficultyTimerTimeout()
	{
		Lvl2Audio.Play();
		Globals.Difficulty = 2.5f;
		EnemySpawnTimer.WaitTime = EnemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		Soundtrack2Audio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}
	
	private void OnLvl5DifficultyTimerTimeout()
	{
		Lvl2Audio.Play();
		Globals.Difficulty = 3.0f;
		EnemySpawnTimer.WaitTime = EnemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		Soundtrack2Audio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}
	
	private void OnLvl6DifficultyTimerTimeout()
	{
		Lvl6Audio.Play();
		Globals.Difficulty = 3.5f;
		EnemySpawnTimer.WaitTime = EnemySpawnTimer.WaitTime / Globals.Difficulty * 1.2f;
		Soundtrack2Audio.PitchScale += 0.04f;
		EmitSignal(SignalName.DifficultyUp);
	}

	private void OnUIGameStarted()
	{
		SoundtrackAudio.Stop();
		Soundtrack2Audio.Play();
		Lvl2DifficultyTimer.Start();
		Lvl3DifficultyTimer.Start();
		Lvl4DifficultyTimer.Start();
		Lvl5DifficultyTimer.Start();
		Lvl6DifficultyTimer.Start();
	}

	private void OnUIGameRestarted()
	{
		Lvl2DifficultyTimer.Stop();
		Lvl3DifficultyTimer.Stop();
		Lvl4DifficultyTimer.Stop();
		Lvl5DifficultyTimer.Stop();
		Lvl6DifficultyTimer.Stop();
	}
}

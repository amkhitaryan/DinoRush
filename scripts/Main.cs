using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Main : Node2D
{
	private Player Player => GetNode<Player>("/root/Main/Player");
	// private CharacterBody2D Enemy => GetNode<CharacterBody2D>("/root/Main/Enemy");
	
	private PackedScene _eoraptorScene = GD.Load<PackedScene>("res://scenes/eoraptor.tscn");
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
		var dino = (Node2D)_eoraptorScene.Instantiate();

		var dinoClass = dino as Eoraptor;
		dinoClass.HitPlayer += (damage, x, y) => Player.OnEoraptorHitPlayer(damage, x, y);
		dinoClass.IndexOnMap = rnd;
		dinoClass.IsHorizontal = true;
		dinoClass.Position = new Vector2(785.0f, 40.0f * Math.Max(rnd, 1));
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
		dinoClass.Position = new Vector2(40.0f * Math.Max(rnd, 1), -20.0f);
		AddChild(dino);
	}
}

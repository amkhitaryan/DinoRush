using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Main : Node2D
{
	private Player Player => GetNode<Player>("/root/Main/Player");
	// private CharacterBody2D Enemy => GetNode<CharacterBody2D>("/root/Main/Enemy");
	
	private PackedScene _eoraptorScene = GD.Load<PackedScene>("res://scenes/eoraptor.tscn");
	private Random _random;
	
	public override void _Ready()
	{
		_random = new Random((int)DateTime.Now.Ticks);
	}

	public override void _Process(double delta)
	{
	}

	private void OnEnemySpawnTimerTimeout()
	{
		if (Globals.DinoSpawnMap.All(x => x.Value))
		{
			return;
		}
		
		int rnd;
		do
		{
			rnd = _random.Next(0, 10);
		} while (Globals.DinoSpawnMap[rnd]);
		
		Globals.DinoSpawnMap[rnd] = true;
		var dino = (Node2D)_eoraptorScene.Instantiate();

		var dinoClass = dino as Eoraptor;
		dinoClass.HitPlayer += (damage, x, y) => Player.OnEoraptorHitPlayer(damage, x, y); // connect signal
		dinoClass.IndexOnMap = rnd;
		dinoClass.Position = new Vector2(750, (float)(40 * Math.Pow(rnd, 1) + 40));
		AddChild(dino);
	}
}

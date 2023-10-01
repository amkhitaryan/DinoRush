using Godot;
using System;

public partial class Main : Node2D
{
	// private CharacterBody2D Player => GetNode<CharacterBody2D>("/root/Main/Player");
	// private CharacterBody2D Enemy => GetNode<CharacterBody2D>("/root/Main/Enemy");
	
	private PackedScene _eoraptorScene = GD.Load<PackedScene>("res://scenes/eoraptor.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnEnemySpawnTimerTimeout()
	{
		var dino = (Node2D)_eoraptorScene.Instantiate();
		AddChild(dino);
	}
}

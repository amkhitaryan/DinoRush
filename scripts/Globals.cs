using Godot;
using System;
using Godot.Collections;

public partial class Globals : Node
{
	public static bool IsGameStarted = true; // false
	public static bool IsGameOver = false;

	public static volatile Dictionary<int, bool> DinoSpawnMap = new()
	{
		{ 0, false },
		{ 1, false },
		{ 2, false },
		{ 3, false },
		{ 4, false },
		{ 5, false },
		{ 6, false },
		{ 7, false },
		{ 8, false },
		{ 9, false },
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

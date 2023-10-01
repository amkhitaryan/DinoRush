using Godot;
using System;
using Godot.Collections;

public partial class Globals : Node
{
	public static bool IsGameStarted = false; // false
	public static bool IsGameOver = false;
	public static int Difficulty = 1;

	public static int Score { get; set; }

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
	
	public static volatile Dictionary<int, bool> DinoSpawnVerticalMap = new()
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
		{ 10, false },
		{ 11, false },
		{ 12, false },
		{ 13, false },
		{ 14, false },
		{ 15, false },
		{ 16, false },
		{ 17, false },
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

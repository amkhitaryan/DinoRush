using Godot;
using Godot.Collections;

public partial class Globals : Node
{
	public static bool IsGameStarted = false;
	public static bool IsGameOver = false;
	public static float Difficulty = 1.0f;
	public static Vector2I ViewportSize;
	public static float ZoomFactor = 1.0f;

	public static int ViewportSizeX => (int)(ViewportSize.X / ZoomFactor);
	public static int ViewportSizeY => (int)(ViewportSize.Y / ZoomFactor);

	private static int _score = 0;
	public static int Score
	{
		get => _score;
		set
		{
			if (IsGameStarted && !IsGameOver || value == 0)
			{
				_score = value;
			}
		}
	}
	
	private static int _elapsedSeconds = 0;
	public static int ElapsedSeconds
	{
		get => _elapsedSeconds;
		set
		{
			if (IsGameStarted && !IsGameOver || value == 0)
			{
				_elapsedSeconds = value;
			}
		}
	}

	public static volatile Dictionary<int, bool> DinoSpawnHMap = new()
	{
		{ 0, false }, { 1, false }, { 2, false }, { 3, false },
		{ 4, false }, { 5, false }, { 6, false }, { 7, false }, 
		{ 8, false }, { 9, false }, { 10, false }, { 11, false },
		{ 12, false }, { 13, false }, { 14, false }, { 15, false },
		{ 16, false },
	};

	public static volatile Dictionary<int, bool> DinoSpawnVMap = new()
	{
		{ 0, false }, { 1, false }, { 2, false }, { 3, false },
		{ 4, false }, { 5, false }, { 6, false }, { 7, false },
		{ 8, false }, { 9, false }, { 10, false }, { 11, false },
		{ 12, false }, { 13, false }, { 14, false }, { 15, false },
		{ 16, false }, { 17, false }, { 18, false }, { 19, false },
		{ 20, false }, { 21, false }, { 22, false }, { 23, false },
		{ 24, false }, { 25, false }, { 26, false }, { 27, false },
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ViewportSize = GetTree().Root.ContentScaleSize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

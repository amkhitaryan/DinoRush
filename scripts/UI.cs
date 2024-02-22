using Godot;
using System.Linq;

public partial class UI : CanvasLayer
{
	[Signal]
	public delegate void GameStartedEventHandler();
	
	[Signal]
	public delegate void GameRestartedEventHandler();
	
	[Export]
	private Node2D _beforeGameScreen;
	[Export]
	private Node2D _duringGameScreen;
	[Export]
	private Node2D _endOfGameScreen;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
	
	public void OnGameOver()
	{
		if (Globals.IsGameOver)
		{
			_endOfGameScreen.Visible = true;
		}
	}

	private void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
		EmitSignal(SignalName.GameRestarted);
		Globals.IsGameStarted = false;
		Globals.IsGameOver = false;
		Globals.Score = 0;
		Globals.ElapsedSeconds = 0;
		Globals.Difficulty = 1;
		foreach (var key in Globals.DinoSpawnVMap.Keys.ToList())
		{
			Globals.DinoSpawnVMap[key] = false;
		}
		foreach (var key in Globals.DinoSpawnHMap.Keys.ToList())
		{
			Globals.DinoSpawnHMap[key] = false;
		}
	}
	
	private void OnPlayButtonPressed()
	{
		EmitSignal(SignalName.GameStarted);
		Globals.IsGameStarted = true;
		_beforeGameScreen.Visible = false;
		_duringGameScreen.Visible = true;
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}

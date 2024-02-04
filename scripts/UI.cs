using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class UI : CanvasLayer
{
	[Signal]
	public delegate void GameStartedEventHandler();
	
	[Signal]
	public delegate void GameRestartedEventHandler();
	
	private Node2D BeforeGameScreen => GetNode<Node2D>("Control/BeforeGameScreen");
	private Node2D DuringGameScreen => GetNode<Node2D>("Control/DuringGameScreen");
	private Node2D EndOfGameScreen => GetNode<Node2D>("Control/EndOfGameScreen");
	private Label ScoreLabel => GetNode<Label>("Control/DuringGameScreen/ScoreLabel");
	
	public override void _Ready()
	{
		ScoreLabel.Text = "0";
	}

	public override void _Process(double delta)
	{
		UpdatePoints();
	}
	
	private void UpdatePoints()
	{
		ScoreLabel.Text = Globals.Score.ToString();  
	}
	
	public void OnGameOver()
	{
		if (Globals.IsGameOver)
		{
			EndOfGameScreen.Visible = true;
		}
	}

	private void OnRestartButtonPressed()
	{
		GetTree().ReloadCurrentScene();
		EmitSignal(SignalName.GameRestarted);
		Globals.IsGameStarted = false;
		Globals.IsGameOver = false;
		Globals.Score = 0;
		Globals.Difficulty = 1;
		foreach (var key in Globals.DinoSpawnVerticalMap.Keys.ToList())
		{
			Globals.DinoSpawnVerticalMap[key] = false;
		}
		foreach (var key in Globals.DinoSpawnMap.Keys.ToList())
		{
			Globals.DinoSpawnMap[key] = false;
		}
	}
	
	private void OnPlayButtonPressed()
	{
		EmitSignal(SignalName.GameStarted);
		Globals.IsGameStarted = true;
		BeforeGameScreen.Visible = false;
		DuringGameScreen.Visible = true;
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}

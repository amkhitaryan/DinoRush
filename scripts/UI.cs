using Godot;
using System;
using System.Diagnostics;

public partial class UI : CanvasLayer
{
	// [Signal]
	// public delegate void GameStartedEventHandler();
	
	private Node2D BeforeGameScreen => GetNode<Node2D>("BeforeGameScreen");
	private Node2D DuringGameScreen => GetNode<Node2D>("DuringGameScreen");
	private Node2D EndOfGameScreen => GetNode<Node2D>("EndOfGameScreen");

	// private Label PointsLabel => GetNode<Label>("EndOfGameScreen/EndGameScoreDisplay/Sprite2D/PointsLabel");
	private Label ScoreLabel => GetNode<Label>("DuringGameScreen/ScoreLabel");
	
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
		// Score = points;
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
		Globals.IsGameStarted = false;
		Globals.IsGameOver = false;
		Globals.Score = 0;
		// reset positions, enemies, hps, score
	}
	
	private void OnPlayButtonPressed()
	{
		// EmitSignal(SignalName.GameStarted);
		Globals.IsGameStarted = true;
		BeforeGameScreen.Visible = false;
		DuringGameScreen.Visible = true;
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}

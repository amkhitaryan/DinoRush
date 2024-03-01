using Godot;

public partial class GameScene : Node2D
{
	[Export]
	public Hud Hud;
	
	public override void _Ready()
	{
		CreateTween().TweenCallback(Callable.From(() =>
		{
			Hud.SetHearts(Globals.CurrentHealth, Globals.DefaultMaxHealth);
			Hud.scoreBoard.SetNumber(Globals.Score);
			Hud.timeBoard.SetNumber(Globals.ElapsedSeconds);
		})).SetDelay(0.1f);
		
		CreateTween().SetLoops().TweenCallback(Callable.From(() =>
		{
			Globals.ElapsedSeconds += 1;
			Globals.Score += 1 * (int)Globals.Difficulty;
			Hud.scoreBoard.SetNumber(Globals.Score);
			Hud.timeBoard.SetNumber(Globals.ElapsedSeconds);
		})).SetDelay(1.0f);
		
		CreateTween().SetLoops().TweenCallback(Callable.From(() =>
		{
			Hud.SetHearts(Globals.CurrentHealth, Globals.CurrentMaxHealth);
		})).SetDelay(0.2f);
	}

	public override void _Process(double delta)
	{
	}
}

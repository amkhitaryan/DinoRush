using Godot;

public partial class GameScene : Node2D
{
	[Export]
	public Hud Hud;
	
	public override void _Ready()
	{
		CreateTween().TweenCallback(Callable.From(() => {
			Hud.scoreBoard.SetNumber(Globals.Score);
			Hud.timeBoard.SetNumber(Globals.ElapsedSeconds);
		})).SetDelay(0.1f);
		
		CreateTween().SetLoops().TweenCallback(Callable.From(() =>
		{
			Globals.ElapsedSeconds += 1;
			Hud.scoreBoard.SetNumber(Globals.Score);
			Hud.timeBoard.SetNumber(Globals.ElapsedSeconds);
		})).SetDelay(1.0f);
	}

	public override void _Process(double delta)
	{
	}
}

using Godot;
using System.Collections.Generic;

public partial class TimeBoard : HBoxContainer
{
	[Export]
	public TextureRect number1;
	[Export]
	public TextureRect number2;
	[Export]
	public TextureRect number3;
	[Export]
	public TextureRect number4;
	
	private static readonly Dictionary<int, Rect2> _atlasRegions = new()
	{
		{0, new Rect2(16, 0, 8, 8)},
		{1, new Rect2(24, 0, 8, 8)},
		{2, new Rect2(32, 0, 8, 8)},
		{3, new Rect2(40, 0, 8, 8)},
		{4, new Rect2(48, 0, 8, 8)},
		{5, new Rect2(16, 8, 8, 8)},
		{6, new Rect2(24, 8, 8, 8)},
		{7, new Rect2(32, 8, 8, 8)},
		{8, new Rect2(40, 8, 8, 8)},
		{9, new Rect2(48, 8, 8, 8)},
	};
	
	public override void _Ready()
	{
		number1.PivotOffset = number1.Size / 2.0f;
		number2.PivotOffset = number2.Size / 2.0f;
		number3.PivotOffset = number3.Size / 2.0f;
		number4.PivotOffset = number4.Size / 2.0f;
	}

	public override void _Process(double delta)
	{
	}

	public void SetNumber(int number)
	{
		if (number < 0)
		{
			number = 0;
		}
		else if (number > 5999)
		{
			number = 5999;
		}
		
		var minutes = number / 60;
		var seconds = number - minutes * 60;

		var minutes1 = minutes / 10;
		var minutes2 = minutes % 10;
		var seconds1 = seconds / 10;
		var seconds2 = seconds % 10;
		
		var atlasTexture = number1.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[minutes1])
		{
			BouncyAnimation(number1);
			atlasTexture.Region = _atlasRegions[minutes1];
		}

		atlasTexture = number2.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[minutes2])
		{
			BouncyAnimation(number2);
			atlasTexture.Region = _atlasRegions[minutes2];
		}

		atlasTexture = number3.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[seconds1])
		{
			BouncyAnimation(number3);
			atlasTexture.Region = _atlasRegions[seconds1];
		}

		atlasTexture = number4.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[seconds2])
		{
			BouncyAnimation(number4);
			atlasTexture.Region = _atlasRegions[seconds2];
		}
	}
	
	private void BouncyAnimation(Control control)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(control, "scale", Vector2.One * 1.25f, 0.1f);
		tween.TweenProperty(control, "scale", Vector2.One * 1.0f, 0.1f);
	}
}

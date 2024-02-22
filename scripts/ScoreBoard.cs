using Godot;
using System;
using System.Collections.Generic;

public partial class ScoreBoard : HBoxContainer
{
	[Export]
	public TextureRect number1;
	[Export]
	public TextureRect number2;
	[Export]
	public TextureRect number3;
	[Export]
	public TextureRect number4;
	[Export]
	public TextureRect number5;
	[Export]
	public TextureRect number6;
	[Export]
	public TextureRect number7;
	
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
		number5.PivotOffset = number5.Size / 2.0f;
		number6.PivotOffset = number6.Size / 2.0f;
		number7.PivotOffset = number7.Size / 2.0f;
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
		else if (number > 9999999)
		{
			number = 9999999;
		}

		var millions = number / 1000000;
		var hundredThousands = (number - millions * 1000000) / 100000;
		var tenThousands = (number - millions * 1000000 - hundredThousands * 100000) / 10000;
		var thousands = (number - millions * 1000000 - hundredThousands * 100000 - tenThousands * 10000) / 1000;
		var hundreds = (number - millions * 1000000 - hundredThousands * 100000 - tenThousands * 10000 - thousands * 1000) / 100;
		var tens = (number - millions * 1000000 - hundredThousands * 100000 - tenThousands * 10000 - thousands * 1000 - hundreds * 100) / 10;
		var ones = number - millions * 1000000 - hundredThousands * 100000 - tenThousands * 10000 - thousands * 1000 - hundreds * 100 - tens * 10;
		
		var atlasTexture = number1.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[millions])
		{
			BouncyAnimation(number1);
			atlasTexture.Region = _atlasRegions[millions];
		}

		atlasTexture = number2.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[hundredThousands])
		{
			BouncyAnimation(number2);
			atlasTexture.Region = _atlasRegions[hundredThousands];
		}

		atlasTexture = number3.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[tenThousands])
		{
			BouncyAnimation(number3);
			atlasTexture.Region = _atlasRegions[tenThousands];
		}

		atlasTexture = number4.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[thousands])
		{
			BouncyAnimation(number4);
			atlasTexture.Region = _atlasRegions[thousands];
		}
		
		atlasTexture = number5.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[hundreds])
		{
			BouncyAnimation(number5);
			atlasTexture.Region = _atlasRegions[hundreds];
		}
		
		atlasTexture = number6.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[tens])
		{
			BouncyAnimation(number6);
			atlasTexture.Region = _atlasRegions[tens];
		}
		
		atlasTexture = number7.Texture as AtlasTexture;
		if (atlasTexture.Region != _atlasRegions[ones])
		{
			BouncyAnimation(number7);
			atlasTexture.Region = _atlasRegions[ones];
		}
	}
	
	private void BouncyAnimation(Control control)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(control, "scale", Vector2.One * 1.25f, 0.1f);
		tween.TweenProperty(control, "scale", Vector2.One * 1.0f, 0.1f);
	}
}

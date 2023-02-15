using Godot;
using System;

public partial class Hud : CanvasLayer
{
	
	private Label _scoreLabel;
	public override void _Ready()
	{
		_scoreLabel = GetNode<Label>("Score");
	}

	public override void _Process(double delta)
	{
		_scoreLabel.Text = Convert.ToString(World.Score);
	}
}

using Godot;
using static Godot.GD;
using System;
using Godot.NativeInterop;

public partial class Pipes : Node2D
{
	private VisibleOnScreenNotifier2D _visible;
	private Player _player;
	private bool _dedPlayer;
	//private Area2D _scoreArea;
	public override void _Ready()
	{
		_visible = GetNode<VisibleOnScreenNotifier2D>("VisibilityNotifier");
		_visible.ScreenExited += () =>
			QueueFree();
		//_scoreArea = GetNode<Area2D>("ScoreArea");
	}

	void AddScore()
	{
		World.Score++;
		Print(World.Score);
	}

	public override void _Process(double timeDelta)
	{
		if (!Player.DedPlayer)
			Position -= new Vector2(World.FloorSpeed * (float)timeDelta, 0);
	}
}

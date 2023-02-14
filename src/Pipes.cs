using Godot;
using System;
using Godot.NativeInterop;

public partial class Pipes : Node2D
{
	private VisibleOnScreenNotifier2D _visible;
	public override void _Ready()
	{
		_visible = GetNode<VisibleOnScreenNotifier2D>("VisibilityNotifier");
		_visible.ScreenExited += () =>
			QueueFree();
	}

	public override void _Process(double timeDelta)
	{
		Position -= new Vector2(300 * (float)timeDelta, 0);
	}
}

using Godot;
using static Godot.GD;
using System;
using Godot.NativeInterop;

public partial class Pipes : Node2D
{
    public static event Action RefreshScore;
    private VisibleOnScreenNotifier2D _visible;
    private Player _player;
    private bool _dedPlayer;
    private Area2D _scoreArea;
    public override void _Ready()
    {
        _visible = GetNode<VisibleOnScreenNotifier2D>("VisibilityNotifier");
        _visible.ScreenExited += () =>
            QueueFree();
        Main.StartGame += () =>
            RefreshScore?.Invoke();
        _scoreArea = GetNode<Area2D>("ScoreArea");
        _scoreArea.BodyEntered += (Node2D) =>
        {
            World.Score++;
            RefreshScore?.Invoke();
        };
    }
    public override void _Process(double delta)
    {
        if (!Player.DedPlayer && !Main.IsPaused)
            Position -= new Vector2((World.SpeedRatio[0] + World.SpeedRatio[1] * World.SpeedModifier) * (float)delta, 0);
        if (!World.StartedPlaying)
            QueueFree();
    }
}

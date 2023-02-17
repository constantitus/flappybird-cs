using Godot;
using static Godot.GD;
using System;

public partial class PipeSpawner : Node2D
{
    public PackedScene Pipes;
    private Timer _timer;
    public override void _Ready()
    {
        Pipes = (PackedScene)Load("res://src/Scenes/Pipes.tscn");
        _timer = GetNode<Timer>("Timer");

        _timer.Timeout += SpawnPipe;

        Randomize();
    }
    void SpawnPipe()
    {
        var obstacle = Pipes.Instantiate();
        AddChild(obstacle);
        (obstacle as Node2D).Position = new Vector2(200, Randi() % 200);

    }

    public override void _Process(double delta)
    {
        if (Array.IndexOf(World.SpeedStages, World.Score) != -1)
        {
            var i = Array.IndexOf(World.SpeedStages, World.Score);
            World.SpeedModifier = World.SpeedChart[i];
            _timer.WaitTime = World.TimeRatio[0] - (World.TimeRatio[1] * World.SpeedModifier);
        }
    }
}

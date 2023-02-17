using Godot;
using static Godot.GD;
using System;
using System.Security;

public partial class World : Node2D
{
    public static int[] SpeedStages = { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public static float[] SpeedChart = { 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2.0f };
    public static float SpeedModifier;
    public static float[] SpeedRatio = { 100, 150 };        //	0 + 1 * speedmodifier
    public static float[] TimeRatio = { 1.2f, 0.3f };       // 1.2 - 0.3 * speedmodifier
    public static float[] Gravity = { 1600f, 200f };				// 1600 + 200 * SpeedModifier;

    public static int Score;
    public Player Player;
    public Node2D Lights;
    public TileMap Floor;
    private AnimatedSprite2D _bgImage;
    private ParallaxBackground _scrollingBg;
    private Vector2 _bgScrollOffset;
    private Timer _pipeTimer;
    private VisibleOnScreenNotifier2D _playerVisible;
    public static bool PlayerOffScreen;
    public static bool StartedPlaying;
    private event Action _startPlaying;

    public override void _Ready()
    {
        Player = GetNode<Player>("Player");
        _playerVisible = GetNode<VisibleOnScreenNotifier2D>("Player/PlayerVisible");
        _playerVisible.ScreenExited += _freePlayer;
        Lights = GetNode<Node2D>("Lights");
        Floor = GetNode<TileMap>("Floor");

        var floorVisible = Floor.GetChild<VisibleOnScreenNotifier2D>(0);
        floorVisible.ScreenExited += () =>
            Floor.Position += new Vector2(16 * 32, 0);

        _scrollingBg = GetNode<ParallaxBackground>("Background");

        _bgImage = GetNode<AnimatedSprite2D>("Background/ParallaxLayer/Background");
        _bgImage.Play();

        _pipeTimer = GetNode<Timer>("PipeSpawner/Timer");

        StartedPlaying = false;
        _startPlaying += () =>
        {
            _pipeTimer.Start();
            Input.ActionPress("Jump");
            StartedPlaying = true;
        };
        Hud.PauseGame += () =>
            _pipeTimer.Paused = true;
        Main.ResumeGame += () =>
            {
                _pipeTimer.Paused = false;
                if (Player.DedPlayer)
                {
                    Player.DedPlayer = false;
                }
            };
        Main.StartGame += () =>
        {
            World.Score = 0;
            Player.Position = new Vector2(115, 224);
            _pipeTimer.Start();
            StartedPlaying = false;
            PlayerOffScreen = false;
        };
    }

    public void _on_button_button_down()
    {
        Input.ActionPress("Jump");
    }
    public void _on_button_button_up()
    {
        Input.ActionRelease("Jump");
    }
    void _freePlayer()
    {
        PlayerOffScreen = true;
        _pipeTimer.Stop();
    }
    public override void _Process(double delta)
    {
        _scrollingBg.ScrollOffset = _bgScrollOffset;
        Lights.Position = Player.Position;
        if (!Player.DedPlayer && !Main.IsPaused)
        {
            Floor.Position -= new Vector2((World.SpeedRatio[0] + World.SpeedRatio[1] * World.SpeedModifier) * (float)delta, 0);
            _bgScrollOffset.X -= (World.SpeedRatio[0] + World.SpeedRatio[1] * World.SpeedModifier) / 2 * (float)delta;
        }
        if (Input.IsActionJustPressed("Jump") && !Main.IsPaused && !StartedPlaying)
            _startPlaying?.Invoke();
    }
}

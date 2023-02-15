using Godot;
using static Godot.GD;
using System;
using System.Security;

public partial class World : Node2D
{
	#region Constants and Variables
	public const float BackgroundSpeed = 100;
	public const float FloorSpeed = 250;
	public const float Gravity = 1800;

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

	#endregion
	
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
			if (!StartedPlaying)
			{
				_pipeTimer.Start();
				Input.ActionPress("Jump");
			}
			StartedPlaying = true;
		};
		

		Print(Score);
	}
		
	#region Touchscreen to Inputs
	public void _on_button_button_down()
	{
		Input.ActionPress("Jump");
	}
	public void _on_button_button_up()
	{
		Input.ActionRelease("Jump");
	}
	#endregion
	void _freePlayer()
	{
		PlayerOffScreen = true;
		_pipeTimer.Stop();
		Player.QueueFree();
	}
	public override void _Process(double timeDelta)
	{
		_scrollingBg.ScrollOffset = _bgScrollOffset;
		if (!PlayerOffScreen)
			Lights.Position = Player.Position;
			if (!Player.DedPlayer)
				{
					Floor.Position -= new Vector2( FloorSpeed * (float)timeDelta, 0);
					_bgScrollOffset.X -= BackgroundSpeed * (float)timeDelta;
				}
		if (Input.IsActionJustPressed("Jump"))
			_startPlaying?.Invoke();

	}
}

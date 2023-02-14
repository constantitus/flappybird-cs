using Godot;
using static Godot.GD;
using System;
using System.Security;

public partial class World : Node2D
{
	public const float BackgroundSpeed = 100;
	public const float FloorSpeed = 300;
	
	public Player Player;
	public Node2D Lights;
	
	public AnimatedSprite2D bgSprite;
	public ParallaxBackground ScrollingBg;
	private Vector2 _bgScrollOffset;

	public TileMap Floor;
	
	private bool _dedPlayer;

	public PackedScene Pipes;
	public override void _Ready()
	{
		Pipes = (PackedScene)Load("res://src/Pipes.tscn");
		
		Player = GetNode<Player>("Player");
		Player.Ded += delegate
		{
			_dedPlayer = true;
		};
		Lights = GetNode<Node2D>("Lights");

		Floor = GetNode<TileMap>("Floor");
		
		var visibility = Floor.GetChild<VisibleOnScreenNotifier2D>(0);
		visibility.ScreenExited += () =>
			Floor.Position += new Vector2(16 * 32, 0);

		ScrollingBg = GetNode<ParallaxBackground>("Background");

		bgSprite = GetNode<AnimatedSprite2D>("Background/ParallaxLayer/Background");
		bgSprite.Play();
	}
	//	x: 392
	public void HandlePipes(double timeDelta)
	{
	}

	public override void _Process(double timeDelta)
	{
		ScrollingBg.ScrollOffset = _bgScrollOffset;
		_bgScrollOffset.X -= BackgroundSpeed * (float)timeDelta;

		Floor.Position -= new Vector2( FloorSpeed * (float)timeDelta, 0);
		
		if (!_dedPlayer)
			Lights.Position = Player.Position;

		HandlePipes(timeDelta);
	}
}

using Godot;
using static Godot.GD;
using System;

public partial class Player : CharacterBody2D
{
	#region Constants and Vars

	private AnimatedSprite2D _birdSprite;
	
	public event Action Ded;
	public static bool DedPlayer;

	#endregion
	public override void _Ready()
	{

		_birdSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_birdSprite.Animation = "default";
		_birdSprite.Play();

		_birdSprite.AnimationFinished += () =>
		{
			if (_birdSprite.Animation == "default")
			{
				if (!World.StartedPlaying)
					_birdSprite.Play();
				else
					_birdSprite.Pause();
			}
		};
		Ded += () => _birdSprite.Animation = "dead";
	}

	public void CheckCollision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if ((collision.GetCollider() as Node).Name != "no")
			{
				DedPlayer = true;
				Ded?.Invoke();
			}
		}
	}

	void _movement(double delta)
	{
		Vector2 velocity = Velocity;

		// Handle Gravity
		if (!IsOnFloor())
			velocity.Y += World.Gravity * (float)delta;

		// Handle Flap
		if (Input.IsActionJustPressed("Jump"))
		{
			velocity.Y = -600f;
			_birdSprite.Play();
			_birdSprite.SetFrameAndProgress(0, 0);
		}
	
		// Fall-speed Cap
		if (velocity.Y > 700)
			velocity.Y = 700;

		Velocity = velocity;
		MoveAndSlide();
		CheckCollision();
	}
	private float _fallSpeed = 1;
	void _dying()
	{
		_fallSpeed = Mathf.Lerp(_fallSpeed, 100, 0.01f);
		Position += new Vector2 (0, _fallSpeed);
	}
	public override void _PhysicsProcess(double delta)
	{ 
		if (!DedPlayer)
			{ if (World.StartedPlaying) _movement(delta); }
		else if (!World.PlayerOffScreen)
			_dying();
	}
}

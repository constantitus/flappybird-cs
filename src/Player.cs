using Godot;
using System;

public partial class Player : CharacterBody2D
{
	#region Constants and Vars

	public AnimatedSprite2D birdSprite;
	public Sprite2D spotLight;
	public CanvasItemMaterial spotLightMode;
	public const float Gravity = 1800;
	
	public event Action Ded;

	#endregion
	public override void _Ready()
	{
		birdSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		birdSprite.AnimationFinished += BirdSpriteFinished;

		//spotLight = GetNode<Sprite2D>("LightSprite");
		//spotLightMode = (CanvasItemMaterial)spotLight.Material;

		//spotLightMode.BlendMode = CanvasItemMaterial.BlendModeEnum.Mul;
		
	}

	void BirdSpriteFinished()
	{
		var _currentAnimation = birdSprite.Animation;

		if (_currentAnimation == "default")
			birdSprite.Pause();
	}

	public void CheckCollision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if ((collision.GetCollider() as Node).Name != "no")
			{
				Ded?.Invoke();
				QueueFree();
			}
		}
	}

	private void _movement(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		// Handle Flap.
		if (Input.IsActionJustPressed("Jump"))
		{
			velocity.Y = -600f;
			birdSprite.Play();
			birdSprite.SetFrameAndProgress(0, 0);
		}
	

		if (velocity.Y > 700)
			velocity.Y = 700;

		Velocity = velocity;
		MoveAndSlide();
		CheckCollision();
	}

	public override void _PhysicsProcess(double delta)
	{ 
		_movement(delta);
	}
}

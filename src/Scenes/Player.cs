using Godot;
using static Godot.GD;
using System;

public partial class Player : CharacterBody2D
{
    private AnimatedSprite2D _birdSprite;

    public static event Action Ded;
    public static bool DedPlayer;

    public override void _Ready()
    {

        _birdSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Main.ResumeGame += () =>
        {
            if (!World.StartedPlaying)
                _birdSprite.Play("default");
        };
        Hud.PauseGame += () =>
        {
            _birdSprite.Pause();
        };
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
        Ded += () =>
            _birdSprite.Animation = "dead";
        Main.StartGame += () =>
            _birdSprite.Animation = "default";
        Main.ResumeGame += () =>
            Visible = true;
    }

    private bool _collidingWithFloor;
    public void CheckCollision()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if ((collision.GetCollider() as Node).Name != "no")
            {
                DedPlayer = true;
                Ded?.Invoke();
                _fallSpeed = 0;
                _collidingWithFloor = false;
            }
            else
                _collidingWithFloor = true;
        }
    }


    void _movement(double delta)
    {
        Vector2 velocity = Velocity;

        // Handle Gravity
        if (!IsOnFloor())
            velocity.Y += (World.Gravity[0] + World.Gravity[1] * World.SpeedModifier) * (float)delta;

        // Handle Flap
        if (Input.IsActionJustPressed("Jump"))
        {
            if (IsOnFloor() && _collidingWithFloor)
                velocity.Y = -900f;
            else
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
        Position += new Vector2(0, _fallSpeed);
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!DedPlayer)
        {
            if (World.StartedPlaying && !Main.IsPaused)
                _movement(delta);
        }
        else if (!World.PlayerOffScreen)
            _dying();
    }
}

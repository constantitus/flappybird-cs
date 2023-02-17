using Godot;
using static Godot.GD;
using System;

public partial class Main : Node
{
    public static event Action ResumeGame;
    public static event Action StartGame;
    public static bool IsPaused;

    private CanvasLayer _pauseMenu;
    private Button _resumeButton;
    private Label _resumeLabel;
    private Label _gameLabel;

    private PackedScene _world;
    private World World;
    public override void _Ready()
    {
        _resumeButton = GetNode<Button>("PauseMenu/Pause/Resume");
        _resumeLabel = GetNode<Label>("PauseMenu/Pause/Resume/ResumeLabel");
        _pauseMenu = GetNode<CanvasLayer>("PauseMenu");
        _gameLabel = GetNode<Label>("PauseMenu/Label");

        _gameLabel.Text = "Flappy Bird";
        _resumeLabel.Text = "Play";

        IsPaused = true;
        _resumeButton.ButtonDown += () =>
        {
            if (Player.DedPlayer)
            {
                StartGame?.Invoke();
                Player.DedPlayer = false;
            }
            ResumeGame?.Invoke();
            _pauseMenu.Visible = false;
            IsPaused = false;
        };
        Hud.PauseGame += () =>
        {
            _pauseMenu.Visible = true;
            _resumeLabel.Text = "Resume";
            _gameLabel.Text = "Floppy Diks";
        };

        Player.Ded += () =>
            {
                Hud.PauseButton.Visible = false;
                _pauseMenu.Visible = true;
                _resumeLabel.Text = "Retry";
                _gameLabel.Text = "MAHIRO MY WIFE";
            };
    }
}

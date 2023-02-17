using Godot;
using System;

public partial class Hud : CanvasLayer
{
    public static event Action PauseGame;
    private Label _scoreLabel;
    public static Button PauseButton;
    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>("Score");
        PauseButton = GetNode<Button>("Pause");

        PauseButton.ButtonDown += () =>
        {
            PauseGame?.Invoke();
            Main.IsPaused = true;
            PauseButton.Visible = false;
        };
        Main.ResumeGame += () =>
            PauseButton.Visible = true;
        Pipes.RefreshScore += () =>
            _scoreLabel.Text = Convert.ToString(World.Score);
    }
}
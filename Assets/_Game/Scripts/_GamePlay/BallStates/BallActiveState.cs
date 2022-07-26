using UnityEngine;
using Service;

public class BallActiveState : BallBaseState
{
    public override void EnterState(Ball ball)
    {
        ball.SetRandomColor();

        ball.ballRigidbody.useGravity = true;
        ball.ballRigidbody.isKinematic = false;

        ball.tag = "Active";
        ball.gameManager.activeBalls.Add(ball);
    }

    public override void OnCollisionEnter(Ball ball, Collision collision)
    {
        if (collision.gameObject.layer != PhysicsLayers.Obstacle) { return; }

        GameService.Instance.SoundManager.PlaySound(GameService.Instance.SoundManager.ballAudioClip);
        GameService.Instance.VibrationManager.HapticMedium();
    }
}

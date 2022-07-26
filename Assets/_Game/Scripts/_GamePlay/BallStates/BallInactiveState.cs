using UnityEngine;
using Service;

public class BallInactiveState : BallBaseState
{
    public override void EnterState(Ball ball)
    {
        ball.SetWhiteColor();

        ball.ballRigidbody.useGravity = false;
        ball.ballRigidbody.isKinematic = true;
    }

    public override void OnCollisionEnter(Ball ball, Collision collision)
    {
        if (collision.gameObject.layer != PhysicsLayers.Ball) { return; }
        if (!collision.gameObject.CompareTag("Active")) { return; }

        ball.TransitionToState(ball.activeState);

        GameService.Instance.SoundManager.PlaySound(GameService.Instance.SoundManager.ballAudioClip);
        GameService.Instance.VibrationManager.HapticSoft();
    }
}

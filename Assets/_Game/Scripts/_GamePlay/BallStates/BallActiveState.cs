using UnityEngine;

public class BallActiveState : BallBaseState
{
    private Color[] activeColors = { Color.red, Color.blue, Color.green, Color.magenta, Color.yellow };

    public override void EnterState(Ball ball)
    {
        // change of color
        ball.ballRenderer.material.SetColor("_Color", activeColors[Random.Range(0, activeColors.Length)]);

        ball.ballRigidbody.useGravity = true;
        ball.ballRigidbody.isKinematic = false;

        ball.tag = "Active";
        ball.gameManager.activeBalls.Add(ball);
    }

    public override void OnCollisionEnter(Ball ball, Collision collision)
    {
    }
}

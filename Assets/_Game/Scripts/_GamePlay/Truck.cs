using DG.Tweening;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public int m_CollisionCount;
    private GameManager m_GameManager;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable() => Service.GameEvents.OnPressedResultNextButton += MoveTruck;

    private void OnDisable() => Service.GameEvents.OnPressedResultNextButton -= MoveTruck;


    [ContextMenu("MoveTruck")]
    public void MoveTruck()
    {
        Ball[] balls = GetComponentsInChildren<Ball>();

        foreach (Ball ball in balls)
        {
            ball.DeactiveTrail();
            ball.ballRigidbody.constraints = RigidbodyConstraints.None;
            ball.ballRigidbody.isKinematic = true;
            ball.ballRigidbody.useGravity = false;
        }

        transform.DOMoveX(30, 10).SetEase(Ease.InOutQuad).SetSpeedBased(true).OnComplete(() =>
        {
            m_GameManager.DoGameOver(true);
        });
    }
}

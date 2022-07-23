using UnityEngine;

public class Balls : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    private BallBaseState m_CurrentState;
    public BallBaseState CurrentState { get => m_CurrentState; }

    public readonly BallActiveState activeState = new();
    public readonly BallInactiveState inactiveState = new();

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        TransitionToState(inactiveState);
    }


    public void TransitionToState(BallBaseState state)
    {
        m_CurrentState = state;
        m_CurrentState.EnterState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_CurrentState.OnCollisionEnter(this, collision);
    }
}

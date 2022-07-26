using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector] public GameManager gameManager;

    [HideInInspector] public Renderer ballRenderer;
    [HideInInspector] public Rigidbody ballRigidbody;

    private BallBaseState m_CurrentState;
    public BallBaseState CurrentState { get => m_CurrentState; }

    public readonly BallActiveState activeState = new();
    public readonly BallInactiveState inactiveState = new();

    public bool isEnteredFinalStage;

    private void Awake()
    {
        float randomSize = Random.Range(0.35f, 0.75f);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        ballRenderer = GetComponent<Renderer>();
        ballRigidbody = GetComponent<Rigidbody>();
        TransitionToState(inactiveState);

        if (gameManager == null) { gameManager = FindObjectOfType<GameManager>(); }
    }

    private void Start()
    {
        if (gameManager == null) { gameManager = FindObjectOfType<GameManager>(); }
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

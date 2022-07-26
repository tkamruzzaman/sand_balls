using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Color[] m_ActiveColors;

    [HideInInspector] public GameManager gameManager;

    private Renderer ballRenderer;
    private TrailRenderer ballTailRenderer;

    [HideInInspector] public Rigidbody ballRigidbody;

    private BallBaseState m_CurrentState;
    public BallBaseState CurrentState { get => m_CurrentState; }

    public readonly BallActiveState activeState = new();
    public readonly BallInactiveState inactiveState = new();

    public bool isEnteredFinalStage;

    private void Awake()
    {
        float randomSize = Random.Range(0.4f, 0.75f);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        ballRenderer = GetComponent<Renderer>();
        ballRigidbody = GetComponent<Rigidbody>();
        ballTailRenderer = GetComponentInChildren<TrailRenderer>();
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

    public void SetRandomColor()
    {
        Color color = m_ActiveColors[Random.Range(0, m_ActiveColors.Length)];
        ballRenderer.material.SetColor("_Color", color);
        ballTailRenderer.material.SetColor("_Color", color);
    }

    public void SetWhiteColor()
    {
        ballRenderer.material.SetColor("_Color", Color.white);
        ballTailRenderer.material.SetColor("_Color", Color.white);
    }
}

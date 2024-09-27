using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool _isActivateOnStart;

    [Header("References")]
    [SerializeField] private Spawner[] _spawners;

    [Header("Cannon Calculator Paramters")]
    [SerializeField] private float _angleInDegrees;
    [SerializeField] private Transform _enemyEndPoint;

    private CannonCalculatorService CannonCalculatorService;

    public ServiceLocator ServiceLocator { get; private set; }

    private void Awake() =>
        Initialize();


    private void Start()
    {
        if (_isActivateOnStart) StartGame();
    }

    private void Initialize()
    {
        ServiceLocator.Initialize();

        CannonCalculatorService = new CannonCalculatorService(_angleInDegrees, _enemyEndPoint);

        ServiceLocator.Instance.Register(CannonCalculatorService);
    }

    private void StartGame()
    {
        foreach (var spawner in _spawners)
        {
            spawner.StartSpawn();
        }
    }

    private void StopGame()
    {
        foreach (var spawner in _spawners)
        {
            spawner.StopSpawn();
        }
    }
}
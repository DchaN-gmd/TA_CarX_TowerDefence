using UnityEngine;

public class CannonTowerRotator : TowerRotator
{
    [Header("Override Parameters")]
    [SerializeField] private bool _activateDebugCube;

    [Header("Override References")]
    [SerializeField] private Transform _debugCube;

    [SerializeField] private Transform _cannonGuide;
    [SerializeField] private Transform _projectileStarTransform;
    
    private CannonCalculatorService _calculatorService;

    protected override void Awake()
    {
        base.Awake();

        _calculatorService = ServiceLocator.Instance.Get<CannonCalculatorService>();
    }

    private void Start() => 
        _cannonGuide.eulerAngles = new Vector3(-_calculatorService.AngleInDegrees, 0, 0);

    protected override void Rotate()
    {
        if (Tower.GetCurrentEnemy() == null) return;

        var enemy = Tower.GetCurrentEnemy();

        var interceptPoint =
            _calculatorService.GetInterceptPoint(enemy.transform, transform, enemy.Speed, _projectileStarTransform.position);

        Debug(interceptPoint);

        var lookRotation = Quaternion.LookRotation(interceptPoint - transform.position);
        var rotation = Quaternion.Slerp(transform.rotation, lookRotation, RotateSpeed * Time.deltaTime);
        rotation.x = 0;
        rotation.z = 0;

        transform.rotation = rotation;
    }

    private void Debug(Vector3 debugPosition)
    {
        if (_activateDebugCube)
        {
            _debugCube.gameObject.SetActive(true);
            _debugCube.transform.position = debugPosition;
        }
        else
        {
            if(_debugCube.gameObject.activeInHierarchy)
                _debugCube.gameObject.SetActive(true);
        }
    }
}
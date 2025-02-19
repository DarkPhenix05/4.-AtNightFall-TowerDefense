using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public float _speed;
    public int _startHeath;
    public int _health;
    public int _value;

    private Vector3 _enemyDirection;
    private Vector3 _targetTransform;
    private Vector3 _enemyTransform;
    private Transform _target;
    private int _wayPointIndex = 0;
    private PlayerStats _playerStats;
    private Vector3 _explodeSice;
    private Vector3 _startSice;
    public Ease _ease;
    public float _explodeTime;

    [Header("EneyUI")]
    public Image _healthBar;
    public float _fillAmount;

    private void Awake()
    {
        _startSice = transform.localScale;
    }

    private void Start()
    {
        _explodeTime = .25f;
        _explodeSice = new Vector3(3, 3, 3);
    }

    void OnEnable()
    {
        transform.localScale = _startSice;
        _ease = Ease.OutCubic;
        _target = WayPoints._wayPoints[0];
        _health = _startHeath;
        _playerStats = FindObjectOfType<PlayerStats>();
        _healthBar.fillAmount = 1.0f;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        _healthBar.fillAmount = (float)_health / _startHeath;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _playerStats.AddMoney(_value);
        gameObject.transform.DOScale(_explodeSice, _explodeTime).SetEase(_ease).OnComplete(()=>
            gameObject.SetActive(false));
        WaveManager._enemiesAlive--;
    }
    void Update()
    {
        if(!_target) { return; }

        _targetTransform = new Vector3(_target.position.x, 12, _target.position.z);
        _enemyTransform = new Vector3(transform.position.x, 12, transform.position.z);

        _enemyDirection = _targetTransform - _enemyTransform;
        transform.Translate(_enemyDirection.normalized * _speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(_enemyTransform, _targetTransform) <= 0.2f)
        {
            GetNextWayPoint();
        }
    }
    void GetNextWayPoint()
    {
        if( _wayPointIndex >= WayPoints._wayPoints.Length - 1)
        {
            this.gameObject.SetActive(false);
            _playerStats.ReduceLife(1); 
            _wayPointIndex = 0;
            _target = WayPoints._wayPoints[_wayPointIndex];
            WaveManager._enemiesAlive--;
            return;
        }
        _wayPointIndex++;
        _target = WayPoints._wayPoints[_wayPointIndex];
    }

    private void OnDisable()
    {
        _wayPointIndex = 0;
    }
}

using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class BowFox : MonoBehaviour
{
    TurretBP turretBP;

    [Header("Character Properties")]
    public int _level = 0;
    public float _range;
    public int _damage;
    public float _turnSpeed;
    public float _updateFrecuency;
    public int _upgradeCost;
    public int _saleCost;
    public int _value;
    public Material[] _materials;

    [Header("Transforms")]
    public GameObject _turretParent;
    public Transform _turretRotation;
    private Vector3 _lookDir;
    private Quaternion _quaternionRotation;
    private Vector3 _eulerRotation;

    [Header("Fire Properties")]
    public float _fireRate;
    public float _fireCountDown = 0;
    public GameObject _bullet;
    public Transform _gun;
    private GameObject _bulletGameObject;//Check
    public GameObject _bulletParent;//Check
    public List<GameObject> _bullets;
    private Bullet _bulletScript;
    public Vector3 _turretDeformation;
    private Vector3 _turretEndSice = new Vector3(1,1,1);
    public Ease _easeTurret;
    private float _fireTime;
    public bool _closest;
    public bool _moastHelath;
    public bool _leastHealth;


    [Header("Enemy Properties")]
    private Transform _targetPosition;
    private GameObject _nearestEnemy;
    private GameObject _leastHealthEnemy;
    private GameObject _mostHealthEnemy;
    private GameObject[] _enemies;
    public string _enemyTag;
    private float _shortestDistance;
    private float _distanceToEnemy;
    private float _minHelath;
    private float _maxHelath;
    private float _enemyHealth;

    [Header("Parent")]
    public string _TurretParentTag;


    //int GetHP() => hp;
    TurretUI _turretUI;
    private void Awake()
    {
        _bulletParent = GameObject.FindGameObjectWithTag("_bulletParent");
        _closest = true;
        SetStats();
    }
    private void Start()
    {
        _turretParent = GameObject.FindGameObjectWithTag(_TurretParentTag.ToString());
        transform.parent = _turretParent.transform;
    }

    void OnEnable()
    {
        _fireTime = _fireRate - (_fireRate / 2f);
        //InvokeRepeating("UpdateTarget", 0f, _updateFrecuency);
        SetStats();
    }
    void SetStats()
    {
        _level = 1;
        _range = BuildManager.instance.GetTurretToBuild()._range;
        _damage = BuildManager.instance.GetTurretToBuild()._damage;
        _turnSpeed = BuildManager.instance.GetTurretToBuild()._turnSpeed;
        _updateFrecuency = BuildManager.instance.GetTurretToBuild()._updateFrecuency;
        _upgradeCost = BuildManager.instance.GetTurretToBuild()._upgradeCost;
        _fireRate = BuildManager.instance.GetTurretToBuild()._fireRate;
        _materials = BuildManager.instance.GetTurretToBuild()._materials;
        _saleCost = Mathf.RoundToInt(BuildManager.instance.GetTurretToBuild()._cost *.6f);
    }

    void Update()
    {
        _fireCountDown -= Time.deltaTime;
        UpdateTarget();
        if (!_targetPosition)
        {
            return;
        }
        
        _lookDir = _targetPosition.position - transform.position;
        _quaternionRotation = Quaternion.LookRotation(_lookDir);
        _eulerRotation = Quaternion.Lerp(_turretRotation.rotation, _quaternionRotation, Time.deltaTime * _turnSpeed).eulerAngles;
        _turretRotation.rotation = Quaternion.Euler(0f, _eulerRotation.y , 0f);

        if (_fireCountDown <= 0)
        {
            Shoot();
            _fireCountDown = 1.0f / _fireRate;
        }

    }

    void UpdateTarget()
    {
         _enemies = GameObject.FindGameObjectsWithTag(_enemyTag);

        if (_closest) 
            ClosestTarget();

        else if (_leastHealth)
            LeastHealth();
        else
            MoastHealth();

    }

    void Shoot() //Change to pull
    {
        UpdateList();
        GameObject pullBullet;
        //MoveBullet
        pullBullet = _bullets[GetIndex()];
        pullBullet.transform.position = _gun.position;
        //SetBullet
        _bulletScript = pullBullet.GetComponent<Bullet>();
        _bulletScript.SetDamage(_damage);

        //Shoot
        pullBullet.SetActive(true);

        transform.DOScaleZ(_turretDeformation.z, _fireTime).SetEase(_easeTurret)
        .OnComplete(() => transform.DOScale(_turretEndSice, _fireTime));
        
        //Persue target
        if (_bulletScript) 
        {
            //Debug.Log(_damage);
            _bulletScript.Persue(_targetPosition);
        }
        //Debug.Log("Shot");
    }

    private int GetIndex()
    {
        int index = 0;
        for (index= 0; index < _bulletParent.transform.childCount; index++)
        {
            if(!_bullets[index].activeInHierarchy && index >= 0)
            {
                //Debug.Log(index);
                return index;
            }
        }

        _bulletGameObject = (GameObject)Instantiate(_bullet, _gun.position, _gun.rotation);
        _bulletGameObject.transform.parent = _bulletParent.transform;
        _bullets.Add(_bulletGameObject);
        return index++;
    }

    private void UpdateList()
    {
        _bullets.Clear();
        for (int i = 0; i < _bulletParent.transform.childCount; i++) 
        { 
            _bullets.Add(_bulletParent.transform.GetChild(i).gameObject); 
        }
    }

    void ClosestTarget()
    {
        _shortestDistance = Mathf.Infinity;
        _nearestEnemy = null;

        foreach (GameObject enemy in _enemies) //Thats new.
        {
            _distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (_distanceToEnemy < _shortestDistance)
            {
                _shortestDistance = _distanceToEnemy;
                _nearestEnemy = enemy;
            }
        }

        if (_nearestEnemy && _shortestDistance <= _range)
        {
            _targetPosition = _nearestEnemy.transform;
        }
        else
        {
            _targetPosition = null;
        }
    }

    void LeastHealth()
    {
        _minHelath = Mathf.Infinity;
        _leastHealthEnemy = null;
        int i = 0;
        foreach (GameObject enemy in _enemies) //Thats new.
        {
            _distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            _enemyHealth = _enemies[i].GetComponent<EnemyBehavior>()._health;
            if (_enemyHealth < _minHelath && _distanceToEnemy <= _range)
            {
                _minHelath = _enemyHealth;
                _leastHealthEnemy = enemy;
            }
        }


        if (_leastHealthEnemy && _distanceToEnemy <= _range)
        {
            _targetPosition = _leastHealthEnemy.transform;
        }
        else
        {
            _targetPosition = null; 
        }
    }

    void MoastHealth()
    {
        _maxHelath = 0f;
        _leastHealthEnemy = null;
        int i = 0;
        foreach (GameObject enemy in _enemies) //Thats new.
        {
            _distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            _enemyHealth = _enemies[i].GetComponent<EnemyBehavior>()._health;
            if (_enemyHealth > _maxHelath && _distanceToEnemy <= _range)
            {
                _maxHelath = _enemyHealth;
                _mostHealthEnemy = enemy;
            }
        }
        if (_mostHealthEnemy && _distanceToEnemy <= _range)
        {
            _targetPosition = _mostHealthEnemy.transform;
        }
        else
        {
            _targetPosition = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,0,0);
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    public void Upgrade(int _increment, int _damage)
    {
        _level += _increment;
        UpgradeStats(_level, _damage);
    }
    
    private void UpgradeStats(int level, int damage)
    {
        _range = _range * (level / 1.5f);
        _damage = Mathf.RoundToInt(Mathf.Sqrt(level * damage))+1;
        _fireRate = _fireRate + (_fireRate * .20f);
        _value = Mathf.RoundToInt(_value + _upgradeCost);
        _saleCost = Mathf.RoundToInt(_value * .60f);
        _upgradeCost = Mathf.RoundToInt(_upgradeCost * level/1.5f);
        GetComponentInChildren<Renderer>().material = _materials[level--];
    }

    public int ReturnUpgradeValue()
    {
        return _upgradeCost;
    }
    public int ReturSaleValue()
    {
        return _saleCost;
    }

    public int ReturnLevel()
    {
        return _level;
    }

    public float ReturnRange()
    {
        return _range;
    }

    public void ChangeToClosest()
    {
        _turretUI = FindObjectOfType<TurretUI>();
        //SetStates(1);

        _closest = true;
        _moastHelath = false;
        _leastHealth = false;
    }

    public void ChangeToLeastHealth()
    {
        _turretUI = FindObjectOfType<TurretUI>();
        //SetStates(2);

        _closest = false;
        _leastHealth = true;
        _moastHelath = false;
    }

    public void ChangeToMoastHealth()
    {
        _turretUI = FindObjectOfType<TurretUI>();
        
        _closest = false;
        _leastHealth = false;
        _moastHelath = true;
    }
}
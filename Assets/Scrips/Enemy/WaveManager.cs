using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("ENEMY BP")] 
    public EnemyBP _standardEnemyBP;
    public EnemyBP _standardEnemyBP1;
    public EnemyBP _standardEnemyBP2;
    public int _enemyToUse;
    
    [Header("ENEMYS LISTS")]
    private GameObject _enemyGameObject;
    public GameObject _enemy1Parent;
    public List<GameObject> _Enemy1;
    public GameObject _enemy2Parent;
    public List<GameObject> _Enemy2;
    public GameObject _enemy3Parent;
    public List<GameObject> _Enemy3;
    //public Transform _enemyPull; //Variable Brackeys
    public float _enemySeparation;

    [Header("TIMERS")]
    public static int _enemiesAlive = 0;
    public TMP_Text waveCountDownText;
    public float _waveCoolDown;
    [SerializeField]private float _countDown;

    [Header("WAVE")]
    private int _waveNumber;
    public Transform _spawner;

    private void Awake()
    {
        _waveNumber = 0;
        _enemy1Parent = _standardEnemyBP._parent;
        _enemy2Parent = _standardEnemyBP1._parent;
        _enemy3Parent = _standardEnemyBP2._parent;
        _countDown = 10f;
    }

    void Update()
    {
        if(_enemiesAlive > 0)
            return;

        if(_countDown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countDown = _waveCoolDown;
        }

        UITimer();
    }

    IEnumerator SpawnWave()
    {
        _waveNumber++;
        PlayerStats._wavesSurvived++;

        for (int i = 0; i<_waveNumber; i++)
        {
            SpawnEnemy(GetEnemyToSpawn());
            _enemiesAlive++;
            yield return new WaitForSeconds(_enemySeparation);
        }
    }

    private int GetEnemyToSpawn()
    {
        _enemyToUse = Mathf.Abs(Random.Range(-_waveNumber, _waveNumber));
        
        if (_enemyToUse < 1)
        {
            _enemyToUse = 1;
        }
        else if (_enemyToUse > 3)
        {
            _enemyToUse = 3;
        }

        return _enemyToUse;
    }
    //PULL DE ENEMIGOS
    private void SpawnEnemy (int enemyN)
    {
        switch (enemyN)
        {
            case 1:
                SpawnEnemy1();
                return;
            case 2:
                SpawnEnemy2();
                break;
            case 3:
                SpawnEnemy3();
                break;
        } 
    }
    void SpawnEnemy1()
    {
        UpdateList(1);
        GameObject pullEnemy;
        
        pullEnemy = _Enemy1[FindEnemy(1)];
        
        pullEnemy.transform.position = _spawner.transform.position;
        pullEnemy.GetComponent<EnemyBehavior>()._health = _standardEnemyBP._health;
        pullEnemy.GetComponent<EnemyBehavior>()._startHeath = _standardEnemyBP._health;
        pullEnemy.GetComponent<EnemyBehavior>()._speed = _standardEnemyBP._speed;
        pullEnemy.GetComponent<EnemyBehavior>()._value = _standardEnemyBP._value;
        pullEnemy.SetActive(true);
    }
    void SpawnEnemy2()
    {
        UpdateList(2);
        GameObject pullEnemy;
        
        pullEnemy = _Enemy2[FindEnemy(2)];
        pullEnemy.transform.position = _spawner.transform.position;
        pullEnemy.GetComponent<EnemyBehavior>()._health = _standardEnemyBP1._health;
        pullEnemy.GetComponent<EnemyBehavior>()._startHeath = _standardEnemyBP1._health;
        pullEnemy.GetComponent<EnemyBehavior>()._speed = _standardEnemyBP1._speed;
        pullEnemy.GetComponent<EnemyBehavior>()._value = _standardEnemyBP1._value;
       
        pullEnemy.SetActive(true); 
    }
    void SpawnEnemy3()
    {
        UpdateList(3);
        GameObject pullEnemy;
        
        pullEnemy = _Enemy3[FindEnemy(3)];
        pullEnemy.transform.position = _spawner.transform.position;
        pullEnemy.GetComponent<EnemyBehavior>()._health = _standardEnemyBP2._health;
        pullEnemy.GetComponent<EnemyBehavior>()._startHeath = _standardEnemyBP2._health;
        pullEnemy.GetComponent<EnemyBehavior>()._speed = _standardEnemyBP2._speed;
        pullEnemy.GetComponent<EnemyBehavior>()._value = _standardEnemyBP2._value;
       
        pullEnemy.SetActive(true); 
    }

    private int FindEnemy1()
    {
        int index = 0;
        for (index = 0; index < _enemy1Parent.transform.childCount; index++)
        {
            if (!_Enemy1[index].gameObject.activeInHierarchy && index >= 0 && index < _enemy1Parent.transform.childCount)
            {
                return index;
            }
            if (index > _enemy3Parent.transform.childCount)
            {
                Debug.Log("Error");
            }
        }

        _enemyGameObject= Instantiate(_standardEnemyBP._modelPre.gameObject, transform.position, transform.rotation);
        
        _enemyGameObject.transform.parent = _enemy1Parent.transform;
        _enemyGameObject.GetComponent<EnemyBehavior>()._health = _standardEnemyBP._health;
        _enemyGameObject.GetComponent<EnemyBehavior>()._speed = _standardEnemyBP._speed;
        _enemyGameObject.GetComponent<EnemyBehavior>()._value = _standardEnemyBP._value;
        _Enemy1.Add(_enemyGameObject.gameObject);
        return index++;
    }
    private int FindEnemy2()
    {
        int index= 0;
        for (index = 0; index < _enemy2Parent.transform.childCount; index++)
        {
            if (!_Enemy2[index].gameObject.activeInHierarchy && index >= 0 && index < _enemy2Parent.transform.childCount)
            {
                return index;
            }
            if(index > _enemy3Parent.transform.childCount)
            {
                Debug.Log("Error");
            }
        }

        _enemyGameObject= Instantiate(_standardEnemyBP1._modelPre.gameObject, transform.position, transform.rotation);
        _enemyGameObject.transform.parent = _enemy2Parent.transform;
        _enemyGameObject.GetComponent<EnemyBehavior>()._startHeath = _standardEnemyBP1._health;
        _enemyGameObject.GetComponent<EnemyBehavior>()._speed = _standardEnemyBP1._speed;
        _enemyGameObject.GetComponent<EnemyBehavior>()._value = _standardEnemyBP1._value;
        _Enemy2.Add(_enemyGameObject.gameObject);
        return index++;
    }
    private int FindEnemy3()
    {
        int index = 0;
        for (index = 0; index < _enemy3Parent.transform.childCount; index++)
        {
            if (!_Enemy3[index].gameObject.activeInHierarchy && index >= 0 && index < _enemy3Parent.transform.childCount)
            {
                return index;
            }
            if (index > _enemy3Parent.transform.childCount)
            {
                Debug.Log("Error");
            }
        }

        _enemyGameObject= Instantiate(_standardEnemyBP2._modelPre.gameObject, transform.position, transform.rotation);
        _enemyGameObject.transform.parent = _enemy3Parent.transform;
        _enemyGameObject.GetComponent<EnemyBehavior>()._startHeath = _standardEnemyBP2._health;
        _enemyGameObject.GetComponent<EnemyBehavior>()._speed = _standardEnemyBP2._speed;
        _enemyGameObject.GetComponent<EnemyBehavior>()._value = _standardEnemyBP2._value;
        _Enemy3.Add(_enemyGameObject.gameObject);
        return index++; 
    }

    private void UpdateList(int listN)
    {
        switch (listN)
        {
            case 1:
                _Enemy1.Clear();
                for (int i = 0; i < _enemy1Parent.transform.childCount; i++)
                {
                    _Enemy1.Add(_enemy1Parent.transform.GetChild(i).gameObject);
                }
                break;
            case 2:
                _Enemy2.Clear();
                for (int i = 0; i < _enemy2Parent.transform.childCount; i++)
                {
                    _Enemy2.Add(_enemy2Parent.transform.GetChild(i).gameObject);
                }
                break;
            case 3:
                _Enemy3.Clear();
                for (int i = 0; i < _enemy3Parent.transform.childCount; i++)
                {
                    _Enemy3.Add(_enemy3Parent.transform.GetChild(i).gameObject);
                }
                break; 
        }
    }

    private int FindEnemy(int listN)
    {
        switch (listN)
        {
            case 1:
                return FindEnemy1();
            case 2:
                return FindEnemy2();
            case 3:
                return FindEnemy3();
        }

        return 0;
    }
    
    //UI sistems
    private void UITimer()
    {
        _countDown -= Time.deltaTime;
        _countDown = Mathf.Clamp(_countDown, 0f, Mathf.Infinity);


        if (_countDown >= 3.5)
        {
            waveCountDownText.color = new Color(255, 255, 255, 255);
        }
        else
        {
            waveCountDownText.color = new Color(255, 0, 0, 255);
        }

        waveCountDownText.text = string.Format("{0:00.00}", _countDown);
    }

}

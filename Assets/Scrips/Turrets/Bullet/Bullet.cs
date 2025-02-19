using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    public float _speed;

    private int _damage;
    
    private float _distanceToMove;
    public GameObject _hitEfect;
    public GameObject _progectile;
    Vector3 _direction;

    public AudioClip _shoot;
    public AudioClip _hit;

    private EnemyBehavior _enemyBehavior;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    void Update()
    {
        if (!_target) 
        { 
            TurnOffProgectile();
            return;
        }

        _direction = _target.position - _progectile.transform.position;
        _distanceToMove= _speed * Time.deltaTime;

        if (_direction.magnitude <= _distanceToMove)
        {
            HitTarget();
            return;
        }

        _progectile.transform.Translate(_direction.normalized * _distanceToMove, Space.World); //Why _distanceToMove not Speed
    }

    public void Persue(Transform target)
    {
        GetComponent<AudioSource>().clip = _shoot;
        GetComponent<AudioSource>().Play();
        _enemyBehavior = target.GetComponent<EnemyBehavior>();

        if (!_enemyBehavior)
        {       
            return;
        }
       
        _target = target;
    }

    void HitTarget()
    {
        TurnOffProgectile();
        _enemyBehavior.TakeDamage(_damage);
        StartCoroutine(HitEfect());
        //Debug.Log("HIT");
    }

    IEnumerator HitEfect()
    {
        GetComponent<AudioSource>().clip = _hit;
        GetComponent<AudioSource>().Play();
        _hitEfect.transform.position = _progectile.transform.position;
        _progectile.transform.position = Vector3.zero;
        _hitEfect.SetActive(true);
        yield return new WaitForSeconds(1);
        _hitEfect.SetActive(false);
        Reset();
    }

    void TurnOffProgectile()//Change
    {
        _target = null;
        _progectile.gameObject.SetActive(false);
    }

    public void TurnOnProgectile()//Change
    {
        _progectile.gameObject.SetActive(true);
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        _progectile.transform.localPosition = Vector3.zero;
        TurnOnProgectile();
    }
}

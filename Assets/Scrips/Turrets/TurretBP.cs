using UnityEngine;

[System.Serializable]
public class TurretBP
{
    public GameObject _modelPre;
    public string _ParentTag;
    public int _level = 0;
    public float _range;
    public int _damage;
    public int _cost;
    public int _upgradeCost;
    public float _fireRate;
    public int _saleCost;

    public Material[] _materials;
    public float _turnSpeed;
    public float _updateFrecuency;
}

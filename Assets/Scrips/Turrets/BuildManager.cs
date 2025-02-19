using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance; //Singleton to dont need reference

    public TurretBP _turretToBuild;
    public TurretUI _turretUI;
    private Node _selectedNode;
    public List<GameObject> _turretList;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one BuildManager in srceen!");
            return;
        }
        _turretToBuild = null;
        instance = this;
    }

    public bool CanBuild { get { return _turretToBuild != null; } }
    public bool HaveMoeny { get { return PlayerStats._money >= _turretToBuild._cost; } }
    
    
    public void SelectNode(Node node)
    {
        if (_selectedNode == node)
        {
            DeselectNode();
            return;
        }
        _selectedNode = node;
        _selectedNode._turret.gameObject.transform.DOScale(new Vector3(1.8f,1.8f,1.8f), 0.25f).OnComplete(()=>
        _selectedNode._turret.gameObject.transform.DOScale(new Vector3(1f,1f,1f), 0.25f));
        _turretToBuild = null;
        
        _turretUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        _selectedNode._turret.gameObject.transform.DOScale(new Vector3(1,1,1), 0.25f);
        _selectedNode = null;
        _turretUI.DeSelectTarget();
    }
    public void SelectTurretToBuild(TurretBP turretBP)
    {
        if(_selectedNode != null)
        {
            DeselectNode();
        }

        _turretToBuild = turretBP;
        _selectedNode = null;
        _selectedNode = null;
    }

    public TurretBP GetTurretToBuild()
    {
        return _turretToBuild;
    }
}

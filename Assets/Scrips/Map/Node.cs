using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Material _hoverMaterialCanBuy;
    public Material _hoverMaterialCantBuy;
    public Vector3 _positionOffset;

    public Vector3 _standardScale;
    public Vector3 _maxScale;
    public Vector3 _zeroScale;
    public float _tweenTime;
    public Ease _openEase;
    public Ease _destroyEase;


    [HideInInspector]
    public GameObject _turret;
    [HideInInspector]
    public TurretBP _curTurretBP;

    private Material _defaultMaterial;
    private MeshRenderer _rend;


    BuildManager buildManager;
    MoneyUI _moneyUI;
    void Awake()
    {
        _rend = GetComponent<MeshRenderer>();
        _defaultMaterial = _rend.material;
        _defaultMaterial = _rend.material;
        _moneyUI = FindObjectOfType<MoneyUI>();
    }

    void Start()
    {
        buildManager = BuildManager.instance;
        _standardScale = new Vector3(1, 1, 1);
        _zeroScale = new Vector3(0, 0, 0);
        _tweenTime= 1f;
        _openEase = Ease.OutElastic;
        _destroyEase = Ease.InFlash;
    }


    public Vector3 GetBuildPosition()
    {
        return transform.position + _positionOffset;
    }


    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (buildManager._turretToBuild == null)
        {
            return ;
        }

        if (!buildManager.CanBuild)
        { 
            return; 
        }

        if (_turret != null)
        {
            return;
        }

        if (buildManager.HaveMoeny)
        {
            _rend.material = _hoverMaterialCanBuy;
            return;
        }

        _rend.material = _hoverMaterialCantBuy;
    }

    private void OnMouseExit()
    {
        _rend.material = _defaultMaterial;
    }

    private void OnMouseDown()
    {
        if(_turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        { 
            return; 
        }

        //Build Torret;
        BuildTurret(buildManager.GetTurretToBuild());
    }

    void BuildTurret(TurretBP turretBP)
    {
        if (turretBP._modelPre == null)
        {
            return;
        }

        if (PlayerStats._money < turretBP._cost)
        {
            Debug.Log("Not Enough Stars");
            return;
        }
        
        Buy(turretBP);
        InstanciteTurret(turretBP);
        BuildAnimation();
    }
    private void InstanciteTurret(TurretBP turretBP)
    {
        GameObject turret = Instantiate(turretBP._modelPre, GetBuildPosition(), Quaternion.identity);
        _turret = turret.gameObject;
        _turret.transform.localScale = new Vector3(0, 0, 0);
        turret.GetComponent<BowFox>()._TurretParentTag = turretBP._ParentTag;
        turret.GetComponent<BowFox>()._upgradeCost = turretBP._upgradeCost;
        turret.SetActive(true);
        _curTurretBP = turretBP;
    }

    private void Buy(TurretBP turretBP)
    {
        PlayerStats._money -= turretBP._cost;
        _moneyUI.UpdateGiveMoney();
    }

    private void BuildAnimation()
    {
        _turret.transform.transform.localScale = new Vector3(0, 0, 0);
        _turret.transform.transform.DOScale(_standardScale, _tweenTime).SetEase(_openEase);
    }

    public void TurretUpgrade()
    {
        if (PlayerStats._money < _curTurretBP._upgradeCost)
        {
            Debug.Log("Not Enough Stars");
            return;
        }

        PayUpgrade(_curTurretBP);
        AplyUpgrade(_curTurretBP);
        UpgradeAnimation();
    }

    private void PayUpgrade(TurretBP turretBP)
    {
        PlayerStats._money -= turretBP._upgradeCost;
        _moneyUI.UpdateGiveMoney();
    }

    private void AplyUpgrade(TurretBP turretBP)
    {
        _turret.GetComponent<BowFox>().Upgrade(1, turretBP._damage);
    }

    private void UpgradeAnimation()
    {
        _turret.transform.transform.localScale = _zeroScale;
        _turret.transform.transform.DOScale(_maxScale, _tweenTime).SetEase(_openEase).OnComplete(()=>
        _turret.transform.transform.DOScale(_standardScale, _tweenTime).SetEase(_openEase));
    }

    public void SellTurret()
    {
        PlayerStats._money += _turret.GetComponent<BowFox>().ReturSaleValue();
        _turret.transform.transform.DOScale(_zeroScale, (_tweenTime/2)).SetEase(_destroyEase).OnComplete(()=>
        Destroy(_turret));
    }
}

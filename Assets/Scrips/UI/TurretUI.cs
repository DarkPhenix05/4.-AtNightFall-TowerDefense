using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    private Node _target;
    private Animator _animator;
    public TMP_Text _upgradeText;
    public TMP_Text _sellText;
    public Canvas _canvas;
    public Toggle _distanceTog;
    public Toggle _leastLifeTog;
    public Toggle _mostLifeTog;
    private bool _maxLevel;
    private Vector3 _vectorZero = Vector3.zero;

    private BowFox _bowFox;
    public MoneyUI _moneyUI;
    void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    public void SetTarget (Node target)
    {
        _target = target;
        _bowFox = target._turret.GetComponent<BowFox>();

        _canvas.transform.DOScale(_bowFox.ReturnRange(), .25f);

        if(_bowFox.ReturnLevel() < 7)
        {
            target._curTurretBP._upgradeCost = _bowFox.ReturnUpgradeValue();
            _upgradeText.text = "Upgrade \n -$" + target._curTurretBP._upgradeCost.ToString();
            _maxLevel = false;
        }
        else
        {
            _upgradeText.text = "MAX \n LEVEL";
            _maxLevel = true;
        }

        target._curTurretBP._saleCost = _bowFox.ReturSaleValue();
        _sellText.text = "Sell \n +$" + target._curTurretBP._saleCost.ToString();
        
        transform.position = target.GetBuildPosition();
        //_animator.ResetTrigger("Close");
        gameObject.SetActive(true);
    }

    public void DeSelectTarget()
    {
        _canvas.transform.DOScale(_vectorZero, .25f);
        _animator.SetTrigger("Close");
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeTurret()
    {
        if (_maxLevel)
        {
            return;
        }
        if(PlayerStats._money < _target._curTurretBP._upgradeCost)
        {
            return ;
        }
        _target.TurretUpgrade();
        BuildManager.instance.DeselectNode();
    }

    public void SellTurret()
    {
        _target.SellTurret();
        BuildManager.instance.DeselectNode();
        _moneyUI.UpdateGetMoney();
    }

    public void Closest(bool toggled)
    {
        if (!toggled) return;
        _bowFox.ChangeToClosest();
        SetState(1);
    }

    public void Least(bool toggled)
    {
        if (!toggled) return;
        _bowFox.ChangeToLeastHealth();
        SetState(2);
    }

    public void Moast(bool toggled)
    {
        if (!toggled) return;
        _bowFox.ChangeToMoastHealth();
        SetState(3);
    }

    public void SetState(int _case)
    {
        switch (_case)
        {
            case 1:
                _leastLifeTog.isOn = false;
                _mostLifeTog.isOn = false;
                _distanceTog.isOn = true;
                break;
            case 2:
                _leastLifeTog.isOn = true;
                _mostLifeTog.isOn = false;
                _distanceTog.isOn = false;
                break;
            case 3:
                _leastLifeTog.isOn = false;
                _mostLifeTog.isOn = true;
                _distanceTog.isOn = false;
                break;
        }
    }
}

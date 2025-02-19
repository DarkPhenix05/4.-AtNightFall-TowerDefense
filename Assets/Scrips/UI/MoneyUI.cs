using DG.Tweening;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TMP_Text _moneyText;
    public Color _baseColor;
    public Color _getColor;
    public Color _sellColor;
    public float _changeTime;
    public AudioClip _money;
    void Start()
    {
        _moneyText.text = "$" + PlayerStats._money.ToString();
    }
    public void UpdateGetMoney()
    {
        GetComponent<AudioSource>().clip = _money;
        GetComponent<AudioSource>().Play();

        _moneyText.DOBlendableColor(_getColor, _changeTime).OnComplete(()=> 
        _moneyText.DOBlendableColor(_baseColor, _changeTime));
        _moneyText.text = "$" + PlayerStats._money.ToString();
    }
    public void UpdateGiveMoney()
    {
        GetComponent<AudioSource>().clip = _money;
        GetComponent<AudioSource>().Play();

        _moneyText.DOBlendableColor(_sellColor, _changeTime).OnComplete(() =>
        _moneyText.DOBlendableColor(_baseColor, _changeTime));
        _moneyText.text = "$" + PlayerStats._money.ToString();
    }
}

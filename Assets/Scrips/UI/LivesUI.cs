using UnityEngine;
using DG.Tweening;
using TMPro;

public class LivesUI : MonoBehaviour
{
    public TMP_Text _liveText;
    private int _curLife;
    public Color _baseColor;
    public Color _changeColor;
    public Color _dangerColor;
    public Color _deadColor;
    public float _changeTime;
    public AudioClip _bonk;

    public void Awake()
    {
        _liveText.DOColor(_changeColor, _changeTime).OnComplete(() =>
            _liveText.DOColor(CheckHPLevle(), _changeTime));

        _curLife = PlayerStats._curlife;

        _liveText.text = "Lives: " + _curLife.ToString();
    }

    public void UpdateLifeUI()
    {
        GetComponent<AudioSource>().clip = _bonk;
        GetComponent<AudioSource>().Play();
        _liveText.DOColor(_changeColor, _changeTime).OnComplete(() => 
            _liveText.DOColor(CheckHPLevle(), _changeTime));

        _curLife = PlayerStats._curlife;

        _liveText.text = "Lives: " + _curLife.ToString();
    }

    private Color CheckHPLevle()
    {
        if(_curLife > 20)
        {
            return _baseColor;
        }
        else if(_curLife > 10)
        {
            return _dangerColor;
        }
        else
        {
            return _deadColor;
        }
    }
}
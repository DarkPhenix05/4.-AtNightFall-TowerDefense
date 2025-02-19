using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int _money;
    public static int _curlife;
    public static int _wavesSurvived;
    public int _startMoney;
    public int _startLife;
    private LivesUI _livesUI;
    private MoneyUI _moneyUI;
    private GameManager _gameManager;
    private void Awake()
    {
        _money = _startMoney;
        _curlife = _startLife;
        _wavesSurvived = 0;
        _livesUI = FindObjectOfType<LivesUI>();
        _livesUI.UpdateLifeUI();
        _gameManager = FindObjectOfType<GameManager>();
        _moneyUI = FindObjectOfType<MoneyUI>();
    }

    public void ReduceLife(int damage)
    {
        _curlife -= damage;
        _livesUI.UpdateLifeUI();
        if (_curlife <= 0)
        {
            _gameManager.EndGame();
        }
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        _moneyUI.UpdateGetMoney();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _gameOverCanvas;
    public GameObject _vicotryCanvas;
    public GameObject _downCanvas;
    public GameObject _shopCanvas;
    public static bool _gameOver;

    private void Start()
    {
        _gameOver = false;
    }

    private void Update()
    {
        if (PlayerStats._curlife <= 20)
        {
            EndGame();
        }
        if(Input.GetKeyUp(KeyCode.V) || PlayerStats._wavesSurvived > 20) 
        {
            WinLevel();
        }
    }

    public void EndGame()
    {
        _gameOver = true;
        _downCanvas.SetActive(false);
        _shopCanvas.SetActive(false);
        _gameOverCanvas.SetActive(true);
    }

    public void WinLevel()
    {
        _gameOver = true;
        _vicotryCanvas.SetActive(true);
    }
}

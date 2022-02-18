using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public Text playerNameText;
    public string playerName;
    
    public Button startBotton;
    public Button quitBotton;

    public bool isGameActive = false;

    public MainManager mainManager;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void StartGame()
    {
        DontDestroyOnLoad(gameObject);
        Instance.playerName = playerNameText.text;
        SceneManager.LoadScene(1);
    }
    
}

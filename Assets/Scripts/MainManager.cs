using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public DataManager Instance;
    public int bestScore;
    public string bestPlayerName;
    private void Awake()
    {
        bestScore = 0;
        bestPlayerName = "";
        Instance = DataManager.Instance;
        LoadRank();
        DisplayBestScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > bestScore)
        {
            SaveRank();
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    
    [Serializable]
    public class Rank
    {
        public int bestScore;
        public string playerName;
    }

    public void SaveRank()
    {
        Rank rank = new Rank();
        rank.bestScore = m_Points;
        rank.playerName = Instance.playerName;
        string json = JsonUtility.ToJson(rank);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }

    public void LoadRank()
    {
        string path = Application.persistentDataPath + "/saveFile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Rank rank = JsonUtility.FromJson<Rank>(json);
            bestScore = rank.bestScore;
            bestPlayerName = rank.playerName;
        }
    }

    void DisplayBestScore()
    {
        if (bestPlayerName != "" && bestScore != 0)
        {
            BestScoreText.text = "Best Score : " + bestPlayerName + " : " + bestScore;
        }
        else
        {
            BestScoreText.text = "Best Score : Name : 0";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InputField playerNameText;
    [SerializeField] private TextMeshProUGUI currentHighScoreText;
    [SerializeField] private TextMeshProUGUI requiredText;
    [SerializeField] private Button startButton;

    [HideInInspector] public static MenuManager menuInstance;
    [HideInInspector] public string playerCurrentName;
    [HideInInspector] public string currentHighScoreName;
    [HideInInspector] public int currentHighScore;
    private PlayerData highScoreData;
    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();
        startButton.onClick.AddListener(StartGame);

        if (menuInstance == null)
        {
            MenuManager.menuInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartGame()
    {
        playerCurrentName = playerNameText.text;
        if (!string.IsNullOrEmpty(playerCurrentName))
        {
            SceneManager.LoadScene("main");
        }
        else
        {
            requiredText.gameObject.SetActive(true);
        }
    }

    public void SaveHighScore(string path, int m_Points)
    {
        highScoreData = new PlayerData();
        highScoreData.playerName = MenuManager.menuInstance.playerCurrentName;
        highScoreData.playerHighScore = m_Points;

        currentHighScoreName = MenuManager.menuInstance.playerCurrentName;
        currentHighScore = m_Points;

        string json = JsonUtility.ToJson(highScoreData);
        File.WriteAllText(path, json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/saveHighScore.json";
        if (CheckFile(path))
        {
            string json = File.ReadAllText(path);
            highScoreData = JsonUtility.FromJson<PlayerData>(json);
            currentHighScoreName = highScoreData.playerName;
            currentHighScore = highScoreData.playerHighScore;

            currentHighScoreText.text = "HighScore: " + currentHighScoreName + " - " + currentHighScore;
        }
        else
        {
            currentHighScoreText.text = "No HighScore";
        }

    }

    public bool CheckFile(string path) { return File.Exists(path); }

    [System.Serializable]
    class PlayerData
    {
        public string playerName;
        public int playerHighScore;
    }
}

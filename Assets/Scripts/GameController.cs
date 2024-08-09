

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static Action<string> OnGameComplete;
    public static Action OnTurnStart;
    public static Action OnTurnChange;

    int playerTurn; // 0 for video, 1 for audio

    public List<string> gameMoves = new List<string>();
    public string[,] gameBoard = new string[3, 3]; // 3x3 game board

    //GamePlay UI
    public CanvasGroup boardCG;
    public TextMeshProUGUI playerTurnText;

    //GameComplete UI
    public GameObject gameCompletePanel;
    public TextMeshProUGUI winPlayerText;

   

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        playerTurn = 0; // Video starts first
        OnTurnStart.Invoke();
    }

    private void OnEnable()
    {
        MediaDownloadManager.onMediaDownload += onMediaDownload;
        MediaDownloadManager.onMediaPlayComplete += onMediaPlayComplete;
        OnTurnChange += TurnChange;
        OnGameComplete += GameComplete;
    }



    private void OnDisable()
    {
        MediaDownloadManager.onMediaDownload -= onMediaDownload;
        MediaDownloadManager.onMediaPlayComplete -= onMediaPlayComplete;
        OnTurnChange -= TurnChange;
        OnGameComplete -= GameComplete;
    }


    public int PlayerTurn()
    {
        return playerTurn;
    }

    private void TurnChange()
    {
        playerTurn = 1 - playerTurn;
        OnTurnStart.Invoke();
    }

    private void onMediaDownload()
    {
        boardCG.interactable = false;
    }

    private void onMediaPlayComplete()
    {
        boardCG.interactable = true;
    }


    public IEnumerator DownloadAndPlayMedia(Button btn)
    {
        //Debug.Log("DownloadAndPlayMedia");
        IMediaPlayer mediaPlayer = playerTurn == 0 ? new VideoPlayer(btn.transform) : new AudioPlayer(btn.transform);
        btn.GetComponent<Cell>().mediaPlayer = mediaPlayer;
        mediaPlayer.Play();
        yield return new WaitUntil(() => mediaPlayer.IsMediaLoaded());
    }

    public string CheckForWinner()
    {
        // Check rows and columns
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[i, 0] == gameBoard[i, 1] && gameBoard[i, 1] == gameBoard[i, 2] && gameBoard[i, 0] != null)
            {
                //Debug.Log("Row " + gameBoard[i, 0]);
                return gameBoard[i, 0];
            }
            else if (gameBoard[0, i] == gameBoard[1, i] && gameBoard[1, i] == gameBoard[2, i] && gameBoard[0, i] != null)
            {
               // Debug.Log("col " + gameBoard[0, i]);
                return gameBoard[0, i];
            }
        }

        // Check diagonals
        if ((gameBoard[0, 0] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 2] && gameBoard[0, 0] != null) ||
            (gameBoard[0, 2] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 0] && gameBoard[0, 2] != null))
        {
            Debug.Log(gameBoard[1, 1]);
            return gameBoard[1, 1];
        }

        return null;
    }

    public bool IsGameComplete()
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[i, 0] == null || gameBoard[i, 1] == null || gameBoard[i, 2] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void GameComplete(string winner)
    {
       // Debug.Log(winner);
        gameCompletePanel.SetActive(true);
        if (!ReferenceEquals(winner, null))
        {
            winPlayerText.text = $"Congratulations<br><b>{(winner == "X" ? "Player 1" : "Player 2")}</b> is Won ";
        }
        SaveGameMoves();
    }


    void SaveGameMoves()
    {
        StringBuilder jsonBuilder = new StringBuilder();
        jsonBuilder.Append("{\n");

        for (int i = 0; i < gameMoves.Count; i++)
        {
            jsonBuilder.AppendFormat("  Move{0}:{1}", i + 1, gameMoves[i]);

            if (i < gameMoves.Count - 1)
            {
                jsonBuilder.Append(",\n");
            }
            else
            {
                jsonBuilder.Append("\n");
            }
        }

        jsonBuilder.Append("}");

        string json = jsonBuilder.ToString();

        Debug.Log(json);
        string path = Path.Combine(Application.persistentDataPath, "gameplay.json");
        File.WriteAllText(path, json);
        Debug.Log($"Gameplay saved to {path}");
    }

    public static Sprite GetPlayerSprite(int playerTurn)
    {
        return playerTurn == 0 ? Resources.Load<Sprite>("VideoSprite") : Resources.Load<Sprite>("AudioSprite");
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

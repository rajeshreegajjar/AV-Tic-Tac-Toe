using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Button cellBtn;
    public string cellId;
    public IMediaPlayer mediaPlayer;

    private void Awake()
    {
        cellBtn = GetComponent<Button>();
    }

    private void Start()
    {

        cellBtn?.onClick.AddListener(() => OnButtonClick(cellBtn));
    }

    private void OnEnable()
    {
        GameController.OnGameComplete += GameComplete;
    }
    private void OnDisable()
    {
        GameController.OnGameComplete -= GameComplete;
    }

    private void GameComplete(string winner)
    {
        cellBtn?.onClick.RemoveAllListeners();
    }

    private void onMediaPlayComplete()
    {
        if (cellBtn.image.sprite == null)
        {
            cellBtn.image.sprite = GameController.GetPlayerSprite(GameController.Instance.PlayerTurn());

            MediaDownloadManager.onMediaPlayComplete -= onMediaPlayComplete;

            Debug.Log($"{name}");
            string winner = GameController.Instance.CheckForWinner();
            if (winner != null)
            {
                Debug.Log($"Player {(winner == "X" ? "P1" : "P2")} wins!");
                //Show The Game Winner
                GameController.OnGameComplete.Invoke(winner);
            }
            else
            {
                //Check Game Complete
                if (GameController.Instance.IsGameComplete())
                {
                    GameController.OnGameComplete.Invoke(null);
                }
                else
                {
                    //Change Player Turn
                    GameController.OnTurnChange.Invoke();
                }
            }
            
        }
          
    }

    void OnButtonClick(Button btn)
    {
        Debug.Log($"{btn.name}");

        if (btn.image.sprite == null)
        {
            if (GameController.Instance == null)
                return;

            btn.enabled = false;
            MediaDownloadManager.onMediaPlayComplete += onMediaPlayComplete;
            Debug.Log($"{btn.name}");
            string cellId = btn.name; // e.g., "Button_0_0"
            GameController.Instance.gameMoves.Add($"{(GameController.Instance.PlayerTurn() == 0 ? "P1" : "P2")}-{cellId}");
            int row = cellId[0] - '0'; // Assuming button names are "Button_0_0"
            int col = cellId[1] - '0';
            Debug.Log(row + " - " +col);
            GameController.Instance.gameBoard[row, col] = GameController.Instance.PlayerTurn() == 0 ? "X" : "O";
           
            StartCoroutine(GameController.Instance.DownloadAndPlayMedia(btn));
        }
    }
}

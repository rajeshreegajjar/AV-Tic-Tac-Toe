using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SetPlayerTurnText : MonoBehaviour
{
    private TextMeshProUGUI playerTurnText;

    private void Awake()
    {
        playerTurnText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameController.OnTurnStart += GameController_onPlayerTurnChange;
    }
    private void OnDisable()
    {
        GameController.OnTurnStart -= GameController_onPlayerTurnChange;
    }

    private void GameController_onPlayerTurnChange()
    {
        playerTurnText.text = "Turn : " + (GameController.Instance.PlayerTurn()==0?"Player 1":"Player 2");
    }
}

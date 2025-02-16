using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text winnerNameText;
    public void updateStatus(string winnerName)
    {
        winnerNameText.text =  winnerName + " WIN!!!";
    }
}

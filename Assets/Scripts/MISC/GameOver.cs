using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notificationText;
    private void OnTriggerEnter(Collider other)
    {
        notificationText.text = "You Win, Press R to Replay";
    }
}

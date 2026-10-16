using UnityEngine;
using TMPro;

// Put this on the Mission Complete screen to show the player's total time.
public class MissionCompleteUI : MonoBehaviour
{
    public TMP_Text timeText;

    private void Start()
    {
        float t = GameManager.TotalTimeTaken;
        int mins = Mathf.FloorToInt(t / 60f);
        int secs = Mathf.FloorToInt(t % 60f);
        if (timeText) timeText.text = "Your time: " + mins + ":" + secs.ToString("00");
    }
}

using UnityEngine;
using TMPro;

/// <summary>
/// Formats and updates the UI timer for the next match and current match countdown.
/// </summary>
public class CountdownTimerUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI hubCountdownText;

    public void UpdateHubTimer(int secondsLeft) {
        hubCountdownText.text = FormatSecondsToMinutes(secondsLeft);
    }

    public static string FormatSecondsToMinutes(int timer) {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}

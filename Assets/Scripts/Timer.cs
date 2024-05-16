using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 180; // 초 단위로 시간을 설정합니다.

    void Start()
    {
        DisplayTime(timeRemaining); // 시작 시간을 표시합니다.
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // 프레임마다 시간을 감소시킵니다.
            DisplayTime(timeRemaining); // 시간을 표시합니다.
        }
        else
        {
            timerText.text = "00:00"; // 시간이 끝나면 00:00으로 표시합니다.
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        int minutes = Mathf.FloorToInt(timeToDisplay / 60); // 분을 계산합니다.
        int seconds = Mathf.FloorToInt(timeToDisplay % 60); // 초를 계산합니다.

        // 시간을 포맷하여 텍스트로 표시합니다.
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

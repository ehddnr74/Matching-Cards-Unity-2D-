using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float timeRemaining = 180; // �� ������ �ð��� �����մϴ�.

    void Start()
    {
        DisplayTime(timeRemaining); // ���� �ð��� ǥ���մϴ�.
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // �����Ӹ��� �ð��� ���ҽ�ŵ�ϴ�.
            DisplayTime(timeRemaining); // �ð��� ǥ���մϴ�.
        }
        else
        {
            timerText.text = "00:00"; // �ð��� ������ 00:00���� ǥ���մϴ�.
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        int minutes = Mathf.FloorToInt(timeToDisplay / 60); // ���� ����մϴ�.
        int seconds = Mathf.FloorToInt(timeToDisplay % 60); // �ʸ� ����մϴ�.

        // �ð��� �����Ͽ� �ؽ�Ʈ�� ǥ���մϴ�.
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

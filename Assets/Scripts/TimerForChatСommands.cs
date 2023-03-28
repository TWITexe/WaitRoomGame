using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerForChatСommands : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Image timerImage;
    [SerializeField] private Text timerText;
    public bool TimerOn { get; set; } = false;         // Работает ли сейчас таймер
    public float timeLeft = 0f;                        // Время таймера
    private IEnumerator CoroutineForTimer()
    {
        while (timeLeft > 0)
        {
            gameObject.GetComponent<Image>().enabled = true;
            TimerOn = true;
            timeLeft -= Time.deltaTime;
            UpdateTimeText();
            var normalizedValue = Mathf.Clamp(timeLeft / time, 0.0f, 1.0f);
            timerImage.fillAmount = normalizedValue;
            yield return null;
        }

        if (timeLeft <= 0)
        {
            TimerOn = false;
            gameObject.GetComponent<Image>().enabled = false;
        }

    }
    private void UpdateTimeText()
    {
        if (timeLeft < 0)
            timeLeft = 0;
        
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void StartTimer(int second)
    {
        if (timeLeft <= 0)
        {
            timeLeft = second;
            StartCoroutine(CoroutineForTimer());
        }
    }
}

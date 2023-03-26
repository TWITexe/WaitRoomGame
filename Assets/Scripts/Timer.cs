using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private Image timerImage;
    [SerializeField] private Text timerText;
    private bool TimerOn { get; set; } = false;         // Работает ли сейчас таймер

    private float _timeLeft = 0f;
    private void Start()
    {
        _timeLeft = time;
        StartCoroutine(StartTimer());
    }
    private IEnumerator StartTimer()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            UpdateTimeText();
            var normalizedValue = Mathf.Clamp(_timeLeft / time, 0.0f, 1.0f);
            timerImage.fillAmount = normalizedValue;
            yield return null;
        }
    }
    private void UpdateTimeText()
    {
        if (_timeLeft < 0)
            _timeLeft = 0;
        
        float minutes = Mathf.FloorToInt(_timeLeft / 60);
        float seconds = Mathf.FloorToInt(_timeLeft % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}

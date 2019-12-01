using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public Animator UIAnimator;

    public Text TimeDisplay;
    public Text BearDisplay;

    public float UpdateTimeCounterEveryXSeconds = 0.1f;

    WaitForSeconds _timeUpdateWait;
    Coroutine _timeUpdateCoroutine;
    float _startTime = 0f;
    int _deadBearsCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _timeUpdateWait = new WaitForSeconds(UpdateTimeCounterEveryXSeconds);
        BearController.AnotherBearDied += () => { _deadBearsCount++; BearDisplay.text = _deadBearsCount.ToString(); };
        PlayerController.PlayerDied += () => { StopCoroutine(_timeUpdateCoroutine); UIAnimator.SetTrigger("Exit"); };
    }

    public void StartLevel()
    {
        UIAnimator.SetTrigger("Enter");
        _timeUpdateCoroutine = StartCoroutine(UpdateTimeCounter());
        DefaultCounters();
    }

    public void RestartLevel()
    {
        UIAnimator.SetTrigger("Repeat");
        _timeUpdateCoroutine = StartCoroutine(UpdateTimeCounter());
        DefaultCounters();
    }

    void DefaultCounters()
    {
        _startTime = Time.time;
        _deadBearsCount = 0;
        BearDisplay.text = "0";
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator UpdateTimeCounter()
    {
        while (true)
        {
            TimeDisplay.text = FormatTime(Time.time - _startTime);
            yield return _timeUpdateWait;
        }
    }

    //Not very effective, maybe refactor it later
    string FormatTime(float time)
    {
        int totalms = (int)(time*1000f);
        int ms = totalms % 1000;
        int sec = totalms / 1000;
        int min = 0;

        if(sec > 60)
        {
            min = sec / 60;
            sec = sec % 60;
        }

        return $"{min.ToString("D2")}.{sec.ToString("D2")}:{ms.ToString("D3")}";
    }
}

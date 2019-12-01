using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //============================================================
    //Fields
    public Animator UIAnimator;

    public Text TimeDisplay;
    public Text BearDisplay;

    public float UpdateTimeCounterEveryXSeconds = 0.1f;
    public float WaitXSecondsAfterPlayerDeath = 3f;

    WaitForSeconds _timeUpdateWait;
    Coroutine _timeUpdateCoroutine;
    WaitForSeconds _funeralWait;
    float _startTime = 0f;
    int _deadBearsCount = 0;

    //============================================================
    //Methods
    // Start is called before the first frame update
    void Start()
    {
        _timeUpdateWait = new WaitForSeconds(UpdateTimeCounterEveryXSeconds);
        _funeralWait = new WaitForSeconds(WaitXSecondsAfterPlayerDeath);
        BearController.AnotherBearDied += () => { _deadBearsCount++; BearDisplay.text = _deadBearsCount.ToString(); };
        PlayerController.PlayerDied += () => { StopCoroutine(_timeUpdateCoroutine); StartCoroutine(FadeOutAfterWait()); };
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
            QuitGame();
    }


    public void StartLevel()
    {
        UIAnimator.SetTrigger("Enter");
        ResetCounters();
    }

    public void RestartLevel()
    {
        UIAnimator.SetTrigger("Repeat");
        ResetCounters();
    }

    void ResetCounters()
    {
        _startTime = Time.time;
        _deadBearsCount = 0;
        BearDisplay.text = "0";
        _timeUpdateCoroutine = StartCoroutine(UpdateTimeCounter());
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
        int totalms = (int)(time * 1000f);
        int ms = totalms % 1000;
        int sec = totalms / 1000;
        int min = 0;

        if (sec > 60)
        {
            min = sec / 60;
            sec = sec % 60;
        }

        return $"{min.ToString("D2")}.{sec.ToString("D2")}:{ms.ToString("D3")}";
    }

    IEnumerator FadeOutAfterWait()
    {
        yield return _funeralWait;
        UIAnimator.SetTrigger("Exit");
    }
}
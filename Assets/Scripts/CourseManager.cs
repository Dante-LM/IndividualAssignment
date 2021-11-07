using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CourseManager : MonoBehaviour
{
    [HideInInspector] public int score;
    [HideInInspector] public float timer;
    [HideInInspector] public TextMeshProUGUI finalScoreText;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public GameObject[] allTargets;
    [HideInInspector] public GameObject[] endDoors;
    [HideInInspector] public GameObject endZone;
    [HideInInspector] public bool isPlaying;

    void Start()
    {
        scoreText = GameObject.FindWithTag("ScoreUI").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.FindWithTag("TimerUI").GetComponent<TextMeshProUGUI>();
        finalScoreText = GameObject.FindWithTag("FinalScoreUI").GetComponent<TextMeshProUGUI>();
        finalScoreText.gameObject.SetActive(false);
        endDoors = GameObject.FindGameObjectsWithTag("End Door");
        endZone = GameObject.Find("End Zone");
        
        allTargets = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].SetActive(false);
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
            endDoors[1].SetActive(true);
            if (score == 49)
            {
                endDoors[0].SetActive(false);                
            }
            if (score == 50)
            {
                endDoors[1].SetActive(false);
                isPlaying = false;
                StartCoroutine(FinalScore());
            }
        }
        else if (!isPlaying)
        {
            endDoors[1].SetActive(false);
            timer = 0;
            score = 0;
        }

        scoreText.SetText(string.Format("Score: {0}", score));
        timerText.SetText(string.Format("{0:0.00}", timer));
    }

    public void StartPlaying()
    {
        if (!isPlaying)
        {
            for (int i = 0; i < allTargets.Length; i++)
            {
                allTargets[i].SetActive(true);
            }

            endDoors[0].SetActive(true);
            isPlaying = true;
            timer = 0;
            score = 0;
        }
    }
    public IEnumerator FinalScore()
    {
        finalScoreText.gameObject.SetActive(true);
        finalScoreText.SetText("Final Time: {0:0.00}", timer);
        yield return new WaitForSeconds(5f);
        finalScoreText.gameObject.SetActive(false);
    }
}

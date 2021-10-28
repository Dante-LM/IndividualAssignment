using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector] public int score;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public float timer;
    [HideInInspector] public GameObject[] targets;
    [HideInInspector] private Damageable targetScript;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        timer = 0;
        score = 0;
        scoreText = GameObject.FindWithTag("ScoreUI").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.FindWithTag("TimerUI").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        scoreText.SetText(string.Format("Score: {0}", score));
        timerText.SetText(string.Format("{0:0.00}", timer));

        for (int i = 0; i < targets.Length; i++)
        {
            targetScript = targets[i].GetComponent<Damageable>();
            if (!targets[i].activeSelf && (timer - targetScript.timeDestroyed) > targetScript.respawnTime)
            {
                targetScript.health = targetScript.maxHealth;
                targets[i].SetActive(true);
            }
        }
    }
}

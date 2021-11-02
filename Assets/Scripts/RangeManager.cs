using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    public GameObject target;
    [HideInInspector] public int score;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public float timer;
    [HideInInspector] public float targetTimer;
    [HideInInspector] public GameObject[] allTargets;
    [HideInInspector] private Damageable targetScript;

    void Start()
    {
        GameObject targetParent = GameObject.Find("Targets");
        for (int i = 0; i < 2; i++)
        {
            Instantiate(target, new Vector3(Random.Range(-9, 9), Random.Range(1, 3), Random.Range(7.5f, 27)), new Quaternion(), targetParent.transform);
        }
        targetParent.tag = "Target";
        targetParent.layer = 6;


        allTargets = GameObject.FindGameObjectsWithTag("Target");
        timer = 60;
        score = 0;
        scoreText = GameObject.FindWithTag("ScoreUI").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.FindWithTag("TimerUI").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        targetTimer += Time.deltaTime;
        scoreText.SetText(string.Format("Score: {0}", score));
        timerText.SetText(string.Format("{0:0.00}", timer));

        for (int i = 0; i < allTargets.Length; i++)
        {
            targetScript = allTargets[i].GetComponent<Damageable>();
            if (!allTargets[i].activeSelf && (targetTimer - targetScript.timeDestroyed) > targetScript.respawnTime)
            {
                Debug.Log((targetTimer - targetScript.timeDestroyed) > targetScript.respawnTime);
                targetScript.health = targetScript.maxHealth;
                allTargets[i].SetActive(true);
            }
        }
    }
}

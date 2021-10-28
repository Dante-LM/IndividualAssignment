using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector] public int score;
    [HideInInspector] public TextMeshProUGUI scoreText;

    void Start()
    {
        score = 0;
        scoreText = GameObject.FindWithTag("ScoreUI").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        scoreText.SetText(string.Format("Score: {0}", score));
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CourseManager : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI finalScoreText;

    void Start()
    {
        finalScoreText = GameObject.FindWithTag("FinalScoreUI").GetComponent<TextMeshProUGUI>();
        finalScoreText.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}

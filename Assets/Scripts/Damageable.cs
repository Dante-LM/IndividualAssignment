using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Header("Damage Effect")]
    [SerializeField] GameObject hitEffect;

    [Header("Properties")]
    [SerializeField] public int maxHealth;
    [SerializeField] public int respawnTime;
    [HideInInspector] public int health;
    private GameObject currentSceneManager;
    private RangeManager rangeManager;
    private CourseManager courseManager;
    [HideInInspector] public float timeDestroyed;

    void Start()
    {
        if (maxHealth == 0)
            maxHealth = 1;
        else
            health = maxHealth;

        if (this.gameObject.tag == "Target")
        {
            currentSceneManager = GameObject.FindGameObjectWithTag("Button");

            if (currentSceneManager.GetComponentInChildren<RangeManager>() != null)
            {
                rangeManager = currentSceneManager.GetComponentInChildren<RangeManager>();
            }
            else if (currentSceneManager.GetComponentInChildren<CourseManager>() != null)
            {
                courseManager = currentSceneManager.GetComponentInChildren<CourseManager>();
            }
            else
                Debug.Log("No button");
        }        
    }

    public void TakeDamage(Vector3 hitPos, Vector3 hitNormal)
    {        
        Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
        health--;
    }

    void Update()
    {
        DestroyTarget();
    }

    void DestroyTarget()
    {
        if (health <= 0 && this.gameObject.tag == "Target")
        {
            this.gameObject.SetActive(false);
            if(rangeManager != null)
            {
                rangeManager.score++;
                timeDestroyed = rangeManager.targetTimer;
            }
        }
    }
}

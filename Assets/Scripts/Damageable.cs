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
    [HideInInspector] public float timeDestroyed;

    void Start()
    {
        if (maxHealth == 0)
            maxHealth = 6;
        else
            health = maxHealth;

        if (this.gameObject.tag == "Target")
        {
            currentSceneManager = GameObject.FindGameObjectWithTag("Button");
        }

        if (currentSceneManager.GetComponent<RangeManager>() != null)
        {
            
        }
        else
            Debug.Log("No button");
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
            //Destroy(this.gameObject);
            gmScript.score++;
            timeDestroyed = gmScript.targetTimer;
        }
    }
}

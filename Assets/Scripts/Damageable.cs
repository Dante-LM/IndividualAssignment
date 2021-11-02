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
    [HideInInspector] RangeManager gmScript;
    [HideInInspector] public float timeDestroyed;

    void Start()
    {
        health = maxHealth;
        if(this.gameObject.tag == "Target")
        {
            gmScript = GameObject.FindWithTag("GameManager").GetComponent<RangeManager>();
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
            //Destroy(this.gameObject);
            gmScript.score++;
            timeDestroyed = gmScript.timer;
        }
    }
}

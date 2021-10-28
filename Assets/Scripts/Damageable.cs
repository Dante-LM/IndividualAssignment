using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] int health;
    [HideInInspector] GameManagerScript gmScript;

    void Start()
    {
        if(this.gameObject.tag == "Target")
        {
            gmScript = GameObject.FindWithTag("GameManager").GetComponent<GameManagerScript>();
        }
    }

    public void TakeDamage(Vector3 hitPos, Vector3 hitNormal)
    {        
        Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
        health--;
    }

    void Update()
    {
        if (health <= 0 && this.gameObject.tag == "Target")
        {
            Destroy(this.gameObject);
            gmScript.score++;
        }
    }
}

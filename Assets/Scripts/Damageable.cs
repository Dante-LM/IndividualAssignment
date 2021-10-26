using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] int health;
    void Start()
    {
        
    }

    public void TakeDamage(Vector3 hitPos, Vector3 hitNormal)
    {        
        Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
        health--;        
    }

    void Update()
    {
        if (health == 0)
            Destroy(this.gameObject);
    }
}

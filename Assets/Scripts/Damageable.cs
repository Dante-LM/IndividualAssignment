using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    void Start()
    {
        
    }

    public void TakeDamage(Vector3 hitPos, Vector3 hitNormal)
    {
        Instantiate(hitEffect, hitPos, Quaternion.LookRotation(hitNormal));
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracer : MonoBehaviour
{
    public float movementSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
        Destroy(this.gameObject, 1f);
    }
}

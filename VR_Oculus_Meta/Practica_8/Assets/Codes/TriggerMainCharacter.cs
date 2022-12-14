using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMainCharacter : MonoBehaviour
{

    public Collider mainCharacter;
    public bool collision = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (mainCharacter != null && other == mainCharacter)
            collision = true;

    }

    private void OnTriggerExit(Collider other)
    {

        if (mainCharacter != null && other == mainCharacter)
            collision = false;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "bala")
            collision = true;
    }
}

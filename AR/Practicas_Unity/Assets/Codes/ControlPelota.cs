using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPelota : MonoBehaviour
{

    //public GameObject meta;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Meta")
        {
           print("Se ha chocado");
           Object.Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

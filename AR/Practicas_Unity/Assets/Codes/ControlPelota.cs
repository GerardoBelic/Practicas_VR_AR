using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPelota : MonoBehaviour
{

    //public GameObject meta;
    public bool ganar = false;
    public bool perder = false;

    private void OnTriggerEnter(Collider collision)
    {
        /*if (collision.collider.name == "Meta")
        {
           print("Se ha chocado");
           Object.Destroy(this.gameObject);
        }*/
        if (collision.GetComponent<Collider>().name == "muerte")
        {
            perder = true;
        }
        else if (collision.GetComponent<Collider>().name == "meta")
        {
            ganar = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.name == "Meta")
        {
           print("Se ha chocado");
           Object.Destroy(this.gameObject);
        }*/
        if (collision.collider.name == "muerte")
        {
            perder = true;
        }
        else if (collision.collider.name == "meta")
        {
            ganar = true;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -25.0)
        {
            perder = true;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNivel : MonoBehaviour
{

    public GameObject pelotaActiva;
    public GameObject ReiniciaBoton;
    
    public Transform inicioPelota;
    public GameObject pelota;

    public void OnTriggerEnter(Collider other)
    {
        print("Ganaste");
    }

    // Start is called before the first frame update
    void Start()
    {
       //Instantiate(pelota, inicioPelota);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
            //Instantiate(pelota, inicioPelota);
        if (pelotaActiva == null)
            perdio();
    }

    public void reinicia()
    {
        pelotaActiva = Instantiate(pelota, inicioPelota);

        ReiniciaBoton.SetActive(false);
    }

    public void perdio()
    {
        ReiniciaBoton.SetActive(true);
    }
}

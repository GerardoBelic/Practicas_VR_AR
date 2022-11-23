using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNivel : MonoBehaviour
{
    
    public Transform inicioPelota;
    public GameObject pelota;

    public bool ganar;
    public bool perder;

    void Start()
    {
       pelota = Instantiate(pelota, inicioPelota);
    }

    void Update()
    {
        ControlPelota statusPelota = pelota.GetComponent<ControlPelota>();

        if (statusPelota != null)
        {
            perder = statusPelota.perder;
            ganar = statusPelota.ganar;
        }
    }
}

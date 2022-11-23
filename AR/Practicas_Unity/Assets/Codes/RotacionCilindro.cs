using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionCilindro : MonoBehaviour
{

    public float grados = 10.0f;

    void Update()
    {
        transform.Rotate(0, 0, grados * Time.deltaTime);
    }
}

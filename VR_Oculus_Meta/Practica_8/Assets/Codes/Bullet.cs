using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float startTime;
    public float bulletLifeSpan = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (startTime + bulletLifeSpan < Time.time)
            Destroy(gameObject);

    }
}

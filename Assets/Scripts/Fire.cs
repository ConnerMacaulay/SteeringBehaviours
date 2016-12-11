using UnityEngine;
using System.Collections;
using System;

public class Fire : MonoBehaviour {

    public float rof = 2.0f;
    public GameObject bullet;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        rof -= Time.deltaTime;
        if (rof <= 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            rof = 1.5f;
        }
    }

   
}

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int bulletSpeed = 5;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.up * bulletSpeed);
    }
}

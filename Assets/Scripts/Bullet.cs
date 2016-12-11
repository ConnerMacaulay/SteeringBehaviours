using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public int bulletSpeed = 5;
    public float bulletGone = 1.5f;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.up * bulletSpeed);
        bulletGone -= Time.deltaTime;
        if (bulletGone <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
         
        }
        else if (col.gameObject.tag =="Agent")
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossBowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(10);

        }
        Destroy(gameObject);

    }
    // Update is called once per frame
    void Update () {
		
	}
}

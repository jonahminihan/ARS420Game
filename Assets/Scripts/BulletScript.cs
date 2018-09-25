using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public void Death(){
        //gameObject.GetComponentInChildren<Explosion>();
        GameObject explos = this.gameObject.transform.GetChild(0).gameObject;
        explos.GetComponent<MeshRenderer>().enabled = true;
        explos.GetComponent<SphereCollider>().enabled = true;

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
}

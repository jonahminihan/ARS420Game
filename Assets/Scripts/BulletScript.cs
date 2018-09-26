using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletScript : NetworkBehaviour {

    public GameObject bulletExplosPrefab;

    public void Death(){
        //gameObject.GetComponentInChildren<Explosion>();
        GameObject explos = this.gameObject.transform.GetChild(0).gameObject;
        explos.GetComponent<MeshRenderer>().enabled = true;
        explos.GetComponent<SphereCollider>().enabled = true;

    }

    private void OnDestroy()
    {
        /*Vector3 bulletPlace;
        Quaternion bulletRot = new Quaternion();
        GameObject bullet = this.gameObject;
        bulletPlace.x = bullet.transform.position.x;
        bulletPlace.y = bullet.transform.position.y;
        bulletPlace.z = bullet.transform.position.z;
        bulletRot.x = bullet.transform.rotation.x;
        bulletRot.y = bullet.transform.rotation.y;
        bulletRot.z = bullet.transform.rotation.z;

        //Destroy(bullet, 2.0f);
        var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletPlace, bulletRot);
       NetworkServer.Spawn(bulletExplos);
       */
        CmdExplosionFunc();
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
    //[Command]
    void CmdExplosionFunc(){
        Vector3 bulletPlace = Vector3.zero;
        Quaternion bulletRot = new Quaternion();
        GameObject bullet = this.gameObject;
        bulletPlace.x = bullet.transform.position.x;
        bulletPlace.y = bullet.transform.position.y;
        bulletPlace.z = bullet.transform.position.z;
        bulletRot.x = bullet.transform.rotation.x;
        bulletRot.y = bullet.transform.rotation.y;
        bulletRot.z = bullet.transform.rotation.z;

        //Destroy(bullet, 2.0f);
        var bulletExplos = (GameObject)Instantiate(bulletExplosPrefab, bulletPlace, bulletRot);
        //NetworkServer.Spawn(bulletExplos);
        Destroy(bulletExplos, 0.2f);
        //NetworkServer.Spawn(bulletExplosPrefab);
    }
}

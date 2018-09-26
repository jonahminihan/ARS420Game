using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplosScript : MonoBehaviour
{

    public GameObject bulletExplosPrefab;


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
        //ExplosionFunc();
    }
    private void Update()
    {
        //Destroy(gameObject, 0.3f);
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
    //void ExplosionFunc()
    //{
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
        Destroy(bulletExplos, 0.3f);
        //NetworkServer.Spawn(bulletExplosPrefab);
        */
    //}
}

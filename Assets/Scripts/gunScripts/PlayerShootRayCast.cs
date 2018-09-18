using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// This is not finished
/// </summary>
public class PlayerShootRayCast : NetworkBehaviour{


    public PlayerWeaponRayCast weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot : No camera Referenced!");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            //we hit something
            Debug.Log("We hit " + _hit.collider.name);
        }
    }
}

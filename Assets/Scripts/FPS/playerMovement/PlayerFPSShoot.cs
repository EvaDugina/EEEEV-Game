using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPSShoot : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitInfo;

    public GameObject bullet;

    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Camera cam;
    private float weoponRange = 50f;

    private float weoponForcePower = 10f;

    private float weoponDamage = 10f;
    // private float distance = 3f;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerFPSLook>().cam;
        inputManager = GetComponent<InputManager>();

    }

    float pokeForce;

    void Update()
    {

        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward * weoponRange);
        Debug.DrawRay(ray.origin, ray.direction * weoponRange, Color.red);
        if (Physics.Raycast(ray, out hit))
        {
            if (inputManager.onFoot.Weapon.triggered)
            {
                GameObject clone;
                clone = Instantiate(bullet, hit.point, transform.rotation);
                hit.rigidbody.AddForceAtPosition(ray.direction * weoponForcePower, hit.point);
                Destroy(clone, 5);
            }

        }

    }

}

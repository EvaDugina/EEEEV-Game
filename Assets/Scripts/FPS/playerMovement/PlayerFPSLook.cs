using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPSLook : MonoBehaviour
{
    public Camera cam;
    public GameObject hand;
    private float xRotation = 0f;

    public float xSensivity = 30f;
    public float ySensivity = 30f;


    public void ProccesLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        hand.transform.localRotation = Quaternion.Euler(Mathf.Clamp(xRotation, -7f, 7f), 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensivity);
    }
}

/*
 * written by Joseph Hocking 2017
 * released under MIT license
 * text of license https://opensource.org/licenses/MIT
 */

using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(CharacterController))]

// basic WASD-style movement control
public class FpsMovement3D : MonoBehaviour
{
    [SerializeField] private Camera HeadCamera;
    [SerializeField] private Transform ArrowTransform;

    public float speed = 6.0f;
    public float gravity = -9.8f;

    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float rotationVert = 0;

    private CharacterController charController;

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveCharacter();
        RotateCharacter();
        RotateCamera();
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        charController.Move(movement);
    }

    private void RotateCharacter()
    {
        float xAxis = Input.GetAxis("Mouse X");
        transform.Rotate(0, xAxis * sensitivityHor, 0);
        ArrowTransform.Rotate(0, xAxis * sensitivityHor, 0);

    }

    private void RotateCamera()
    {
        rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
        rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);

        HeadCamera.transform.localEulerAngles = new Vector3(
            rotationVert, HeadCamera.transform.localEulerAngles.y, 0
        );
    }
}

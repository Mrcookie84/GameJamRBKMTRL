using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float lookXLimit = 85.0f;
    
    public Transform playerBody; 

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
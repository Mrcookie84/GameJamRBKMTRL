using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float lookXLimit = 85.0f;

    // IMPORTANT : Glisse ton objet "Capsule" (le parent) ici dans l'Inspector
    public Transform playerBody; 

    private float rotationX = 0f;

    void Start()
    {
        // On verrouille le curseur pour ne pas qu'il sorte du jeu
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 1. ROTATION VERTICALE (On fait tourner la caméra sur elle-même)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        
        // On applique la rotation UNIQUEMENT à la caméra (cet objet)
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // 2. ROTATION HORIZONTALE (On fait tourner tout le corps du joueur)
        // On ne change que l'axe Y (Vector3.up), donc le perso reste bien droit !
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
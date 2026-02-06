using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTutorial : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float groundDrag = 5f;
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    bool readyToJump;

    [Header("Scaling Settings")]
    public float scaleSpeed = 0.8f;
    public float minScale = 0.5f;
    public float maxScale = 5f;
    private bool scaleLock = false;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    private Collider playerCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        HandleScaling();
        UpdateFriction();
        
        rb.linearDamping = grounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(KeyCode.L))
        {
            if (scaleLock)
            {
                scaleLock = false;
            }
            else
            {
                scaleLock = true;
            }
        }
    }

    private void UpdateFriction()
    {
        if (grounded)
        {
            playerCollider.material.dynamicFriction = 0.6f;
            playerCollider.material.staticFriction = 0.6f;
            playerCollider.material.frictionCombine = PhysicsMaterialCombine.Average;
        }
        else
        {
            playerCollider.material.dynamicFriction = 0f;
            playerCollider.material.staticFriction = 0f;
            playerCollider.material.frictionCombine = PhysicsMaterialCombine.Minimum;
        }
    }

    private void HandleScaling()
    {
        if (scaleLock)
        {
            return;    
        }
        if (verticalInput != 0)
        {
            float scaleChange = (verticalInput * scaleSpeed * Time.deltaTime) * -1.0f;
            Vector3 newScale = transform.localScale + Vector3.one * scaleChange;

            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            transform.localScale = newScale;
        }
        
        if (horizontalInput != 0)
        {
            float scaleChange = horizontalInput * scaleSpeed * Time.deltaTime;
            Vector3 newScale = transform.localScale + Vector3.one * scaleChange;
            
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            transform.localScale = newScale;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce * transform.localScale.y, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y > 0.6f)
                {
                    grounded = true;
                    return;
                }
            }
            grounded = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            grounded = false;
        }
    }
}
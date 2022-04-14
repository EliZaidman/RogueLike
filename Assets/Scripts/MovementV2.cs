using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class MovementV2 : MonoBehaviour
{
    // Move player in  space
    public float maxSpeed = 1f;
    public float jumpHeight = 6.5f;
    public float _depth;
    public float rayLenugh;
    public Camera mainCamera;

    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    bool isGroundedDown = false;
    bool isJumped = false;
    Vector3 cameraPos;
    Rigidbody rb;
    CapsuleCollider mainCollider;
    Transform t;

    public GameObject raypos;
    // Use this for initialization
    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
        mainCollider = GetComponent<CapsuleCollider>();
        rb.freezeRotation = true;
        //rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //rb.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        }
        else
        {
            moveDirection = 0;
        }

        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection >= 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection <= 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        }

        // Jumping

        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S) && isGrounded)
        {
            StartCoroutine(GoDownPlatform());
        }

         if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (mainCollider.enabled)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight);
                isJumped = true;
            }

        }

        else if (Input.GetKeyDown(KeyCode.Space) && isJumped)
        {
            if (mainCollider.enabled)
            {
                isJumped = false;
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight);
            }
        }
        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }

    void FixedUpdate()
    {

        CheckPlatformUnder();
        GroundCheck(_depth);
        // Apply movement velocity
        rb.velocity = new Vector3((moveDirection) * maxSpeed, rb.velocity.y);
    }

    private void GroundCheck(float Depth)
    {

        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.bounds.size.x * Depth * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.2f, colliderRadius * 0.5f, 0);
        // Check if player is grounded
        Collider[] colliders = Physics.OverlapSphere(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Simple debug

        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);


    }

    private void CheckPlatformUnder()
    {

        if (isGrounded)
        {
            if (Physics.Raycast(raypos.transform.position, Vector3.down, 5))
            {

                isGroundedDown = true;
                Debug.Log("Can Go Down");
            }
            else
            {
                isGroundedDown = false;
                Debug.Log("You Cannot Go Down");
            }

        }
    }

    private IEnumerator GoDownPlatform()
    {
        Debug.Log("going down");
        if (isGroundedDown)
        {
            mainCollider.enabled = false;
            yield return new WaitForSeconds(1f);
            mainCollider.enabled = true;
        }
    }
}

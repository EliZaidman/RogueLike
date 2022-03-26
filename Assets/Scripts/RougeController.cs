using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RougeController : MonoBehaviour
{
    #region Variables
    //Variables for Player
    [HideInInspector] public Rigidbody rb;
    public bool canGoDeep = false;
    public float moveSpeed;
    public int jumpSpeed;

    // IsGrounded
    public MeshRenderer renda;
    public float Height;
    bool IsGrounded;

    // Classes
    DepthChecker depthChecker;
    public CameraFollow2D cameraFollow;
    #endregion
    private void Start()
    {
        // Lock rotation of rb - 2.5D Effect
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;

        // IsGrounded
        //renda = gameObject.GetComponent<MeshRenderer>();
        //Height = renda.bounds.size.y;
    }
    
    private void Update()
    {
        //Checking For Grounded Every Second
        ChackIfGrounded();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (cameraFollow.offset == new Vector3(90, cameraFollow.currentY.y, cameraFollow.currentZ.z))
        {
            Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal);
            rb.AddForce(movement * moveSpeed);
            Debug.Log("90deg");
        }
        else
        {
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * moveSpeed);
            Debug.Log("not 90dig");
        }
        

        if (IsGrounded /*&& depthChecker*/)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
            }
        }
    }

    private void ChackIfGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, Height))
        {
            IsGrounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            IsGrounded = false;
            Debug.Log("Not Grounded!");
        }
    }
}
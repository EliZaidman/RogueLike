using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RougeController : MonoBehaviour
{
    #region Variables
    //Variables for Player
    public bool canGoDeep = false;
    [HideInInspector]
    public Rigidbody rb;

    public float moveSpeed;
    public int jumpSpeed;
    private Vector3 moveDir;  
    public float rotateSpeed = 200f;
    private float rotate;
    // IsGrounded
    public MeshRenderer renda;
    public float Height;
    bool IsGrounded;
    Ray ray;


    // Classes
    DepthChecker depthChecker;
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
        //rb.MovePosition(transform.position + moveDir);
        //transform.Rotate(0, rotate, 0);

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * moveSpeed);

        if (IsGrounded)
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
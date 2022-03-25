using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SwingController : MonoBehaviour
{
    #region Variables
    //Variables for Player

    private Rigidbody rb;
    public float moveSpeed;
    public int jumpSpeed;

    // Variables for effects 
    public AudioSource connectSound;
    public AudioSource releaseSound;
    private bool soundplayed;


    // IsGrounded
    public MeshRenderer renda;
    public float Height;
    bool IsGrounded;
    Ray ray;

    #endregion
    private void Start()
    {
        // Lock rotation of rb - 2.5D Effect
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;

        // Sound
        //connectSound = GetComponent<AudioSource>();
        //soundplayed = false;

        // IsGrounded
        //renda = gameObject.GetComponent<MeshRenderer>();
        //Height = renda.bounds.size.y;
    }
    void FixedUpdate()
    {
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
    private void Update()
    {
        //Checking For Grounded Every Second
        ChackIfGrounded();
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
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RougeController : MonoBehaviour
{
    #region Variables

    //Enum
    public enum CurrnetView
    {
        FrontView = 6,
        SideView = 7,
        BackView = 8,
    }
    //Variables for Player
    [HideInInspector] public Rigidbody rb;
    public bool canGoDeep;
    public float moveSpeed;
    public int jumpSpeed;

    public GameObject goto2;
    public Quaternion wantedPos = Quaternion.Euler(0, 90, 0);
    public Quaternion wantedPos2 = Quaternion.Euler(0, 0, 0);
    public Quaternion rotation_;
    public float speed = 2f;

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

        rotation_ = transform.rotation;
        // IsGrounded
        //renda = gameObject.GetComponent<MeshRenderer>();
        //Height = renda.bounds.size.y;
    }

    private void Update()
    {
        //Checking For Grounded Every Second
        ChackIfGrounded();


        if (Input.GetKey(KeyCode.E))
        {
            float push = gameObject.transform.position.z + 20;
             gameObject.transform.Translate(0.2f, Time.deltaTime, 0, Space.World);
            // StartCoroutine(RotateOverTime(transform.rotation, wantedPos, 2));
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            StartCoroutine(RotateOverTime(transform.rotation, wantedPos2, 2));
        }

    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        if (gameObject.layer == (int)CurrnetView.SideView)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            Vector3 movement = new Vector3(moveVertical, 0.0f, moveHorizontal);
            rb.AddForce(movement * moveSpeed);
            Debug.Log("SideView");
        }
        else
        {
            if (!canGoDeep)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            }

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * moveSpeed);
            Debug.Log("FrontView");
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


    IEnumerator RotateOverTime(Quaternion start, Quaternion end, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            transform.rotation = Quaternion.Slerp(start, end, t / dur);
            yield return null;
            t += Time.deltaTime;
        }
        transform.rotation = end;
    }

}
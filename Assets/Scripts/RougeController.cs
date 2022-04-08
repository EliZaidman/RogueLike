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

    public GameObject bullet;
    public GameObject currentBall;
    public Transform bulletResPos;
    public GameObject sprite;
    public int bulletVelocity;

    [HideInInspector]  public Quaternion sideView = Quaternion.Euler(0, 270, 0);
    [HideInInspector]  public Quaternion frontView = Quaternion.Euler(0, 0, 0);
    public float speed = 2f;

    // IsGrounded
    public float Height;
    public bool IsGrounded;
    bool isPressed;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentBall = Instantiate(bullet, bulletResPos.position, bulletResPos.rotation);
            currentBall.GetComponent<Rigidbody>().AddRelativeForce(new Vector3
                                                     (bulletVelocity, 0, 0));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            sprite.transform.rotation = new Quaternion(0, 0, 0,0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            sprite.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    rb.isKinematic = true;
        //    isPressed = true;
        //    if (gameObject.layer == (int)CurrnetView.FrontView)
        //    {
        //        StartCoroutine(RotateOverTime(transform.rotation, sideView, 2));
        //    }
        //    else
        //    {

        //        StartCoroutine(RotateOverTime(transform.rotation, frontView, 2));
        //    }
        //}
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (IsGrounded /*&& depthChecker*/)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
            }
        }


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
            if (gameObject.layer == (int)CurrnetView.FrontView)
            {
                gameObject.transform.Translate(0, 0, 0.3f / 15, Space.World);
                //yield return new WaitForSeconds(2f);
                //gameObject.layer = (int)CurrnetView.SideView;
            }
            else
            {
                gameObject.transform.Translate(0, 0, -0.02f, Space.World);
                //yield return new WaitForSeconds(2f);
                //gameObject.layer = (int)CurrnetView.FrontView;
            }




            yield return null;
            t += Time.deltaTime;
        }
        transform.rotation = end;
        if (gameObject.layer == (int)CurrnetView.SideView)
        {
            gameObject.layer = (int)CurrnetView.FrontView;
        }
        else
        {
            gameObject.layer = (int)CurrnetView.SideView;
        }
        yield return new WaitForSeconds(0.3f);
        rb.isKinematic = false;
        isPressed = false;
    }

}
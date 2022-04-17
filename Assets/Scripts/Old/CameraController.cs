using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public GameObject player;
    RougeController con;
    public int left;
    public int right;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.layer == 7)
        {
        _camera.cullingMask = left << right;
        }
        else
        {
            _camera.cullingMask = -1 << right;
        }
    }
}

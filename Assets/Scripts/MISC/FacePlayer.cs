using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform player;
    public Transform Cam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Cam.forward);
    }
}


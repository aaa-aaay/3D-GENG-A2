using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbCheck : MonoBehaviour
{
    public GameObject touchingObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject != gameObject)
        {
            touchingObject = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        touchingObject = null;
    }
}

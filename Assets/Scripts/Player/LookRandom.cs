using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class LookRandom : MonoBehaviour
{
    [SerializeField] private Transform aimTargetTransform;
    [SerializeField] private MultiAimConstraint aimConstraint;
    [SerializeField] private SwitchCamera lockCamera;
    private Vector3 targetPosition;
    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = origin = aimTargetTransform.localPosition;
        

        StartCoroutine(ChangeTargetPosition());
    }
    // Update is called once per frame
    void Update()
    {

        if (lockCamera.currentTarget != null)
        {
            WeightedTransformArray sources = aimConstraint.data.sourceObjects;
            sources.SetTransform(0, lockCamera.currentTarget);
            aimConstraint.data.sourceObjects = sources;
        }


        if (aimTargetTransform.localPosition != targetPosition)
        {
            float speed = 2; 
            aimTargetTransform.localPosition = Vector3.Lerp(aimTargetTransform.localPosition, targetPosition, Time.deltaTime * speed);
        }
    }

    IEnumerator ChangeTargetPosition()
    {

        yield return new WaitForSeconds(3);
        float x = Random.Range(-5, 5);
        float y = Random.Range(-1, 1);
        float z = Random.Range(-2, -5);


        targetPosition = origin + new Vector3(x, y, z);
        //Loop
        StartCoroutine(ChangeTargetPosition());
    }
}

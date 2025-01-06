using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class LookRandom : MonoBehaviour
{
    [SerializeField] private Transform aimTargetTransform;
    private Vector3 targetPosition;
    private Vector3 origin;

    void Start()
    {
        targetPosition = origin = aimTargetTransform.localPosition;
        StartCoroutine(ChangeTargetPosition());
    }

    void Update()
    {
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
        StartCoroutine(ChangeTargetPosition());
    }
}

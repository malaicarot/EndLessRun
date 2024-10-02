using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float followSpeed = 5.0f;


    Vector3 offset;

    void Start()
    {
        offset = new Vector3(0, 4, -10);
    }
    void LateUpdate()
    {
        if (target != null) {
            Vector3 rotatedOffset = target.TransformDirection(offset);
            
            Vector3 desiredPosition = target.position + rotatedOffset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

        }
    }

    public void RotateCamera(int turn, Action onFinish){
        
        Vector3 rotateTarget = new Vector3(0, 90 * turn, 0) + transform.eulerAngles;

        transform.DORotate(rotateTarget, 0.8f).SetEase(Ease.OutSine).OnComplete(() => {
            onFinish.Invoke();
        });

        /*DORotate co tham so 'duration' tao do tre den target*/
    }
}


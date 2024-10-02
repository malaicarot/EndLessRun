using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FowardMove : MonoBehaviour
{
    private Animator animator;
    private bool isMarkPoint = false;

    [SerializeField] private float speed = 5f;

    // isDisapointed
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(!isMarkPoint){
        //     transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // }


        
    }
}

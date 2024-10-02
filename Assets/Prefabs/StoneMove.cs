using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMove : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(gameObject, 4f);
    }

    void OnTriggerEnter(Collider other){

        if(other.gameObject.CompareTag("Obstacle")){
            if(gameObject.CompareTag("Stone")){
                Destroy(other.gameObject);
            }
        }

    }

}

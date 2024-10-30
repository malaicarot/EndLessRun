using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{

    public GameObject RotateTriggerBox;

    private Mover player;
    [SerializeField] float distanceDelete = 20f;

    public Action trapAction;

    void Start(){
        player = FindObjectOfType<Mover>();

    }


    void Update()
    {
        if(player.transform.position.z > transform.position.z + distanceDelete){
            Destroy(gameObject);
        }

    }

    public void ActiceRotateTriggerBox(bool isActive)
    {
        RotateTriggerBox.SetActive(isActive);

    }

    public void Init(Action _trapAction){

        trapAction = _trapAction;
        
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            trapAction?.Invoke();
        }
    }
}

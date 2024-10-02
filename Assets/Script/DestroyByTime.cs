using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color objectColor;
    [SerializeField] private float lifeTime;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectColor = objectRenderer.material.color;

    }

    void Update(){
        float alpha = Mathf.Lerp(1f, 0f, Time.time / lifeTime);
        objectColor.a = alpha;
        objectRenderer.material.color = objectColor;

        if(alpha <= 0 ){
            Destroy(gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject StartPoint;



    [SerializeField] private Transform player;

    private float SpawnCoinsInterval = 2f;



    private float itemPerRows = 5f;

    private float rowSpacing = 20f;

    private float itemSpacing = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnCoinsInterval)
        {
            SpawnStart();
            timer = 0f;
        }

    }

    void SpawnStart()
    {
        Vector3 spawnPosition = player.position + player.forward * rowSpacing;
        
        spawnPosition.y += 2f;

        for(int i = 0; i < itemPerRows; i++){
            GameObject coins = Instantiate(StartPoint, spawnPosition, Quaternion.Euler(90, 0, 0));
            spawnPosition += player.forward  * itemSpacing;
        }
        transform.position += new Vector3(0f, 0f, rowSpacing);
    }

}

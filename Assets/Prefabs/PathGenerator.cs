using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> PlaneList;
    [SerializeField] private List<GameObject> WallList;

    [SerializeField] private GameObject EnterBanner;

    [SerializeField] private GameObject Destination;

    [SerializeField] public GameObject LavaPrefabs;
    [SerializeField] public GameObject ObstaclePrefabs;
    [SerializeField] public GameObject StonePrefabs;

    public bool isNeedSpawnTrap = false;
    [SerializeField] public GameObject CoinsPrefabs;

    private int coinsPerRows = 5;


    private float coinsSpacing = 2f;

    public bool spawnTrap = false;

    [SerializeField] private List<GameObject> ItemsList;
    private List<GameObject> ItemsSpawn = new List<GameObject>(100);
    [SerializeField] private float[] spawnRates;

    private Vector3 nextSpawnPoint;

    private Vector3 lavaSpawnPoint;


    private Quaternion nextRotation = Quaternion.identity;
    private Quaternion currentRotation = Quaternion.identity;

    [SerializeField] int SEGMENT_QUANTITY = 20;
    private const int SEGMENT_SQUARE_SIZE = 10;
    private int segmentBeforeTurn = 5;

    [SerializeField] int numberOfLevels = 3;
    private bool isNewLevel = false;
    private bool isDestination = false;

    private int currentTotalRotation = 0;
    private List<GameObject> LeftWall = new List<GameObject>(5);
    private List<GameObject> RightWall = new List<GameObject>(5);

    private Mover player;

    void Start()
    {
        player = FindObjectOfType<Mover>();
        Spawner();
    }

    void Spawner()
    {
        for (int j = 0; j < numberOfLevels; j++)
        {
            isNewLevel = false;
            player.speed += 10;
            for (int i = 0; i < SEGMENT_QUANTITY; i++)
            {
                if (i > 2)
                {
                    isNeedSpawnTrap = true;
                }
                if (i + 1 == SEGMENT_QUANTITY)
                {
                    isNewLevel = true;
                }
                if(j + 1 == numberOfLevels && i + 1 == SEGMENT_QUANTITY){
                    isDestination = true;
                    isNewLevel = false;

                }
                SegmentSpawn(i + 1, i, PlaneList[j], WallList[j], isNeedSpawnTrap, isNewLevel, isDestination);
                ItemsSpawn[i].SetActive(false);
                ActiveItem();
            }
        }
        ItemsSpawn.Clear();
    }

    public void SegmentSpawn(int segmentCount, int needSpawnObstacle, GameObject planeType, GameObject wallType, bool isSpawn, bool newLevel, bool _isDestination)
    {
        GameObject newSegment = Instantiate(planeType, nextSpawnPoint, nextRotation);
        PlaneController SegmentController = newSegment.GetComponent<PlaneController>();
        if (newLevel)
        {
            Instantiate(EnterBanner, nextSpawnPoint, nextRotation);
        }
        if(_isDestination){
            GameObject destination = Instantiate(Destination, nextSpawnPoint, Quaternion.identity);
        }


        SpawnItems(nextSpawnPoint, nextRotation);
        SpawnCoins(nextSpawnPoint, nextRotation);
        if (isSpawn)
        {
            SpawnTrap(nextSpawnPoint, nextRotation);
        }

        if (needSpawnObstacle >= 1)
        {
            float randomSpawn = Random.Range(0, 11);
            if (randomSpawn == 3)
            {
                Instantiate(LavaPrefabs, lavaSpawnPoint, nextRotation);
            }
        }

        int rotationAngle = RandomTurnWithConstraint();

        if (nextRotation.eulerAngles.y == 270)
        {

            GameObject leftWall = Instantiate(wallType, nextSpawnPoint + new Vector3(0f, 0.55f, -4.52f), nextRotation);
            GameObject rightWall = Instantiate(wallType, nextSpawnPoint + new Vector3(0f, 0.55f, 4.52f), nextRotation);

            LeftWall.Add(leftWall);
            RightWall.Add(rightWall);


        }
        else if (nextRotation.eulerAngles.y == 90)
        {

            GameObject leftWall = Instantiate(wallType, nextSpawnPoint + new Vector3(0f, 0.55f, 4.52f), nextRotation);
            GameObject rightWall = Instantiate(wallType, nextSpawnPoint + new Vector3(0f, 0.55f, -4.52f), nextRotation);

            RightWall.Add(rightWall);
            LeftWall.Add(leftWall);


        }
        else if (nextRotation.eulerAngles.y == 0)
        {
            GameObject leftWall = Instantiate(wallType, nextSpawnPoint + new Vector3(-4.52f, 0.55f, 0f), nextRotation);
            GameObject rightWall = Instantiate(wallType, nextSpawnPoint + new Vector3(4.52f, 0.55f, 0f), nextRotation);

            LeftWall.Add(leftWall);
            RightWall.Add(rightWall);
        }

        if (segmentCount % segmentBeforeTurn == 0)
        {
            if (rotationAngle == -90)
            {
                LeftWall.Clear();
                RightWall.Clear();

            }
            else if (rotationAngle == 90)
            {
                RightWall.Clear();
                LeftWall.Clear();
            }
            currentRotation = newSegment.transform.rotation;
            nextRotation *= Quaternion.Euler(0, rotationAngle, 0);
            currentTotalRotation += rotationAngle;
        }

        if ((segmentCount - 1) % segmentBeforeTurn == 0 && nextRotation.eulerAngles.y != currentRotation.eulerAngles.y)
        {
            SegmentController.ActiceRotateTriggerBox(true);
        }
        else
        {
            SegmentController.ActiceRotateTriggerBox(false);

        }

        nextSpawnPoint += newSegment.transform.forward * SEGMENT_SQUARE_SIZE;
        lavaSpawnPoint += newSegment.transform.forward * SEGMENT_SQUARE_SIZE;
        if (LeftWall.Count != 0)
        {
            LeftWall[0].SetActive(false);
        }

        if (RightWall.Count != 0)
        {
            RightWall[0].SetActive(false);
        }
    }

    void SpawnItems(Vector3 spawnDirection, Quaternion RotateOfPlane)
    {
        float totalRate = 0;
        foreach (float rate in spawnRates)
        {
            totalRate += rate;
        }
        float randomSpawn = Random.Range(0, totalRate);
        for (int i = 0; i < ItemsList.Count; i++)
        {
            float randomPosition = Random.Range(-3.3f, 3.3f);
            if (randomSpawn < spawnRates[i])
            {
                GameObject item = Instantiate(ItemsList[i], spawnDirection + new Vector3(0f, 2f, 0f), Quaternion.identity);

                if (RotateOfPlane.eulerAngles.y == 270 || RotateOfPlane.eulerAngles.y == 90)
                {
                    item.transform.position += new Vector3(0f, 0f, randomPosition);
                }
                else
                {
                    item.transform.position += new Vector3(randomPosition, 0f, 0f);
                }
                ItemsSpawn.Add(item);
                return;
            }
            else
            {
                randomSpawn -= spawnRates[i];
            }
        }
    }


    void ActiveItem()
    {
        float activationRate = 0.3f;

        foreach (GameObject item in ItemsSpawn)
        {
            float randomSpawn = Random.Range(0f, 1f);

            if (randomSpawn < activationRate)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }

        }

    }



    void SpawnTrap(Vector3 spawnDirection, Quaternion RotateOfPlane)
    {
        float spawnTrapRate = 0.1f;

        float randomSpawn = Random.Range(0f, 1f);

        if (randomSpawn < spawnTrapRate)
        {
            Instantiate(ObstaclePrefabs, spawnDirection + new Vector3(0f, 3f, 0f), RotateOfPlane);
        }
    }
    void SpawnCoins(Vector3 spawnDirection, Quaternion RotateOfPlane)
    {
        float SpanwCoinsRate = 0.8f;

        float randomSpawn = Random.Range(0f, 1f);

        Vector3 spawnPosition = spawnDirection + new Vector3(0f, 2f, 0f);
        if (randomSpawn < SpanwCoinsRate)
        {
            Vector3 spawnForward;
            float randomCoins = Random.Range(-3.3f, 3.3f);

            if (RotateOfPlane.eulerAngles.y == 0)
            {
                spawnForward = new Vector3(0f, 0f, coinsSpacing);
            }
            else
            {
                spawnForward = new Vector3(coinsSpacing, 0f, 0f);
            }

            for (int i = 0; i < coinsPerRows; i++)
            {
                if (RotateOfPlane.eulerAngles.y == 0)
                {
                    Instantiate(CoinsPrefabs, spawnPosition + new Vector3(randomCoins, 0f, 0f), Quaternion.Euler(90, 0, 0));
                }
                else
                {
                    Instantiate(CoinsPrefabs, spawnPosition + new Vector3(0f, 0f, randomCoins), Quaternion.Euler(90, 0, 0));
                }
                spawnPosition += spawnForward;
            }
        }
    }

    int RandomTurnWithConstraint()
    {
        int randomTurn;

        switch (currentTotalRotation)
        {
            case 90:
                randomTurn = Random.Range(-1, 1) * 90;
                break;
            case -90:
                randomTurn = Random.Range(0, 2) * 90;
                break;
            default:
                randomTurn = Random.Range(-1, 2) * 90;
                break;
        }
        return randomTurn;
    }


}

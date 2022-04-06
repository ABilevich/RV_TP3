using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{

    public GameObject buildingPrefab;
    public float clearance;
    public Transform origin;
    public LayerMask m_buildings;
    public float blockSize = 1;
    public int boxSize = 10;
    public int blocksInBlock = 5;
    public float despawnRadious = 20;
    public int oceanGap = 100;
    public float oceanPercentage = 0.5f;
    private float segmentSize;
    public float minBuildingHeight = 1;
    public float maxBuildingHeight = 100;
    public Material[] materials;
    public GameObject[] spawnedConcretes;
    public GameObject concretePrefab;
    public GameObject[] spawnedStreets;
    public GameObject streetPrefab;
    public GameObject lampPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 actualConcretePosition = spawnedConcretes[1].transform.position;
        Vector3 concreteSpawnPoint = new Vector3(actualConcretePosition.x, actualConcretePosition.y, actualConcretePosition.z + 2 * concretePrefab.transform.localScale.z);
        spawnedConcretes[2] = Instantiate(concretePrefab, concreteSpawnPoint, Quaternion.identity);

        Vector3 actualStreetPosition = spawnedStreets[1].transform.position;
        Vector3 streetSpawnPoint = new Vector3(actualStreetPosition.x, actualStreetPosition.y, actualStreetPosition.z + 750);
        spawnedStreets[2] = Instantiate(streetPrefab, streetSpawnPoint, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = transform.position;
        float offset = (blockSize * boxSize/2);
        Vector3 botomLeftCorner = new Vector3(playerPos.x - offset, 0, playerPos.z - offset);
        Vector3 topRightCorner = new Vector3(playerPos.x + offset, 0, playerPos.z + offset);

        int minX = (int) (botomLeftCorner.x / blockSize);
        int maxX = (int) (topRightCorner.x / blockSize);

        int minZ = (int) (botomLeftCorner.z / blockSize);
        int maxZ = (int) (topRightCorner.z / blockSize);


        for (int x = minX; x <= maxX; x++){
            for (int z = minZ; z  <= maxZ; z ++){
                float buildingHeight = Random.Range(minBuildingHeight, maxBuildingHeight);
                Vector3 spawnPoint = new Vector3(x * blockSize + blockSize/2, buildingHeight/2 + 3, z * blockSize + blockSize/2);
                Vector3 buildingScale = buildingPrefab.transform.localScale;
                Vector3 boundry = new Vector3(buildingScale.x/2, maxBuildingHeight, buildingScale.y/2);
                Collider[] hitColliders = Physics.OverlapBox(spawnPoint,boundry, Quaternion.identity, m_buildings);
                bool isOnStreet = (x % blocksInBlock) == 0 || (z % blocksInBlock) == 0 || (z % oceanGap) > oceanGap * oceanPercentage;

                float distance = Vector3.Distance(spawnPoint, transform.position);
                bool canSpown = distance < despawnRadious;

                if(hitColliders.Length == 0 && canSpown){
                    if(!isOnStreet){
                        GameObject newBlock = Instantiate(buildingPrefab, spawnPoint, Quaternion.identity) as GameObject;
                        newBlock.transform.localScale = new Vector3(blockSize, buildingHeight, blockSize);
                        newBlock.transform.parent = origin.transform;
                        newBlock.gameObject.GetComponent<Renderer>().material = materials[Random.Range(0,9)];
                    }
                }
            }
        }

        for(int i = 0; i< origin.transform.childCount; i++){
            GameObject child = origin.transform.GetChild(i).gameObject;
            float distance = Vector3.Distance(child.transform.position, transform.position);
            if(distance > despawnRadious){
                Destroy(child);
            }
        }

        if(Mathf.Abs(Camera.main.transform.position.z - spawnedConcretes[2].transform.position.z) < 150){
            Destroy(spawnedConcretes[0]);
            spawnedConcretes[0] = spawnedConcretes[1];
            spawnedConcretes[1] = spawnedConcretes[2];
            Vector3 actualConcretePosition = spawnedConcretes[1].transform.position;
            Vector3 concreteSpawnPoint = new Vector3(actualConcretePosition.x, actualConcretePosition.y, actualConcretePosition.z + 2 * concretePrefab.transform.localScale.z);
            spawnedConcretes[2] = Instantiate(concretePrefab, concreteSpawnPoint, Quaternion.identity);
        }

        if(Mathf.Abs(Camera.main.transform.position.z - spawnedStreets[2].transform.position.z) < 350){
            Destroy(spawnedStreets[0]);
            spawnedStreets[0] = spawnedStreets[1];
            spawnedStreets[1] = spawnedStreets[2];
            Vector3 actualStreetPosition = spawnedStreets[1].transform.position;
            Vector3 streetSpawnPoint = new Vector3(actualStreetPosition.x, actualStreetPosition.y, actualStreetPosition.z + 750);
            spawnedStreets[2] = Instantiate(streetPrefab, streetSpawnPoint, Quaternion.identity);
        }
    }

    // Vector3 getSpawnPosition(){
    //     Vector3 center = transform.position;
    //     Debug.Log("center: " + center);
    //     Vector3 randomDisplacement = Random.insideUnitCircle * 5;
    //     Debug.Log("randomDisplacement: " + randomDisplacement);
    //     Vector3 randomPoint = new Vector3(center.x + randomDisplacement.x,0,center.z + randomDisplacement.y);
    //     Debug.Log("randomPoint: " + randomPoint);
    //     Instantiate(buildingPrefab, randomPoint, Quaternion.identity);
    //     return center;
    // }
}

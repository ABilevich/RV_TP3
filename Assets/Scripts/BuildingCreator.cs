using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{

    public GameObject buildingPrefab;
    public GameObject streetPrefab;
    public float clearance;
    public Transform origin;
    public LayerMask m_buildings;
    public float blockSize = 1;
    public int boxSize = 10;
    public int blocksInBlock = 5;
    public float despawnRadious = 20;

    public int oceanGap = 30;
    public float oceanPercentage = 0.5f;


    private float segmentSize;

    public float minBuildingHeight = 1;
    public float maxBuildingHeight = 100;
    public Material[] materials;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 spawnPossition = getSpawnPosition();

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
                Vector3 spawnPoint = new Vector3(x * blockSize + blockSize/2, buildingHeight/2, z * blockSize + blockSize/2);
                // Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
                Vector3 buildingScale = buildingPrefab.transform.localScale;
                Vector3 boundry = new Vector3(buildingScale.x/2, maxBuildingHeight, buildingScale.y/2);
                Collider[] hitColliders = Physics.OverlapBox(spawnPoint,boundry, Quaternion.identity, m_buildings);
                // Collider[] hitColliders = Physics.OverlapSphere(new Vector3(spawnPoint.x, 0, spawnPoint.z), blockSize/3);
                bool isOnStreet = (x % blocksInBlock) == 0 || (z % blocksInBlock) == 0 || (z % oceanGap) > oceanGap * oceanPercentage;

                float distance = Vector3.Distance(spawnPoint, transform.position);
                bool canSpown = distance < despawnRadious;

                if(hitColliders.Length == 0 && canSpown){
                    //Debug.Log("spawnin");
                    if(!isOnStreet){
                        GameObject newBlock = Instantiate(buildingPrefab, spawnPoint, Quaternion.identity) as GameObject;
                        newBlock.transform.localScale = new Vector3(blockSize, buildingHeight, blockSize);
                        newBlock.transform.parent = origin.transform;
                        newBlock.gameObject.GetComponent<Renderer>().material = materials[Random.Range(0,9)];
                    }

                }
            }
        }

        // Debug.Log(origin.transform.childCount);
        for(int i = 0; i< origin.transform.childCount; i++){
            GameObject child = origin.transform.GetChild(i).gameObject;
            float distance = Vector3.Distance(child.transform.position, transform.position);
            // Debug.Log(distance);
            if(distance > despawnRadious){
                Destroy(child);
            }
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

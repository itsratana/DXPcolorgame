using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] blockPrefab;
    public List<int> indexSpawnedPrefabs = new List<int>();
    public int amountBlock = 3;
    public GameObject finishBlock;
    public GameObject startBlock;
    public float zIndex = 0;
    void Start()
    {
        GenerateLevel();
    }
    void GenerateLevel()
    {
        float myZ = 0;
        for (int i = 0; i <= amountBlock; i++)
        {
            if (i == 0)
            {                
                Instantiate(startBlock, transform.position, Quaternion.identity);
            }
            else
            {
                myZ += zIndex;
                while (true)
                {
                    int randomLevel = Random.Range(0, blockPrefab.Length);                   
                    if (!indexSpawnedPrefabs.Contains(randomLevel))
                    {
                        Instantiate(blockPrefab[randomLevel], transform.position + new Vector3(0, 0, myZ), Quaternion.identity);
                        indexSpawnedPrefabs.Add(randomLevel);
                        break;
                    }                                   
                }               
            }
            if (i == amountBlock)
            {
                myZ += zIndex;
                Instantiate(finishBlock, transform.position + new Vector3(0, 0, myZ), Quaternion.identity);
            }
        }
    }
}

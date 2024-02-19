using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] blockPrefab;
    public int amountBlock = 3;
    public GameObject finishBlock;
    public int zIndex = 0;
    void Start()
    {
        GenerateLevel();
    }
    void GenerateLevel()
    {
        int myZ = 0;
        for (int i = 0; i <= amountBlock; i++)
        {
            if (i == amountBlock)
            {
                myZ += zIndex;
                Instantiate(finishBlock, transform.position + new Vector3(0, 0, myZ), Quaternion.identity);
            }
            else if (i == 0)
            {
                int randomLevel = Random.Range(0, blockPrefab.Length);
                Instantiate(blockPrefab[randomLevel], transform.position, Quaternion.identity);
            }
            else
            {
                myZ += zIndex;
                int randomLevel = Random.Range(0, blockPrefab.Length);
                Instantiate(blockPrefab[randomLevel], transform.position + new Vector3(0, 0, myZ), Quaternion.identity);
            }
        }
    }
}

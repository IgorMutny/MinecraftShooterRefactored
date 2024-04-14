using System.Collections.Generic;
using UnityEngine;

public class MapOptimizer : MonoBehaviour
{
    [SerializeField] private GameObject _grass44;
    [SerializeField] private GameObject _grass22;
    [SerializeField] private Transform _parent;

    private GameObject[,,] _blocks = new GameObject[100, 20, 100];

    private int _removedBlocks;

    [ContextMenu("Optimize Map")]
    public void Optimize()
    {
        _removedBlocks = 0;

        CreateBlockArray();

        for (int i = 0; i < _blocks.GetLength(1); i++)
        {
            OptimizeHeight(i);
        }

        Debug.Log("Removed blocks: " + _removedBlocks);
    }

    [ContextMenu("Optimize Map Small")]
    public void OptimizeSmall()
    {
        _removedBlocks = 0;

        CreateBlockArray();

        for (int i = 0; i < _blocks.GetLength(1); i++)
        {
            OptimizeHeightSmall(i);
        }

        Debug.Log("Removed blocks: " + _removedBlocks);
    }

    private void CreateBlockArray()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("GrassBlock");

        foreach (GameObject block in blocks)
        {
            int x = (int)block.transform.position.x;
            int y = (int)block.transform.position.y;
            int z = (int)block.transform.position.z;

            _blocks[x, y, z] = block;
        }
    }

    private void OptimizeHeight(int y)
    {
        for (int x = 0; x < _blocks.GetLength(0) - 4; x++)
        {
            for (int z = 0; z < _blocks.GetLength(2) - 4; z++)
            {
                List<GameObject> optimizedBlocks = new List<GameObject>();

                for (int x1 = 0; x1 < 4; x1++)
                {
                    for (int z1 = 0; z1 < 4; z1++)
                    {
                        optimizedBlocks.Add(_blocks[x + x1, y, z + z1]);
                    }
                }

                if (IsValidSetOfBlocks(optimizedBlocks) == true)
                {
                    DestroyBlocks(optimizedBlocks);
                    Instantiate(_grass44, new Vector3(x, y, z), Quaternion.identity, _parent);
                }
            }
        }
    }

    private void OptimizeHeightSmall(int y)
    {
        for (int x = 0; x < _blocks.GetLength(0) - 2; x++)
        {
            for (int z = 0; z < _blocks.GetLength(2) - 2; z++)
            {
                List<GameObject> optimizedBlocks = new List<GameObject>();

                for (int x1 = 0; x1 < 2; x1++)
                {
                    for (int z1 = 0; z1 < 2; z1++)
                    {
                        optimizedBlocks.Add(_blocks[x + x1, y, z + z1]);
                    }
                }

                if (IsValidSetOfBlocks(optimizedBlocks) == true)
                {
                    DestroyBlocks(optimizedBlocks);
                    Instantiate(_grass22, new Vector3(x, y, z), Quaternion.identity, _parent);
                }
            }
        }
    }

    private bool IsValidSetOfBlocks(List<GameObject> blocks)
    {
        foreach (GameObject block in blocks)
        {
            if (block == null)
            {
                return false;
            }
        }

        return true;
    }

    private void DestroyBlocks(List<GameObject> blocks)
    {
        foreach (GameObject block in blocks)
        {
            if (block != null)
            {
                _removedBlocks += 1;
                DestroyImmediate(block);
            }
        }
    }

    [ContextMenu("Count")]
    public void Count()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        Debug.Log("Total: " + gameObjects.Length);
    }
}

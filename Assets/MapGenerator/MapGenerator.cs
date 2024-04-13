using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private readonly int _noBlock = int.MaxValue;

    [SerializeField] private Texture2D _texture;
    [SerializeField] private GameObject _mainBlock;
    [SerializeField] private GameObject _lowerBlock;
    [SerializeField] private GameObject[] _plants;
    [SerializeField] private float[] _plantChances;

    [SerializeField] private float _heightMultiplier;

    private Transform _level;
    private int[,] _heights;

    [ContextMenu("Create Map")]
    public void Create()
    {
        _level = new GameObject("Environment").transform;
        _heights = new int[_texture.width, _texture.height];

        AddMainBlocks();
        AddLowerBlocks();
    }

    private void AddMainBlocks()
    {
        for (int x = 0; x < _texture.width; x++)
        {
            for (int y = 0; y < _texture.height; y++)
            {
                Color color = _texture.GetPixel(x, y);
                AddMainBlock(x, y, color);
            }
        }
    }

    private void AddMainBlock(int x, int z, Color color)
    {
        float height = color.r * _heightMultiplier;
        int y = (int)Mathf.Round(height);
        Vector3 position = new Vector3(x, y, z);
        _heights[x, z] = y;

        if (color.r > 0 && color.a == 1)
        {
            Instantiate(_mainBlock, position, Quaternion.identity, _level);
            TryAddPlants(x, y + 1, z);
        }
        else
        {
            _heights[x, z] = _noBlock;
        }
    }

    private void AddLowerBlocks()
    {
        for (int x = 0; x < _texture.width; x++)
        {
            for (int z = 0; z < _texture.height; z++)
            {
                int height = _heights[x, z];

                if (height != _noBlock)
                {
                    int minNeighbourHeight = GetMinNeighboursHeight(x, z);
                    if (height - minNeighbourHeight > 1)
                    {
                        for (int y = minNeighbourHeight + 1; y < height; y++)
                        {
                            Vector3 position = new Vector3(x, y, z);
                            Instantiate(_lowerBlock, position, Quaternion.identity, _level);
                        }
                    }
                }
            }
        }
    }

    //лопните мои глаза...
    private int GetMinNeighboursHeight(int x, int z)
    {
        int[] neighbourHeights = new int[4];

        if (x > 0)
        {
            neighbourHeights[0] = _heights[x - 1, z];
        }
        else
        {
            neighbourHeights[0] = _heights[x, z];
        }

        if (x < _heights.GetLength(0) - 1)
        {
            neighbourHeights[1] = _heights[x + 1, z];
        }
        else
        {
            neighbourHeights[1] = _heights[x, z];
        }

        if (z > 0)
        {
            neighbourHeights[2] = _heights[x, z - 1];
        }
        else
        {
            neighbourHeights[2] = _heights[x, z];
        }

        if (z < _heights.GetLength(1) - 1)
        {
            neighbourHeights[3] = _heights[x, z + 1];
        }
        else
        {
            neighbourHeights[3] = _heights[x, z];
        }

        int result = Mathf.Min(neighbourHeights);

        return result;
    }

    private void TryAddPlants(int x, int y, int z)
    {
        for (int i = 0; i < _plants.Length; i++)
        {
            float rnd = Random.Range(0.0f, 1.0f);
            if (rnd <= _plantChances[i])
            {
                Instantiate(_plants[i], new Vector3(x, y, z), Quaternion.identity, _level);
                return;
            }
        }
    }
}

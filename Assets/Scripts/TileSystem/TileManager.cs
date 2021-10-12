using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileManager : MonoBehaviour
{
    #region instance
    private static TileManager _instance;
    public static TileManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find("TileManager");
                if (obj == null)
                {
                    obj = new GameObject("TileManager");
                    obj.AddComponent<TileManager>();
                    GameObject cubeMap = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubeMap.transform.SetParent(obj.transform);
                    cubeMap.name = "CubeMap";
                }
                return obj.GetComponent<TileManager>();
            }
            else
            {
                return _instance;
            }
        }
    }
    #endregion

    [Range (1, 4096)]
    public int tilemapSizeX = 1;
    [Range(1, 4096)]
    public int tilemapSizeZ = 1;

    void OnValidate()
    {
        UpdateGrid();
    }

    public void UpdateGrid()
    {
        GameObject cubeMap = transform.GetChild(0).gameObject;
        cubeMap.transform.localScale = new Vector3(tilemapSizeX, 0.1f, tilemapSizeZ);
    }
}

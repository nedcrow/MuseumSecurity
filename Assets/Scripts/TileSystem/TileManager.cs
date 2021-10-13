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

    [Range(1, 4096)]
    public int tilemapSizeX = 1;
    [Range(1, 4096)]
    public int tilemapSizeZ = 1;
    [Range(0.1f, 10.0f)]
    public float tileScale_Meter = 1;
    [HideInInspector]
    public List<Vector3> tileLocations;

    void OnValidate()
    {
        UpdateTileMap();
    }

    public void UpdateTileMap()
    {
        tileLocations.Clear();

        UpdateMesh();
        
        UpdateTileLocations();
    }

    void UpdateMesh()
    {
        GameObject cubeMap = transform.GetChild(0).gameObject;
        cubeMap.transform.localScale = new Vector3(tilemapSizeX, 0.1f, tilemapSizeZ);

        bool isNullGridMaterial = !cubeMap.GetComponent<Renderer>().sharedMaterial ||
            !cubeMap.GetComponent<Renderer>().sharedMaterial.mainTexture ||
            cubeMap.GetComponent<Renderer>().sharedMaterial.mainTexture.name == null ||
            cubeMap.GetComponent<Renderer>().sharedMaterial.mainTexture.name != "Grid";

        if (isNullGridMaterial)
        {
            Material m = Resources.Load("Materials/Grid", typeof(Material)) as Material;

            bool isNullGridMaterialResource = !m || m == null;
            if (!isNullGridMaterialResource)
            {
                m.mainTextureScale.Set(tilemapSizeX, tilemapSizeZ);
                cubeMap.GetComponent<Renderer>().material = m;
            }
            else
            {
                Debug.Log("Warning. Has not material [ Resources/Materials/Grid ].");
            }
        }
    }

    void UpdateTileLocations()
    {
        float halfTilemapSizeX = tilemapSizeX * 0.5f;
        float halfTilemapSizeZ = tilemapSizeZ * 0.5f;
        float halfTileScale = tileScale_Meter * 0.5f;

        for (int x = 0; x < tilemapSizeX; x++)
        {
            for (int z = 0; z < tilemapSizeZ; z++)
            {
                tileLocations.Add(new Vector3(
                    x - halfTilemapSizeX + halfTileScale,
                    .0f,
                    z - halfTilemapSizeZ + halfTileScale
                ) * tileScale_Meter);
            };
        };
    }
}

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
        // scale
        GameObject meshObj = transform.GetChild(0).gameObject;
        meshObj.transform.localScale = new Vector3(tilemapSizeX * tileScale_Meter, 0.1f, tilemapSizeZ * tileScale_Meter);

        // grid
        Material tempMaterial = meshObj.GetComponent<Renderer>().sharedMaterial;
        bool isNullGridMaterial = tempMaterial == null || 
            tempMaterial.mainTexture == null ||
            tempMaterial.mainTexture.name == null ||
            tempMaterial.mainTexture.name != "Grid";

        if (isNullGridMaterial)
        {
            tempMaterial = Resources.Load("Materials/Grid", typeof(Material)) as Material;
        }

        bool isNullGridMaterialResource = !tempMaterial || tempMaterial == null;
        if (!isNullGridMaterialResource)
        {
            tempMaterial.mainTextureScale = new Vector2(tilemapSizeX, tilemapSizeZ);
            meshObj.GetComponent<Renderer>().material = tempMaterial;
        }
        else
        {
            Debug.Log("Warning. Has not material [ Resources/Materials/Grid ].");
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

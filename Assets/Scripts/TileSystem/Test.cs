using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    public int tilemapSizeX = 0;
    void OnValidate()
    {
        TileManager.instance.UpdateGrid();
    }
}

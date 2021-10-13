using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 컴포넌트가 붙은 GameObject 위치를 가장 가까운 TileManager Grid 위로 보정한다.
/// </summary>

[ExecuteInEditMode]
public class SnapGridComponent : MonoBehaviour
{
    void Update()
    {
        if (transform.hasChanged)
        {
            SnapGrid();
        }
    }

    void SnapGrid()
    {
        TileManager tileManager = TileManager.instance;
        int halfTilemapSizeX = Mathf.FloorToInt(tileManager.tilemapSizeX / 2);
        int halfTilemapSizeZ = Mathf.FloorToInt(tileManager.tilemapSizeZ / 2);

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = transform.position;
        float closetDistanceValue = -1;

        for (int x = 0; x < tileManager.tilemapSizeX; x++)
        {
            if (currentPosition.x < 0 && x >= halfTilemapSizeX) { goto SKIP_Z_LOOP; }
            if (currentPosition.x >= 0 && x < halfTilemapSizeX) { goto SKIP_Z_LOOP; }

            for (int z = 0; z < tileManager.tilemapSizeZ; z++)
            {
                if (currentPosition.z < 0 && z >= halfTilemapSizeZ) { goto SKIP_Z_CHECK; }
                if (currentPosition.z >= 0 && z < halfTilemapSizeZ) { goto SKIP_Z_CHECK; }

                int index = x * tileManager.tilemapSizeZ + z;
                float distanceValue = Vector3.Distance(transform.position, tileManager.tileLocations[index]);
                if (closetDistanceValue < 0 || closetDistanceValue > distanceValue)
                {
                    closetDistanceValue = distanceValue;
                    targetPosition = tileManager.tileLocations[index];
                }
                SKIP_Z_CHECK: { }
            }
            SKIP_Z_LOOP: { }
        }

        transform.position = targetPosition;
    }
}

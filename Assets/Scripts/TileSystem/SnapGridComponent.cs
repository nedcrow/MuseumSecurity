using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 컴포넌트가 붙은 GameObject 위치를 가장 가까운 TileManager Grid 위로 보정한다.
/// </summary>

[ExecuteInEditMode]
public class SnapGridComponent : MonoBehaviour
{
    [Tooltip("pivot 위치가 발 밑이 아닌 경우에 사용을 추천합니다.")]
    public bool isSnapWithColliderHeight = false;

    [Tooltip("해당 게임오브젝트의 크기를 tileScale 크기로 덮어씁니다.")]
    public bool isCoverScaleWithTileScale = false;

    [Tooltip("해당 게임오브젝트가 차지하는 타일 수. 정수형만 가능합니다.")]
    public Vector3 tileScale = Vector3.one;
    private Vector3 savedScale = Vector3.one;
    void Update()
    {
        if (transform.hasChanged)
        {
            SnapGrid();
        }
    }

    void OnValidate()
    {
        ConvertScaleInt();
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
        if(isSnapWithColliderHeight) {
            if (tileScale.x % 2 == 0) targetPosition += new Vector3(tileManager.tileScale_Meter * 0.5f, 0, 0);
            if (tileScale.z % 2 == 0) targetPosition += new Vector3(0, 0, tileManager.tileScale_Meter * 0.5f);
            targetPosition.Set(targetPosition.x, targetPosition.y + (GetColliderHeight() * 0.5f), targetPosition.z);
        }
        transform.position = targetPosition;
    }

    void ConvertScaleInt()
    {
        savedScale = isCoverScaleWithTileScale ? transform.localScale : savedScale;
        tileScale.Set(Mathf.Floor(tileScale.x), Mathf.Floor(tileScale.y), Mathf.Floor(tileScale.z));
        transform.localScale = isCoverScaleWithTileScale ? tileScale : savedScale;
    }

    float GetColliderHeight()
    {
        float result = 0.0f;
        Collider collider = GetComponent<Collider>();
        if (collider != null) result = collider.transform.localScale.y;
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private List<TileData> tileDatas;
    [SerializeField]
    private int mycelliumSpread;
    [SerializeField]
    private TileBase mycelliumTile;
    private Dictionary<TileBase, TileData> dataFromTiles;
    
    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
                print(tileData.isRootable);
            }
        }
    }

    private void Update()
    {
        
    }

    public float getMovementMultiplikator(Vector2 position){
        Vector3Int gridPosition = map.WorldToCell(position);
        TileBase tile = map.GetTile(gridPosition);
        if (tile == null)
        {
            return 1f;
        }
        return dataFromTiles[tile].movementMultiplicator;
    }

    public void rootGround(Vector2 position)
    {
        changeTile(position);

        for (int k = 1; k < mycelliumSpread; k++)
        {
            for (int i = -mycelliumSpread + k; i <= mycelliumSpread - k; i++)
            {
                for (int j = -mycelliumSpread + k; j < mycelliumSpread - k; j++)
                {
                    if (neighborWasUpdatedThisFrame(position + new Vector2(i, j)))
                    {
                        changeTile(position + new Vector2(i, j));
                    }
                }
            }
        }
        
    }

    private void changeTile(Vector2 position)
    {
        TileBase tile = getTileFromPosition(position);
        if (tile != null)
        {
            if (dataFromTiles[tile].isRootable)
            {
                map.SetTile(map.WorldToCell(position), mycelliumTile);
            }
        }
    }

    private bool neighborWasUpdatedThisFrame(Vector2 position)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                TileBase tile = getTileFromPosition(position + new Vector2(i, j));
                if (tile != null && dataFromTiles[tile].isMycellium )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private TileBase getTileFromPosition(Vector2 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);
        return map.GetTile(gridPosition);
    }
}

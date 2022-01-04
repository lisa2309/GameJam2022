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
        for (int i = -mycelliumSpread ; i <= mycelliumSpread; i++)
        {
            for (int j = -mycelliumSpread; j < mycelliumSpread; j++)
            {
                changeTile(position - new Vector2(i, j));
            }
        }
    }

    private void changeTile(Vector2 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);
        TileBase tile = map.GetTile(gridPosition);
        if (tile != null && dataFromTiles[tile].isRootable)
        {
            map.SetTile(gridPosition, mycelliumTile);
        }
    }
}

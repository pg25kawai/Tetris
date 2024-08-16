using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField] private int _height;
    [SerializeField] private int _width;
    [SerializeField] private Tile _tileBlock;
    [SerializeField] private Tile _tileBackground;

    private Tilemap _tileMap;
    private int[,] _boardMatrix;

    //public Tilemap TileMap => _tileMap;
    //public int Height => _height;
    //public int Width => _width;

    void Awake()
    {
        _tileMap = GetComponentInChildren<Tilemap>();
        CreateBoard();
    }

    private void CreateBoard()
    {
        _boardMatrix = new int[_width, _height];
    }

    public bool IsCoordValid(Vector2Int coords)
    {
        return !IsOutOfBound(coords) && IsCellEmpty(coords);
    }

    private bool IsCellEmpty(Vector2Int coords)
    {
        return _boardMatrix[coords.x, coords.y] == 0;
    }

    private bool IsOutOfBound(Vector2Int coords)
    {
        return IsOutOfBoundX(coords.x) || IsOutOfBoundY(coords.y);
    }

    private bool IsOutOfBoundX(int xCoord)
    {
        return xCoord < 0 || xCoord >= _width;
    }

    private bool IsOutOfBoundY(int yCoord)
    {
        return yCoord < 0 || yCoord >= _height;
    }

    public void SetBlockOnTileMap(Vector2Int[] coords, bool toClear = false)
    {
        Tile tile = toClear ? _tileBackground : _tileBlock;
        int boardValue = toClear ? 0 : 1;
        foreach (Vector2Int cellCoord in coords)
        {
            _tileMap.SetTile((Vector3Int)cellCoord, tile);
        }
    }

    public void SetBlockOnMatrix(Vector2Int[] coords, bool toClear = false)
    {
        int value = toClear ? 0 : 1;
        foreach (Vector2Int cellCoord in coords)
        {
            _boardMatrix[cellCoord.x, cellCoord.y] = value;
        }
        Debug.Log("");
    }

    public bool IsLineFilled(int yCoord)
    {
        for (int x = 0; x < _width; x++)
        {
            if (IsCellEmpty(new Vector2Int(x, yCoord)))
                return false;
        }

        return true;
    }

    public void ClearLine(int yCoord)
    {
        for (int x = 0; x < _width; x++)
        {
            _tileMap.SetTile(new Vector3Int(x, yCoord), null);
            _boardMatrix[x, yCoord] = 0;
        }
    }

    public void ShiftLinesDown(int yCoord)
    {
        int y = yCoord;
        while (y < _height)
        {
            for (int x = 0; x < _width; x++)
            {
                TileBase tile = _tileMap.GetTile(new Vector3Int(x, y + 1));
                _tileMap.SetTile(new Vector3Int(x, y), tile);
                if (!IsOutOfBoundY(y + 1))
                    _boardMatrix[x, y] = _boardMatrix[x, y + 1];
            }
            y++;
        }
    }
}

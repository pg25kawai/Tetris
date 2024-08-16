using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    // The coords of the pivot is always at origin
    private Vector2Int[] _coords;
    // Offset the coords to get the actual coords on board
    private Vector2Int _offset;
    private Board _board;

    public Vector2Int[] Coords => _coords;
    public Vector2Int PivotPosition { get; set; }


    void Awake()
    {
        _board = FindFirstObjectByType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Vector2Int[] coords)
    {
        _coords = coords;
        // Initial offset is at the next block display area
        _offset = new Vector2Int(14, 14);
    }

    public void SetPivotToSpawnLocation()
    {
        _offset = new Vector2Int(5, 18);
    }

    public Vector2Int[] GetCoordsAfterOffset()
    {
        Vector2Int[] newCoords = new Vector2Int[_coords.Length];
        for (int i = 0; i < _coords.Length; i++)
        {
            newCoords[i].x = _coords[i].x + _offset.x;
            newCoords[i].y = _coords[i].y + _offset.y;
        }

        return newCoords;
    }

    public void Translate(Vector2Int offset)
    {
        Vector2Int[] newCoords = new Vector2Int[_coords.Length];
        bool isCoordValid = true;
        for (int i = 0; i < _coords.Length; i++)
        {
            newCoords[i].x = _coords[i].x + _offset.x + offset.x;
            newCoords[i].y = _coords[i].y + _offset.y + offset.y;

            isCoordValid = isCoordValid && 
                _board.IsCoordValid(newCoords[i]);
        }

        if (isCoordValid)
        {
            _board.SetBlockOnTileMap(GetCoordsAfterOffset(), toClear: true);
            _offset += offset;
            _board.SetBlockOnTileMap(GetCoordsAfterOffset());
        }
    }

    public void Rotate(float angle)
    {
        Vector2Int[] newCoords = new Vector2Int[_coords.Length];
        bool isCoordValid = true;
        angle *= Mathf.Deg2Rad;
        for (int i = 0; i < _coords.Length; i++)
        {
            newCoords[i].x = 
                (int)Mathf.Cos(angle) * _coords[i].x - 
                (int)Mathf.Sin(angle) * _coords[i].y;

            newCoords[i].y = 
                (int)Mathf.Sin(angle) * _coords[i].x + 
                (int)Mathf.Cos(angle) * _coords[i].y;

            isCoordValid = isCoordValid && 
                _board.IsCoordValid(newCoords[i]);
        }

        if (isCoordValid)
        {
            _board.SetBlockOnTileMap(GetCoordsAfterOffset(), toClear: true);
            _coords = newCoords;
            _board.SetBlockOnTileMap(GetCoordsAfterOffset());
        }
    }
}

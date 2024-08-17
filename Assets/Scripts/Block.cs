using System;
using UnityEngine;
using UnityEngine.Events;

public class Block : MonoBehaviour
{

    [SerializeField] private UnityEvent _nextBlockEvent;

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

    public void Initialize(Vector2Int[] coords)
    {
        _coords = coords;
        // Initial offset is at the next block display area
        _offset = new Vector2Int(14, 14);
    }

    public void SetOffsetToSpawnLocation()
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

            if (!isCoordValid)
                break;
        }

        if (isCoordValid)
        {
            _board.SetBlockOnTileMap(GetCoordsAfterOffset(), toClear: true);
            _offset += offset;
            _board.SetBlockOnTileMap(GetCoordsAfterOffset());
        }

        // Stop the block if attempt to move down but can't
        else if (offset.y == -1)
        {
            StopBlock();
        }
    }

    public void Rotate(float angle = 90)
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

            Vector2Int coordWithOffset = new Vector2Int(
                newCoords[i].x + _offset.x, 
                newCoords[i].y + _offset.y); 

            isCoordValid = isCoordValid && 
                _board.IsCoordValid(coordWithOffset);
        }

        if (isCoordValid)
        {
            _board.SetBlockOnTileMap(GetCoordsAfterOffset(), toClear: true);
            _coords = newCoords;
            _board.SetBlockOnTileMap(GetCoordsAfterOffset());
        }
    }

    public void StopBlock()
    {
        _board.SetBlockOnMatrix(GetCoordsAfterOffset());

        // Get all y coords and sort them in ascending order
        int[] yCoords = new int[_coords.Length];
        for (int i = 0; i < _coords.Length; i++)
        {
            int y = _coords[i].y + _offset.y;
            yCoords[i] = y;
        }
        Array.Sort(yCoords);

        // Check each line affected by the block
        // to see if entire line is filled
        // Need to check from top to bottom as shift down from
        // bottom to top will affect coord at top
        for (int i = yCoords.Length - 1; i >= 0; i--)
        {
            if (_board.IsLineFilled(yCoords[i]))
            {
                _board.ClearLine(yCoords[i]);
                _board.ShiftLinesDown(yCoords[i]);
            }
        }

        // Event for GameManager for next round
        _nextBlockEvent.Invoke();
        Destroy(gameObject);
    }
}

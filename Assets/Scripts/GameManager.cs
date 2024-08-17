using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private LevelData[] _levelData;
    [SerializeField] private UnityEvent _showWinScreenEvent;
    [SerializeField] private UnityEvent _showLoseScreenEvent;

    private BlockController _controller;
    private Dictionary<BlockType, Vector2Int[]> _blockCoordsDict;
    private Board _board;
    private Queue<Block> _blocksQueue = new Queue<Block>();
    private float _score;
    private int _currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        _blockCoordsDict = GetComponent<BlockData>().BlockDataDict;
        _controller = GetComponent<BlockController>();
        _board = FindFirstObjectByType<Board>();
        _currentLevel = -1;
        //SpawnBlock();
        //NextRound();
    }

    public void RestartFromFirstLevel()
    {
        _currentLevel = -1;
        NextLevel();
    }

    public void NextLevel()
    {
        _score = 0;
        _currentLevel++;
        _controller.DropSpeed = _levelData[_currentLevel].SpeedMin;
        _board.ResetBoard();
        SpawnBlock();
        NextRound();
    }

    public void NextRound()
    {
        // Get next block and clear block from next block area
        Block controlledBlock = _blocksQueue.Dequeue();
        _board.SetBlockOnTileMap(controlledBlock.GetCoordsAfterOffset(), toClear: true);

        // Spawn at top of board
        controlledBlock.SetOffsetToSpawnLocation();
        _controller.ControlledBlock = controlledBlock;
        if (_controller.DropSpeed > _levelData[_currentLevel].SpeedMax)
            _controller.DropSpeed -= _levelData[_currentLevel].SpeedStep;
        _board.SetBlockOnTileMap(controlledBlock.GetCoordsAfterOffset());

        // Next block
        SpawnBlock();
    }

    private Block SpawnBlock()
    {
        // Instantiate the prefab and get the Block script
        GameObject blockGO = Instantiate(_blockPrefab);
        Block block = blockGO.GetComponent<Block>();

        // Pick block type and get corresponding coords
        int blockTypeInd = GetNextBlockType();
        Vector2Int[] coords = _blockCoordsDict[(BlockType)blockTypeInd];

        // Initialize the coords of block
        // the initial offset is at the next block display area
        block.Initialize(coords);

        // Display on the board
        _board.SetBlockOnTileMap(block.GetCoordsAfterOffset());

        _blocksQueue.Enqueue(block);
        return block;
    }

    private int GetNextBlockType()
    {
        // Let Block A prob = 1, Block B prob = 2, Block C prob = 3
        // Sum prob = 6
        // Then pick a random num between 1 to 6 inclusive
        // Say pick 3.
        // Then we subtract prob of block A, i.e. 3 - 1 => sum = 2,
        // Then subtract prob of block B, i.e. 2 - 2 => sum = 0
        // Therefore pick block B

        int sum = _levelData[_currentLevel].Probabilities.Sum();
        // Range from 1 to sum inclusive
        int num = Random.Range(1, sum + 1);

        int selectedBlockType = 0;

        for (int i = 0; i < _levelData[_currentLevel].Probabilities.Length ; i++)
        {
            num -= _levelData[_currentLevel].Probabilities[i];

            if (num < 0)
            {
                break;
            }
            selectedBlockType++;
        }

        return selectedBlockType;
    }

    public void IncrementScore()
    {
        _score += 1;

        if (_score >= _levelData[_currentLevel].WiningScore)
        {
            _showWinScreenEvent.Invoke();
        }
    }
}

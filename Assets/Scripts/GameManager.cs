using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private BlockController _controller;
    private Dictionary<BlockType, Vector2Int[]> _blockCoordsDict;
    private Board _board;
    private Queue<Block> _blocksQueue = new Queue<Block>();
    private float score;
    [SerializeField] private GameObject _blockPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        _blockCoordsDict = GetComponent<BlockData>().BlockDataDict;
        _controller = GetComponent<BlockController>();
        _board = FindFirstObjectByType<Board>();
        SpawnBlock();
        NextRound();
        Debug.Log("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NextRound()
    {
        // Get next block and clear block from next block area
        Block controlledBlock = _blocksQueue.Dequeue();
        _board.SetBlockOnTileMap(controlledBlock.GetCoordsAfterOffset(), toClear: true);

        // Spawn at top of board
        controlledBlock.SetPivotToSpawnLocation();
        _controller.ControlledBlock = controlledBlock;
        _board.SetBlockOnTileMap(controlledBlock.GetCoordsAfterOffset());

        // Next block
        SpawnBlock();
    }

    public Block SpawnBlock()
    {
        // Instantiate the prefab and get the Block script
        GameObject blockGO = Instantiate(_blockPrefab);
        Block block = blockGO.GetComponent<Block>();

        // Randomly pick which block type and get corresponding coords
        int blockTypeInd = Random.Range(0, _blockCoordsDict.Count);
        Vector2Int[] coords = _blockCoordsDict[(BlockType)blockTypeInd];

        // Initialize the coords of block
        // the initial offset is at the next block display area
        block.Initialize(coords);

        // Display on the board
        _board.SetBlockOnTileMap(block.GetCoordsAfterOffset());

        _blocksQueue.Enqueue(block);
        return block;
    }
}

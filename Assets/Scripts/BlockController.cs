using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Block _controlledBlock;
    private float _lastMoveTime;
    private float _dropSpeed;

    public Block ControlledBlock 
    { 
        get => _controlledBlock; 
        set => _controlledBlock = value; 
    }

    public float DropSpeed
    {
        get => _dropSpeed;
        set => _dropSpeed = value;
    }

    void Start()
    {
        _lastMoveTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_controlledBlock == null)
            return;

        if (Time.time - _lastMoveTime >= _dropSpeed ||
            Input.GetKeyDown(KeyCode.S))
        // if (Input.GetKeyDown(KeyCode.S))
        {
            _controlledBlock.Translate(Vector2Int.down);
            _lastMoveTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _controlledBlock.Translate(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _controlledBlock.Translate(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _controlledBlock.Rotate();
        }
    }
}

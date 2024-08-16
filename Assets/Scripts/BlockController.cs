using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Block _controlledBlock;
    private float _lastMoveTime;
    private float _speedStep = 0.5f;

    public Block ControlledBlock 
    { 
        get => _controlledBlock; 
        set => _controlledBlock = value; 
    }

    void Start()
    {
        _lastMoveTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastMoveTime >= _speedStep || 
            Input.GetKeyDown(KeyCode.S))
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
    }
}

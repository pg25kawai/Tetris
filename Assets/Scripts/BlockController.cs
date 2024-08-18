using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Block _controlledBlock;
    private float _lastDropTime;
    private float _lastMoveTime;
    private float _lastPressTime;
    private float _dropCooldown;
    private float _moveCooldown = 0.1f;
    private bool _holding;

    public Block ControlledBlock 
    { 
        get => _controlledBlock; 
        set => _controlledBlock = value; 
    }

    public float DropCooldown
    {
        get => _dropCooldown;
        set => _dropCooldown = value;
    }

    void Start()
    {
        _lastDropTime = Time.time;
        _lastMoveTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_controlledBlock == null)
            return;

        bool coolDownCondition = 
            Time.time - _lastMoveTime >= _moveCooldown;

        bool holdThresholdCondition = 
            _holding && Time.time - _lastPressTime >= _moveCooldown * 2;

        bool autoDropCondition =
            Time.time - _lastDropTime >= _dropCooldown;

        if (autoDropCondition)
        {
            _controlledBlock.Translate(Vector2Int.down);
            _lastDropTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _holding = true;
            _lastPressTime = Time.time;
            _controlledBlock.Translate(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _holding = true;
            _lastPressTime = Time.time;
            _controlledBlock.Translate(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _holding = true;
            _lastPressTime = Time.time;
            _controlledBlock.Translate(Vector2Int.right);
        }

        // Key released
        if (Input.GetKeyUp(KeyCode.A) &&
            Input.GetKeyUp(KeyCode.D) && 
            Input.GetKeyUp(KeyCode.S))
        {
            _holding = false;   
        }

        // If holding and after initial hold delay
        // Apply burst translate
        if (holdThresholdCondition && coolDownCondition)
        {
            if (Input.GetKey(KeyCode.S))
                _controlledBlock.Translate(Vector2Int.down);
            if (Input.GetKey(KeyCode.A))
                _controlledBlock.Translate(Vector2Int.left);
            if (Input.GetKey(KeyCode.D))
                _controlledBlock.Translate(Vector2Int.right);
            _lastMoveTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _controlledBlock.Rotate();
        }
    }
}

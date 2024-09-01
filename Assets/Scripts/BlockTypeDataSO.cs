using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block", menuName = "Block/New Block Type ID")]

public class BlockType : ScriptableObject
{
    public char UID;
    public Vector2Int[] Coords;
}

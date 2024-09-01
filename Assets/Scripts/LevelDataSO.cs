using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelParams", menuName = "Level/New Level Parameters")]
public class LevelDataSO : ScriptableObject
{
    public BlockType[] IncludedShapes;
    public int[] Probabilities;
    public int WiningScore;
    public float SpeedMin;
    public float SpeedMax;
    public float SpeedStep;
}

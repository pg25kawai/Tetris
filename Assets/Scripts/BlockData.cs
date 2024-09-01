using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class BlockDataReader : MonoBehaviour
{
    public void GetAllBlockData(string sourceDirectory)
    {
        // Read all text files block data from sourceDirectory
        var txtFiles = Directory.EnumerateFiles(sourceDirectory, "*.txt");
        foreach (string currentFile in txtFiles)
        {
            ReadDataFromTextFile(currentFile);
        }
    }

    private void ReadDataFromTextFile(string filePath)
    {
        // XIdx increases from left to right
        int XIdx = 0;
        // YIdx decreases from top to bottom
        int YIdx = 0;
        // 4 cells for each block
        int cellIdx = 0;
        Vector2Int[] coords = new Vector2Int[4];

        int pivotXIdx = 0;
        int pivotYIdx = 0;

        StreamReader sr = new StreamReader(filePath);
        // First letter is block type
        char UID = sr.ReadLine()[0];
        UID = Char.ToUpper(UID);

        string line;
        line = sr.ReadLine();

        while (line != null)
        {
            XIdx = 0;
            foreach (char c in line)
            {
                if (c != '_')
                {
                    // If pivot
                    if (c == 'O')
                    {
                        pivotXIdx = XIdx;
                        pivotYIdx = YIdx;
                    }
                    coords[cellIdx] = new Vector2Int(XIdx, YIdx);
                    cellIdx++;
                }
                XIdx++;
            }
            // Read the next line
            line = sr.ReadLine();
            YIdx--;
        }
        // Close the file
        sr.Close();

        SetPivotToOrigin(pivotXIdx, pivotYIdx, coords);
        AddBlockToData(UID, coords);
    }

    private void SetPivotToOrigin(int pivotXCoord, int pivotYCoord, Vector2Int[] allCoords)
    {
        // Shift all coords such that pivot is at (0, 0)
        for (int i = 0;  i < allCoords.Length; i++)
        {
            allCoords[i].x -= pivotXCoord;
            allCoords[i].y -= pivotYCoord;
        }
    }

    private void AddBlockToData(char UID, Vector2Int[] allCoords)
    {
        // Create scriptable object of blocks
        BlockType newBlock = ScriptableObject.CreateInstance<BlockType>();
        newBlock.UID = UID;
        newBlock.Coords = allCoords;

        string blockName = "Block" + UID + ".asset";

        AssetDatabase.CreateAsset(newBlock, "Assets/Data/SOBlockData/" + blockName);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = newBlock;
    }
}

// Click button in inspector to read all block from txt data
[CustomEditor(typeof(BlockDataReader))]
class BlockDataReaderHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BlockDataReader script = (BlockDataReader)target;

        if (GUILayout.Button("Read Blocks"))
            script.GetAllBlockData("Assets/Data/TxtBlockData/");
    }
}
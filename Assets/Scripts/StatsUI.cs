using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    private TextMeshProUGUI _currentLevelText;
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _currentLevelText = 
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        _scoreText = 
            transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ChangeLevelDisplay(string level)
    {
        _currentLevelText.text = level;
    }

    public void ChangeScoreDisplay(string score) 
    { 
        _scoreText.text = score;
    }
}

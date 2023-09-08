using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreVisualizer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ScoreManager manager;
    [SerializeField] int digits = 10;
    private void Start()
    {
        manager.onAnyScoreUp += delegate (float score)
        {
            scoreText.text = Mathf.RoundToInt(score).ToString("D"+digits);
        };
    }
}

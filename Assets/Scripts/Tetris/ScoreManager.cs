using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void ScoreUpEvent(float score);
    public ScoreUpEvent onTetrisEvent, onTSpinEvent, onTSpinMiniEvent, onISpinEvent, onBackToBackEvent, onBasicScoreUp;
    public ScoreUpEvent onAnyScoreUp;

    private static readonly int[] easyMoveScoreTable =
    {
        100,
        300,
        500,
        800
    };
    private static readonly int[] difficultMoveScoreTable =
    {
        400,    //T or I Spin No Lines
        800,    //T or I Spin Single
        1200,   //T or I Spin Double
        1600    //T or I Spin Triple
    };
    private static readonly int[] easyDifficultMoveScoreTable = 
    {
        100,    //T Spin Mini No Lines
        200,    //T Spin Mini Single
        400     //T Spin Mini Double
    };
    private static readonly int softDropRewardPerLine = 1;
    private static readonly int hardDropRewardPerLine = 2;
    private static readonly float backToBackRewardMultiplier = 1.5f;

    [SerializeField] float myScore = 0;
    [SerializeField] bool wasDifficultMove = false;

    void AddScore(float scoreToAdd, bool bInvokeEvent = false)
    {
        myScore += scoreToAdd;
        onAnyScoreUp?.Invoke(myScore);
        if(bInvokeEvent)
            onBasicScoreUp?.Invoke(scoreToAdd);
    }

    internal void SoftDrop() => AddScore(softDropRewardPerLine, true);
    internal void HardDrop(int distance) => AddScore(distance * hardDropRewardPerLine, true);

    internal void LineClear(int numClearedLines)
    {
        int scoreToAdd = easyMoveScoreTable[numClearedLines-1];
        if (numClearedLines == 4)
        {
            if (TryGetBackToBackBonus(scoreToAdd))
                return;
            onTetrisEvent?.Invoke(scoreToAdd);
            AddScore(scoreToAdd, true);
        }
        else
        {
            AddScore(scoreToAdd, true);
            wasDifficultMove = false;
        }
    }

    internal void ISpin(int numClearedLines)
    {
        int scoreToAdd = difficultMoveScoreTable[numClearedLines];
        if (numClearedLines != 0)
        {
            if (TryGetBackToBackBonus(scoreToAdd))
                return;
        }
        else wasDifficultMove = false;
        AddScore(scoreToAdd);
        onISpinEvent?.Invoke(scoreToAdd);
    }

    internal void TSpin(int numClearedLines)
    {
        int scoreToAdd = difficultMoveScoreTable[numClearedLines];
        if (numClearedLines != 0)
        {
            if (TryGetBackToBackBonus(scoreToAdd))
                return;
        }
        else wasDifficultMove = false;
        AddScore(scoreToAdd);
        onTSpinEvent?.Invoke(scoreToAdd);
    }

    internal void TSpinMini(int numClearedLines)
    {
        int scoreToAdd = easyDifficultMoveScoreTable[numClearedLines];
        if (numClearedLines != 0)
        {
            if (TryGetBackToBackBonus(scoreToAdd))
                return;
        }
        else wasDifficultMove = false;
        AddScore(scoreToAdd);
        onTSpinMiniEvent?.Invoke(scoreToAdd);
    }
    
    bool TryGetBackToBackBonus(int scoreToAdd)
    {
        if (wasDifficultMove)
        {
            var scoreToAddBackToBack = scoreToAdd * backToBackRewardMultiplier;
            AddScore(scoreToAddBackToBack);
            onBackToBackEvent?.Invoke(scoreToAddBackToBack);
            return true;
        }
        wasDifficultMove = true;
        return false;
    }
}
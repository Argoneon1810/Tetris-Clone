using UnityEngine;

public class TetrisManager : MonoBehaviour
{
    private const int SRS_MAX_TRIAL = 4;

    public delegate void NoParamReturnEvent();
    public NoParamReturnEvent onLiveMinoStop, onDefeat;

    [SerializeField] int tickCountSinceLastMove = 0;
    [SerializeField] int forceMoveEveryNTick = 30;

    [SerializeField] TetrisGridMediator tetrisGridMediator;
    [SerializeField] SubTetrisGridMediator holdGridMediator;
    [SerializeField] SubTetrisGridMediator[] nextGridMediators;
    [SerializeField] ScoreManager scoreManager;

    private IMino liveMino, heldMino;
    public IMino LiveMino => liveMino;
    public IMino HeldMino => heldMino;

    bool wasTSpin = false, wasTSpinMini = false, wasISpin = false;
    bool lost = false;

    [SerializeField] int numNextBlock = 5;
    int numNextGridMediatorsReady = 0;
    public SevenBagSeries sbs;
    IMino[] lastNexts;

    public Vector2Int SpawnPosition;

    private void Awake()
    {
        sbs = new SevenBagSeries();
    }

    private void Start()
    {
        TickGenerator.Instance.onTickEvent += OnTick;
        tetrisGridMediator.onPostGridGeneration += SpawnNext;
        foreach (var ngm in nextGridMediators)
            ngm.onPostGridGeneration += LoadNextsWhenReady;

        //Debug Console Prints
        scoreManager.onBackToBackEvent += delegate (float score) { Debug.Log("Back To Back : " + score); };
        scoreManager.onTetrisEvent += delegate (float score) { Debug.Log("Tetris : " + score); };
        scoreManager.onISpinEvent += delegate (float score) { Debug.Log("I Spin : " + score); };
        scoreManager.onTSpinEvent += delegate (float score) { Debug.Log("T Spin : " + score); };
        scoreManager.onTSpinMiniEvent += delegate (float score) { Debug.Log("T Spin Mini : " + score); };
    }

    void OnTick(float delta)
    {
        if (tickCountSinceLastMove >= forceMoveEveryNTick)
        {
            tickCountSinceLastMove = 0;
            MoveDown();
        }
        else ++tickCountSinceLastMove;
    }

    internal void ResetTimer()
    {
        tickCountSinceLastMove = 0;
    }

    void Swap(ref IMino a, ref IMino b)
    {
        var temp = a;
        a = b;
        b = temp;
    }

    void ClearFlags()
    {
        wasISpin = wasTSpin = wasTSpinMini = false;
    }

    private void SpawnNext() => SpawnAt(sbs.GetNext(), SpawnPosition);
    void SpawnAt(IMino mino, Vector2Int spawnPosition, bool resetRotation = true)
    {
        if (lost) return;
        if (!tetrisGridMediator.HasSpaceForNext(mino, spawnPosition))
        {
            Lose();
            return;
        }
        mino.SpawnAt(spawnPosition, resetRotation);
        tetrisGridMediator.Spawn(mino);
        liveMino = mino;
        if (numNextGridMediatorsReady >= numNextBlock)
            LoadNexts();
    }

    IMino[] GetNextBlocks(int length) => sbs.PeekNext(length);
    void LoadNextsWhenReady()
    {
        if (++numNextGridMediatorsReady >= numNextBlock)
            LoadNexts();
    }
    void LoadNexts()
    {
        IMino[] nexts = GetNextBlocks(numNextBlock);
        for (int i = 0; i < numNextBlock; ++i)
        {
            if (lastNexts != null)
            {
                if (lastNexts.Length > 0)
                {
                    nextGridMediators[i].Erase(lastNexts[i]);
                }
            }
            nextGridMediators[i].Draw(nexts[i]);
        }
        lastNexts = nexts;
    }

    private void TestPlayfieldOverflow()
    {
        if (tetrisGridMediator.IsVanishingZoneFilled())
            Lose();
    }
    private void Lose()
    {
        onDefeat?.Invoke();
        Debug.Log("LOST!!");
        lost = true;
    }

    private void TestLineFill(IMino stoppedBlock)
    {
        int numClearedLines = tetrisGridMediator.ClearFilledLines(stoppedBlock);
        if (wasTSpinMini)
        {
            wasTSpinMini = false;
            scoreManager.TSpinMini(numClearedLines);
        }
        else if (wasTSpin)
        {
            wasTSpin = false;
            scoreManager.TSpin(numClearedLines);
        }
        else if (wasISpin)
        {
            wasISpin = false;
            scoreManager.ISpin(numClearedLines);
        }
        else if (numClearedLines > 0)
            scoreManager.LineClear(numClearedLines);
    }

    void PostBlockStopSequence(IMino stoppedBlock)
    {
        onLiveMinoStop?.Invoke();
        TestLineFill(stoppedBlock);
        TestPlayfieldOverflow();
        SpawnNext();    //tests spawn area availability internally
    }

    internal void MoveLeft()
    {
        if (liveMino == null) return;
        if (tetrisGridMediator.TryMove(liveMino, Vector2Int.left))
        {
            //play some booming sound
        }
        else
        {
            ClearFlags();
            liveMino.MoveLeft();
        }
    }

    internal void MoveRight()
    {
        if (liveMino == null) return;
        if (tetrisGridMediator.TryMove(liveMino, Vector2Int.right))
        {
            //play some booming sound
        }
        else
        {
            ClearFlags();
            liveMino.MoveRight();
        }
    }

    internal void MoveDown()
    {
        if (liveMino == null) return;
        if (tetrisGridMediator.TryMove(liveMino, Vector2Int.down))
        {
            var stoppedMino = liveMino;
            liveMino = null;
            PostBlockStopSequence(stoppedMino);
        }
        else
        {
            ClearFlags();
            liveMino.MoveDown();
        }
    }

    internal void SoftDrop()
    {
        MoveDown();
        scoreManager.SoftDrop();
    }

    internal void HardDrop()
    {
        if (liveMino == null) return;
        ClearFlags();
        int distance = tetrisGridMediator.DoAHardDrop(liveMino);
        scoreManager.HardDrop(distance);
        liveMino.MoveDown(distance);
        var stoppedMino = liveMino;
        liveMino = null;
        PostBlockStopSequence(stoppedMino);
    }

    void DetermineRewardedSpins(Pair<int, bool> rotateResult)
    {
        //회전했으므로 T스핀 및 I스핀 조건 1 달성
        bool isWallKick = 0 < rotateResult.First && SRS_MAX_TRIAL >= rotateResult.First;
        bool isThreeCorner = rotateResult.Second;
        if (isWallKick && liveMino is Mino_I)   //I스핀 조건 #2
        {
            if (wasISpin)
                scoreManager.ISpin(0);          //이미 I스핀이었을 경우 I Spin No Lines 트리거
            wasISpin = true;
        }
        else if (isThreeCorner)                 //T스핀 조건 #2
        {
            if (isWallKick)
            {
                if (wasTSpinMini)
                    scoreManager.TSpinMini(0);  //이미 T스핀 미니였을 경우 T Spin Mini No Lines 트리거
                wasTSpinMini = true;            //T스핀 미니 (Easy T-Spin) 조건
            }
            else
            {
                if (wasTSpin)
                    scoreManager.TSpin(0);      //이미 T스핀이었을 경우 T Spin No Lines 트리거
                wasTSpin = true;                //T스핀 미니 조건이 아니므로, T스핀
            }
        }
    }

    internal void RotateLeft()
    {
        if (liveMino == null) return;
        var result = tetrisGridMediator.TryRotateLeft(liveMino);
        if (result.First > SRS_MAX_TRIAL)
        {
            //play some booming sound
        }
        else
        {
            DetermineRewardedSpins(result);
            liveMino.RotateLeft(result.First);
        }
    }

    internal void RotateRight()
    {
        if (liveMino == null) return;
        var result = tetrisGridMediator.TryRotateRight(liveMino);
        if (result.First > SRS_MAX_TRIAL)
        {
            //play some booming sound
        }
        else
        {
            DetermineRewardedSpins(result);
            liveMino.RotateRight(result.First);
        }
    }

    internal void Hold()
    {
        if (liveMino == null) return;
        tetrisGridMediator.Despawn(liveMino);
        holdGridMediator.Erase(heldMino);
        holdGridMediator.Draw(liveMino);
        Swap(ref liveMino, ref heldMino);
        if (liveMino == null) SpawnNext();
        else SpawnAt(liveMino, SpawnPosition, false);
    }
}
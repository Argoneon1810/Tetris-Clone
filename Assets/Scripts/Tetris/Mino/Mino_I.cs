using UnityEngine;

public class Mino_I : WallKick_I, IMino
{
    private const int NUM_MINO = 4;

    private static readonly Vector2[][] cells =
    {
        new [] {    //회전 상태가 B일 때
            new Vector2(-1.5f, .5f),
            new Vector2(-.5f, .5f),
            new Vector2(.5f, .5f),
            new Vector2(1.5f, .5f)
        },
        new [] {    //회전 상태가 L일 때
            new Vector2(-.5f, -1.5f),
            new Vector2(-.5f, -.5f),
            new Vector2(-.5f, .5f),
            new Vector2(-.5f, 1.5f)
        },
        new [] {    //회전 상태가 T일 때
            new Vector2 (1.5f, -.5f),
            new Vector2 (.5f, -.5f),
            new Vector2 (-.5f, -.5f),
            new Vector2 (-1.5f, -.5f)
        },
        new [] {    //회전 상태가 R일 때
            new Vector2(.5f, 1.5f),
            new Vector2(.5f, .5f),
            new Vector2(.5f, -.5f),
            new Vector2(.5f, -1.5f)
        }
    };
    private static readonly int[][] bottomSideIndices =
    {
        new [] { 0, 1, 2, 3 },      //회전 상태가 B일 때
        new [] { 0 },               //회전 상태가 L일 때
        new [] { 0, 1, 2, 3 },      //회전 상태가 T일 때
        new [] { 3 },               //회전 상태가 R일 때
    };

    private Vector2Int position;
    private Vector2 rotationPivotOffset = new Vector2(.5f, -.5f);
    private IMino.Rotation currentRotationState;

    private Color color;
    public Color GetColor() => color;

    public Mino_I(Color color)
    {
        this.color = color;
    }

    public void SpawnAt(Vector2Int pos, bool resetRotation = true)
    {
        position = pos;
        if(resetRotation) currentRotationState = IMino.Rotation.B;
    }

    public Vector2Int[] GetCellsRelativePosition()
    {
        int rotationIndex = (int)currentRotationState;
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO];
        Vector2 temp;
        Vector2Int temp2;
        for (int i = 0; i < cells[rotationIndex].Length; i++)
        {
            temp = cells[rotationIndex][i] + rotationPivotOffset;
            temp2 = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
            toReturn[i] = temp2;
        }
        return toReturn;
    }

    public Vector2Int[] GetCellsGlobalPositionBottomSided()
    {
        int rotationIndex = (int)currentRotationState;
        int[] currentBottomSidedIndices = bottomSideIndices[rotationIndex];
        Vector2Int[] toReturn = new Vector2Int[currentBottomSidedIndices.Length];
        int index = 0;
        Vector2 temp;
        Vector2Int temp2;
        foreach (var bottomSideIndex in currentBottomSidedIndices)
        {
            temp = cells[rotationIndex][bottomSideIndex] + rotationPivotOffset;
            temp2 = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
            toReturn[index++] = temp2 + position;
        }
        return toReturn;
    }

    public Vector2Int[] GetCellsGlobalPosition() => GetCellsGlobalPositionOfGivenRotationIndex((int)currentRotationState);
    public Vector2Int[] GetCellsGlobalPositionLeftTurned() => GetCellsGlobalPositionOfGivenRotationIndex((int)currentRotationState.GetLeft());
    public Vector2Int[] GetCellsGlobalPositionRightTurned() => GetCellsGlobalPositionOfGivenRotationIndex((int)currentRotationState.GetRight());
    public Vector2Int[] GetCellsGlobalPositionOfGivenRotationIndex(int rotationIndex)
    {
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO];
        Vector2 temp;
        Vector2Int temp2;
        for (int i = 0; i < cells[rotationIndex].Length; i++)
        {
            temp = cells[rotationIndex][i] + rotationPivotOffset;
            temp2 = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
            toReturn[i] = temp2 + position;
        }
        return toReturn;
    }

    public Vector2Int[] GetSRSOffsets(IMino.Rotation orig, IMino.Rotation target) => srsOffsets[(int)orig][(int)target];

    public void MoveDown() => MoveDown(1);
    public void MoveDown(int distance) => position += Vector2Int.down * distance;
    public void MoveLeft() => position += Vector2Int.left;
    public void MoveRight() => position += Vector2Int.right;

    public bool IsRotatable() => true;
    public IMino.Rotation GetCurrentRotation() => currentRotationState;
    public void RotateLeft(int srsIndex)
    {
        var from = currentRotationState;
        var to = currentRotationState.GetLeft();
        position += srsOffsets[(int)from][(int)to][srsIndex];
        currentRotationState = to;
    }

    public void RotateRight(int srsIndex)
    {
        var from = currentRotationState;
        var to = currentRotationState.GetRight();
        position += srsOffsets[(int)from][(int)to][srsIndex];
        currentRotationState = to;
    }
}
using UnityEngine;

public class Mino_L : WallKick_Other, IMino
{
    private const int NUM_MINO = 4;

    private static readonly Vector2Int[][] cells =
    {
        new [] {    //ȸ�� ���°� B�� ��
            Vector2Int.left,
            Vector2Int.zero,
            Vector2Int.right,
            new Vector2Int(1, 1)
        },
        new [] {    //ȸ�� ���°� L�� ��
            Vector2Int.down,
            Vector2Int.zero,
            Vector2Int.up,
            new Vector2Int(-1, 1)
        },
        new [] {    //ȸ�� ���°� T�� ��
            Vector2Int.right,
            Vector2Int.zero,
            Vector2Int.left,
            new Vector2Int(-1, -1)
        },
        new [] {    //ȸ�� ���°� R�� ��
            Vector2Int.up,
            Vector2Int.zero,
            Vector2Int.down,
            new Vector2Int(1, -1)
        }
    };
    private static readonly int[][] bottomSideIndices =
    {
        new [] { 0, 1, 2 },         //ȸ�� ���°� B�� ��
        new [] { 0, 3 },            //ȸ�� ���°� L�� ��
        new [] { 0, 1, 3 },         //ȸ�� ���°� T�� ��
        new [] { 2, 3 }             //ȸ�� ���°� R�� ��
    };

    public Vector2Int position;
    public IMino.Rotation currentRotationState;

    private Color color;
    public Color GetColor() => color;

    public Mino_L(Color color)
    {
        this.color = color;
    }

    public void SpawnAt(Vector2Int pos, bool resetRotation = true)
    {
        position = pos;
        if (resetRotation) currentRotationState = IMino.Rotation.B;
    }

    public Vector2Int[] GetCellsRelativePosition()
    {
        int rotationIndex = (int)currentRotationState;
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO];
        for (int i = 0; i < cells[rotationIndex].Length; i++)
            toReturn[i] = cells[rotationIndex][i];
        return toReturn;
    }

    public Vector2Int[] GetCellsGlobalPositionBottomSided()
    {
        int rotationIndex = (int)currentRotationState;
        int[] currentBottomSidedIndices = bottomSideIndices[rotationIndex];
        Vector2Int[] toReturn = new Vector2Int[currentBottomSidedIndices.Length];
        int index = 0;
        foreach (var bottomSideIndex in currentBottomSidedIndices)
            toReturn[index++] = cells[rotationIndex][bottomSideIndex] + position;
        return toReturn;
    }
    public Vector2Int[] GetCellsGlobalPosition() => GetCellsGlobalPositionOfGivenRotationIndex((int)currentRotationState);
    public Vector2Int[] GetCellsGlobalPositionLeftTurned() => GetCellsGlobalPositionOfGivenRotationIndex((int)currentRotationState.GetLeft());
    public Vector2Int[] GetCellsGlobalPositionRightTurned() => GetCellsGlobalPositionOfGivenRotationIndex((int)currentRotationState.GetRight());
    public Vector2Int[] GetCellsGlobalPositionOfGivenRotationIndex(int rotationIndex)
    {
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO];
        for (int i = 0; i < cells[rotationIndex].Length; i++)
            toReturn[i] = cells[rotationIndex][i] + position;
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

using UnityEngine;

public class Mino_T : WallKick_Other, IMino
{
    private const int NUM_MINO = 4;

    private static readonly Vector2Int[][] cells =
    {
        new [] {    //회전 상태가 B일 때
            Vector2Int.left,
            Vector2Int.zero,
            Vector2Int.up,
            Vector2Int.right
        },
        new [] {    //회전 상태가 L일 때
            Vector2Int.down,
            Vector2Int.zero,
            Vector2Int.left,
            Vector2Int.up
        },
        new [] {    //회전 상태가 T일 때
            Vector2Int.right,
            Vector2Int.zero,
            Vector2Int.down,
            Vector2Int.left
        },
        new [] {    //회전 상태가 R일 때
            Vector2Int.up,
            Vector2Int.zero,
            Vector2Int.right,
            Vector2Int.down
        }
    };
    private static readonly int[][] bottomSideIndices =
    {
        new [] { 0, 1, 3 },         //회전 상태가 B일 때
        new [] { 0, 2 },            //회전 상태가 L일 때
        new [] { 0, 2, 3 },         //회전 상태가 T일 때
        new [] { 2, 3 }             //회전 상태가 R일 때
    };
    private static readonly Vector2Int[][] threeCornersMustFill =
    {
        new Vector2Int[] { new (-1, 1), new (1, 1) },
        new Vector2Int[] { new (-1, 1), new (-1, -1)},
        new Vector2Int[] { new (1, -1), new (-1, -1)},
        new Vector2Int[] { new (1, -1), new (1, 1)}
    };
    private static readonly Vector2Int[][] threeCornersEitherOne =
    {
        new Vector2Int[] { new (-1, -1), new (1, -1) },
        new Vector2Int[] { new (1, 1), new (1, -1)},
        new Vector2Int[] { new (1, 1), new (-1, 1)},
        new Vector2Int[] { new (-1, -1), new (-1, 1)}
    };

    public Vector2Int position;
    public IMino.Rotation currentRotationState;

    private Color color;
    public Color GetColor() => color;

    public Mino_T(Color color)
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
    /// <summary>
    /// get three corner offset of current rotation state.
    /// </summary>
    /// <returns>
    /// pair of two Vector2Int array: first is about must-fill two corners, and second is about either-one-filled two corners
    /// </returns>
    public Pair<Vector2Int[], Vector2Int[]> GetThreeCornerOffsets() => new Pair<Vector2Int[], Vector2Int[]>(threeCornersMustFill[(int)currentRotationState], threeCornersEitherOne[(int)currentRotationState]);

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

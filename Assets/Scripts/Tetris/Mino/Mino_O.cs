using UnityEngine;

[System.Serializable]
public class Mino_O : IMino
{
    private const int NUM_MINO = 4;

    private static readonly Vector2[] cells = {
        new Vector2(-.5f, .5f),
        new Vector2(.5f, .5f),
        new Vector2(.5f, -.5f),
        new Vector2(-.5f, -.5f)
    };

    public Vector2Int position;
    private Vector2 rotationPivotOffset = new Vector2(.5f, .5f);

    private Color color;
    public Color GetColor() => color;

    public Mino_O(Color color)
    {
        this.color = color;
    }

    public void SpawnAt(Vector2Int pos, bool resetRotation = true)
    {
        position = pos;
    }

    public Vector2Int[] GetCellsRelativePosition()
    {
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO];
        Vector2 temp;
        Vector2Int temp2;
        for (int i = 0; i < cells.Length; i++)
        {
            temp = cells[i] + rotationPivotOffset;
            temp2 = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
            toReturn[i] = temp2;
        }
        return toReturn;
    }

    public Vector2Int[] GetCellsGlobalPositionBottomSided()
    {
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO / 2];
        Vector2 temp;
        Vector2Int temp2;
        for (int i = 0; i < NUM_MINO/2; i++)
        {
            temp = cells[i + 2] + rotationPivotOffset;
            temp2 = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
            toReturn[i] = temp2 + position;
        }
        return toReturn;
    }

    public Vector2Int[] GetCellsGlobalPosition()
    {
        Vector2Int[] toReturn = new Vector2Int[NUM_MINO];
        Vector2 temp;
        Vector2Int temp2;
        for (int i = 0; i < cells.Length; i++)
        {
            temp = cells[i] + rotationPivotOffset;
            temp2 = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
            toReturn[i] = temp2 + position;
        }
        return toReturn;
    }
    public Vector2Int[] GetCellsGlobalPositionLeftTurned() => GetCellsGlobalPosition();
    public Vector2Int[] GetCellsGlobalPositionRightTurned() => GetCellsGlobalPosition();
    public Vector2Int[] GetCellsGlobalPositionOfGivenRotationIndex(int rotationIndex) => GetCellsGlobalPosition();

    public Vector2Int[] GetSRSOffsets(IMino.Rotation orig, IMino.Rotation target)
    {
        return null;
    }

    public void MoveDown() => MoveDown(1);
    public void MoveDown(int distance) => position += Vector2Int.down * distance;
    public void MoveLeft() => position += Vector2Int.left;
    public void MoveRight() => position += Vector2Int.right;

    public bool IsRotatable() => false;
    public IMino.Rotation GetCurrentRotation() => IMino.Rotation.B;
    public void RotateLeft(int srsIndex) { }
    public void RotateRight(int srsIndex) { }
}
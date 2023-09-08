using UnityEngine;

public interface IMino
{
    public enum Rotation
    {
        B = 0,  //bottom, O in tetris guideline
        L = 1,  //left
        T = 2,  //top, 2 in tetris guideline
        R = 3   //right
    }
    public void SpawnAt(Vector2Int pos, bool resetRotation = true);
    public Vector2Int[] GetCellsRelativePosition();
    public Vector2Int[] GetCellsGlobalPosition();
    public Vector2Int[] GetCellsGlobalPositionBottomSided();
    public Vector2Int[] GetCellsGlobalPositionLeftTurned();
    public Vector2Int[] GetCellsGlobalPositionRightTurned();
    public Vector2Int[] GetCellsGlobalPositionOfGivenRotationIndex(int rotationIndex);
    public Vector2Int[] GetSRSOffsets(Rotation orig, Rotation target);
    public void MoveDown();
    public void MoveDown(int distance);
    public void MoveLeft();
    public void MoveRight();
    public bool IsRotatable();
    public Rotation GetCurrentRotation();
    public void RotateLeft(int srsIndex);
    public void RotateRight(int srsIndex);
    public Color GetColor();
}
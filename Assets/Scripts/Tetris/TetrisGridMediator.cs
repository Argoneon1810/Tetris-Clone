using System;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGridMediator : MonoBehaviour
{
    private const int SRS_MAX_TRIAL = 4;

    public delegate void PostGridGenerationEvent();
    public PostGridGenerationEvent onPostGridGeneration;

    public CellGrid targetGrid;

    void Start()
    {
        targetGrid.onGridGenerationDone += OnPostGridGeneration;
    }

    void OnPostGridGeneration()
    {
        onPostGridGeneration?.Invoke();
    }

    internal void Spawn(IMino mino)
    {
        var cells = mino.GetCellsGlobalPosition();
        Color blockColor = mino.GetColor();
        foreach (var cell in cells)
            targetGrid.EnableBlock(cell, blockColor);
    }

    internal void Despawn(IMino mino)
    {
        var cells = mino.GetCellsGlobalPosition();
        Despawn(ref cells);
    }
    void Despawn(ref Vector2Int[] cells)
    {
        foreach (var cell in cells)
            targetGrid.DisableBlock(cell);
    }

    internal bool TryMove(IMino mino, Vector2Int direction)
    {
        Vector2Int[] cells = mino.GetCellsGlobalPosition();
        Color color = mino.GetColor();
        bool obstructed = false;

        //clear originals
        Despawn(ref cells);

        //check next positions
        foreach (var cell in cells)
        {
            if (targetGrid.IsActive(cell + direction))
            {
                obstructed = true;
                break;
            }
        }

        //if any obstacle found,
        if(obstructed)
            //redraw originals
            foreach (var cell in cells)
                targetGrid.EnableBlock(cell, color);
        //if no obstacle found,
        else
            //draw mino to next position
            foreach (var cell in cells)
                targetGrid.EnableBlock(cell + direction, color);

        //report result
        return obstructed;
    }

    /// <summary>
    /// </summary>
    /// <param name="mino"></param>
    /// <returns>distance travelled</returns>
    internal int DoAHardDrop(IMino mino)
    {
        Vector2Int[] cells = mino.GetCellsGlobalPosition();
        Vector2Int[] bottomCells = mino.GetCellsGlobalPositionBottomSided();
        Color color = mino.GetColor();

        Vector2Int direction = Vector2Int.down;

        //clear originals
        Despawn(ref cells);

        //check all next positions
        int obstacleHitDistance = 1;
        bool obstructed = false;
        while(!obstructed)
        {
            foreach (var cell in bottomCells)
            {
                if (targetGrid.IsActive(cell + direction * obstacleHitDistance))
                {
                    obstructed = true;
                    break;
                }
            }
            if (!obstructed) ++obstacleHitDistance;
        }

        //apply final position
        int distanceToTravel = obstacleHitDistance - 1;
        foreach (var cell in cells)
            targetGrid.EnableBlock(cell + direction * distanceToTravel, color);

        return distanceToTravel;
    }

    bool TestTSpin(Mino_T mino)
    {
        Vector2Int centre = mino.position;
        var result = mino.GetThreeCornerOffsets();
        var mustFill = result.First;
        var eitherOne = result.Second;
        foreach(var offset in mustFill)
            if (!targetGrid.IsActive(centre + offset))
                return false;
        bool filled = false;
        foreach (var offset in eitherOne)
            if (targetGrid.IsActive(centre + offset))
                filled = true;
        return filled;
    }

    internal Pair<int, bool> TryRotateLeft(IMino mino)
    {
        var c = mino.GetCellsGlobalPositionLeftTurned();
        var rs = mino.GetCurrentRotation();
        var srso = mino.GetSRSOffsets(rs, rs.GetLeft());
        bool isTSpin = false;
        if (mino is Mino_T)
            isTSpin = TestTSpin(mino as Mino_T);
        return new Pair<int, bool>(TryRotate(mino, ref c, ref srso), isTSpin);
    }

    internal Pair<int, bool> TryRotateRight(IMino mino)
    {
        var c = mino.GetCellsGlobalPositionRightTurned();
        var rs = mino.GetCurrentRotation();
        var srso = mino.GetSRSOffsets(rs, rs.GetRight());
        bool isTSpin = false;
        if (mino is Mino_T)
            isTSpin = TestTSpin(mino as Mino_T);
        return new Pair<int, bool>(TryRotate(mino, ref c, ref srso), isTSpin);
    }

    int TryRotate(IMino mino, ref Vector2Int[] rotatedCells, ref Vector2Int[] srsOffsets)
    {
        if (!mino.IsRotatable()) return SRS_MAX_TRIAL + 1;

        Vector2Int[] cells = mino.GetCellsGlobalPosition();
        Color color = mino.GetColor();

        //clear originals
        Despawn(ref cells);

        //try all 5 srs offsets
        int srsIndex = 0;
        bool obstructed = false;
        for (int i = 0; i <= SRS_MAX_TRIAL; ++i)
        {
            foreach (var cell in rotatedCells)
            {
                if (targetGrid.IsActive(cell + srsOffsets[i]))
                {
                    obstructed = true;
                    break;
                }
            }

            if (!obstructed) break;

            srsIndex++;
            obstructed = false;
        }

        //if any srs offset was valid, apply. if not, reset to original state
        if (srsIndex <= SRS_MAX_TRIAL)
            foreach (var cell in rotatedCells)
                targetGrid.EnableBlock(cell + srsOffsets[srsIndex], color);
        else
            foreach (var cell in cells)
                targetGrid.EnableBlock(cell, color);

        return srsIndex;
    }

    internal bool HasSpaceForNext(IMino mino, Vector2Int spawnPosition)
    {
        var cells = mino.GetCellsRelativePosition();
        bool obstructed = false;
        foreach(var cell in cells)
        {
            if(targetGrid.IsActive(cell + spawnPosition))
            {
                obstructed = true;
                break;
            }
        }
        if(obstructed) return false;
        return true;
    }

    internal int ClearFilledLines(IMino stoppedMino)
    {
        var cells = stoppedMino.GetCellsGlobalPosition();
        int minX = 0, maxX = Mathf.RoundToInt(targetGrid.Dimen.x);
        int minY = int.MaxValue, maxY = int.MinValue;
        foreach (var cell in cells)
        {
            minY = cell.y < minY ? cell.y : minY;
            maxY = cell.y > maxY ? cell.y : maxY;
        }

        bool empty;
        List<int> filledLineIndices = new List<int>();
        for (int y = minY; y <= maxY; ++y)
        {
            empty = false;
            for (int x = minX; x < maxX; ++x)
            {
                if (!targetGrid.IsActive(x, y))
                {
                    empty = true;
                    break;
                }
            }
            if (empty) continue;
            else filledLineIndices.Add(y);
        }
        if (filledLineIndices.Count <= 0) return 0;

        int cleared = 0;
        foreach (var lineIndex in filledLineIndices)
            targetGrid.RemoveLine(lineIndex - cleared++);

        return filledLineIndices.Count;
    }

    internal bool IsVanishingZoneFilled()
    {
        var dimen = targetGrid.Dimen;
        for(int x = 0; x < dimen.x; ++x)
            for(int y = 20; y < dimen.y; ++y)
                if(targetGrid.IsActive(x, y))
                    return true;
        return false;
    }
}
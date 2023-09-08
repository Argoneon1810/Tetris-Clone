using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    public delegate void OnGridGenerationDoneEvent();
    public OnGridGenerationDoneEvent onGridGenerationDone;

    [SerializeField] bool trigger;
    [SerializeField] GameObject CellPrefab;
    [SerializeField] Vector2 dimen;
    public Vector2 Dimen => dimen;
    [SerializeField] List<List<GameObject>> gridmap;
    [SerializeField] float baseDistance = 1;
    [SerializeField] float distanceMultiplier = 1;
    [SerializeField] float sizeMultiplier = 1;

    private void Awake()
    {
        gridmap = new List<List<GameObject>>();
        Generate();
    }

    void Update()
    {
        if (trigger.Trigger())
        {
            Reset();
            Generate();
        }
    }

    private void Reset()
    {
        if(gridmap == null)
            gridmap = new List<List<GameObject>>();
        else
        {
            List<GameObject> row;
            for(int i = gridmap.Count-1; i>=0; --i)
            {
                row = gridmap[i];
                for (int j = row.Count - 1; j >= 0; --j)
                {
                    Destroy(row[j]);
                }
                row.Clear();
            }
            gridmap.Clear();
        }
    }

    private void Generate()
    {
        List<GameObject> row;
        Vector3 pos = transform.position;
        for (int i = 0; i < dimen.x; ++i)
        {
            row = new List<GameObject>();
            for(int j = 0; j < dimen.y; ++j)
            {
                GameObject cell = Instantiate(CellPrefab);
                cell.transform.position = new Vector3(pos.x + baseDistance * distanceMultiplier * i, pos.y + baseDistance * distanceMultiplier * j, 0);
                cell.transform.localScale = Vector3.one * sizeMultiplier;
                cell.transform.SetParent(transform);
                row.Add(cell);
            }
            gridmap.Add(row);
        }
        onGridGenerationDone?.Invoke();
    }

    internal void EnableBlock(Vector2Int pos, Color color) => EnableBlock(pos.x, pos.y, color);
    internal void EnableBlock(int x, int y, Color color)
    {
        var cell = gridmap[x][y].GetComponent<CellViewHolder>();
        cell.SetState(true);
        cell.SetColor(color);
    }

    internal void DisableBlock(Vector2Int pos) => DisableBlock(pos.x, pos.y);
    internal void DisableBlock(int x, int y)
    {
        var cell = gridmap[x][y].GetComponent<CellViewHolder>();
        cell.SetState(false);
        cell.SetColor(Color.white);
    }

    public bool IsActive(Vector2Int pos) => IsActive(pos.x, pos.y);
    public bool IsActive(int x, int y)
    {
        if (x >= gridmap.Count || y >= gridmap[0].Count || x < 0 || y < 0)
            return true;
        return gridmap[x][y].GetComponent<CellViewHolder>().IsActive();
    }

    internal void RemoveLine(int lineIndex)
    {
        CellViewHolder cell, cellNext;
        for (int x = 0; x < dimen.x; ++x)
        {
            //from current line to top boundary, set current line state to one up
            for (int y = lineIndex; y < dimen.y - 1; ++y)
            {
                cell = gridmap[x][y].GetComponent<CellViewHolder>();
                cellNext = gridmap[x][y + 1].GetComponent<CellViewHolder>();
                cell.SetColor(cellNext.GetColor());
                cell.SetState(cellNext.IsActive());
            }
            //set last line (top boundary) to default state
            cell = gridmap[x][((int)dimen.y) - 1].GetComponent<CellViewHolder>();
            cell.SetColor(Color.white);
            cell.SetState(false);
        }
    }
}
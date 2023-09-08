using UnityEngine;

public class SubTetrisGridMediator : MonoBehaviour
{
    public CellGrid targetGrid;
    [SerializeField] private Vector2Int offset = Vector2Int.one * 2;

    public delegate void PostGridGenerationEvent();
    public PostGridGenerationEvent onPostGridGeneration;
    void Start()
    {
        targetGrid.onGridGenerationDone += OnPostGridGeneration;
    }

    void OnPostGridGeneration()
    {
        onPostGridGeneration?.Invoke();
    }

    internal void Erase(IMino toErase)
    {
        if (toErase == null) return;
        var cells = toErase.GetCellsRelativePosition();
        foreach (var cell in cells)
            targetGrid.DisableBlock(cell + offset);
    }

    internal void Draw(IMino toDraw)
    {
        var cells = toDraw.GetCellsRelativePosition();
        Color blockColor = toDraw.GetColor();
        foreach (var cell in cells)
            targetGrid.EnableBlock(cell + offset, blockColor);
    }
}
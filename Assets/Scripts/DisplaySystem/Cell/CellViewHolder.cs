using UnityEngine;

public class CellViewHolder : MonoBehaviour
{
    [SerializeField] public Cell cell;
    public CellView cellView;

    private void Awake()
    {
        if (!cell)
            cell = new Cell();
        if (!cellView)
        {
            cellView = gameObject.AddComponent<CellView>();
            cellView.ToActivate = new GameObject();
            cellView.ToActivate.name = "Placeholder";
        }
        cell.stateChangeEvent += cellView.OnStateChange;
    }
    public Color GetColor() => cellView.GetColor();
    public void SetColor(Color color) => cellView.SetColor(color);
    public void SetState(bool state) => cell.SetActive(state);
    public bool IsActive() => cell.state;
}
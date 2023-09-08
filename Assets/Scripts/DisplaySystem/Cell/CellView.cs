using UnityEngine;

public class CellView : MonoBehaviour
{
    public delegate void BreakEvent();
    public BreakEvent onBreak;

    public GameObject ToActivate;
    private Material material;

    private void Awake()
    {
        material = ToActivate.GetComponent<Renderer>().material;
    }

    public void OnStateChange(bool state)
    {
        ToActivate.SetActive(state);
        if (!state) onBreak?.Invoke();
    }

    internal void SetColor(Color color) => material.color = color;

    internal Color GetColor() => material.color;
}

[System.Serializable]
public class Cell
{
    public bool state;

    public delegate void OnChangeState(bool newState);
    public OnChangeState stateChangeEvent;

    public void SetActive(bool state)
    {
        this.state = state;
        stateChangeEvent?.Invoke(state);
    }

    public void Toggle()
    {
        state = !state;
        stateChangeEvent?.Invoke(state);
    }

    public static implicit operator bool(Cell cell)
    {
        if (cell == null) return false;
        return true;
    }
}

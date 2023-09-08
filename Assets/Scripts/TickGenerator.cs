using System.Collections;
using UnityEngine;

public class TickGenerator : MonoBehaviour
{
    public delegate void OnTickEvent(float deltaTime);
    public OnTickEvent onTickEvent;

    private static TickGenerator instance;
    public static TickGenerator Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        onTickEvent?.Invoke(Time.deltaTime);
    }
}
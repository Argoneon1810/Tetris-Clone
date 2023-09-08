using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    public float maxWidth = 512;
    public float fullfillTime = 1;
    public RectTransform fill;

    private void Start()
    {
        TickGenerator.Instance.onTickEvent += OnTick;
    }

    private void OnTick(float deltaTime)
    {
        var size = fill.sizeDelta;
        size.x += maxWidth / fullfillTime * deltaTime;
        if (size.x > maxWidth)
            size.x = 0;
        fill.sizeDelta = size;
    }
}

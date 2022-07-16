using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Gauge
{
    public float Current
    {
        get { return current; }
        set { current = Mathf.Clamp(value, float.MinValue, Max); }
    }
    [SerializeField] private float current;

    public float Max
    {
        get { return max; }
        set { max = Mathf.Clamp(value, 0, float.MaxValue); }
    }
    [SerializeField] private float max = 5;

    public Gauge(float max, float starting)
    {
        this.max = max;
        this.current = starting;
    }
}

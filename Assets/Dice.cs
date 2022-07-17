using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dice : MonoBehaviour
{
    public Rigidbody body;
    public Transform[] sides;

    public int currentValue{get => sides.Select((s,i) => (s,i)).OrderByDescending(e => e.s.position.y).Take(1).Single().i + 1;}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
public class UnitController : MonoBehaviour
{
    public Animator? anime;
    public UnityEvent<Vector3> onAttack = new();


    void Start()
    {
        if (anime == null) {
            Debug.LogError("Animator not found");
        }
    }

    private void Update() {
    }

    public void RangeAttack(Vector3 Target) {
        anime?.Play("RangeAttack");
        onAttack?.Invoke(Target);
    }
    public void MeleeAttack(Vector3 Target) {
        onAttack?.Invoke(Target);
    }

    void Defeat() {
        anime?.Play("Defeat");
    }
}

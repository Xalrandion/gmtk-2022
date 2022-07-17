using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
public class UnitController : MonoBehaviour
{
    public Animator? anime;
    public UnityEvent<Vector3> onAttack = new();
    public Transform? target;

    void Start()
    {
        if (anime == null) {
            Debug.LogError("Animator not found");
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Attack(target!.position);
        }
        if (Input.GetMouseButtonDown(1)) {
            Defeat();
        }
    }
    void Attack(Vector3 Target) {
        anime?.Play("Attack");
        onAttack?.Invoke(Target);
    }

    void Defeat() {
        anime?.Play("Defeat");
    }
}

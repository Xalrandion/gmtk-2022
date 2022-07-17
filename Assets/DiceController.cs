using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class DiceController : MonoBehaviour
{
    public Dice AttackDice;
    public Dice DefenceDice;

    public float JumpForce;

    public float TorqueForce;

    public InputAction action = new();

    private bool ThrowingAttackDice = false;
    private bool ThrowingDefenceDice = false;
    private TaskCompletionSource<(int attack, int defence)> promise;
    
    private bool canThrow = false; 

    void Start()
    {
        action.performed += delegate {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, float.MaxValue ,LayerMask.GetMask("Dice"))) {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject == AttackDice.gameObject 
                || hit.collider.gameObject == DefenceDice.gameObject) {
                    Throw();
                }
            }
        };
        action.Enable();
    }
    void Update()
    {
        if (ThrowingAttackDice && AttackDice.body.IsSleeping()
        && ThrowingDefenceDice && DefenceDice.body.IsSleeping()) {
            promise.SetResult((AttackDice.currentValue, DefenceDice.currentValue));
            ThrowingAttackDice = false;
            ThrowingDefenceDice = false;
            Debug.Log("Les des");
        }
    }

    public void Throw() {
        if (!canThrow)
            return;
        Debug.Log("on lance");
        AttackDice.body.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        AttackDice.body.AddTorque(Random.insideUnitSphere * TorqueForce, ForceMode.Impulse);

        DefenceDice.body.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        DefenceDice.body.AddTorque(Random.insideUnitSphere * TorqueForce, ForceMode.Impulse);

        ThrowingAttackDice = true;
        ThrowingDefenceDice = true;
    }

    async public Task<(int attack, int defence)> WaitforNextThrow() {
        promise = new();
        canThrow = true;
        var res = await promise.Task;
        canThrow = false;
        return res;
    }    
}

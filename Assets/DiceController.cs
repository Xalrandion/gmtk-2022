using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DiceController : MonoBehaviour
{
    public Dice AttackDice;
    public Dice DefenceDice;

    public float JumpForce;

    public float TorqueForce;

    private bool ThrowingAttackDice = false;
    private bool ThrowingDefenceDice = false;
    private TaskCompletionSource<(int attack, int defence)> promise;

    void Start()
    {
        
    }

    // Update is called once per frame
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

    async public Task<(int attack, int defence)> Throw() {
        Debug.Log("on lance");
        AttackDice.body.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        AttackDice.body.AddTorque(Random.insideUnitSphere * TorqueForce, ForceMode.Impulse);

        DefenceDice.body.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        DefenceDice.body.AddTorque(Random.insideUnitSphere * TorqueForce, ForceMode.Impulse);

        ThrowingAttackDice = true;
        ThrowingDefenceDice = true;
        promise = new();
        return await promise.Task;
    }
}

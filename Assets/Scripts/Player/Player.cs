using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BasePlayer
{
    [SerializeField] public Gauge hp;

    public override bool IsDead() => hp.Current <= 0;

    public override void StartTurn(GameManager mngr)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damage)
    {
        hp.Current -= damage;
    }
}

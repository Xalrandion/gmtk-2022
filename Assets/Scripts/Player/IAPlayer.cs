using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPlayer : BasePlayer
{
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private bool isEnnemy = true;

    public override bool IsDead() => false;

    public override void StartTurn(GameManager mngr)
    {
        isPlayerTurn = true;
        mngr.Lanes.ForEach(lane =>
        {
            if (isEnnemy && lane.EnnemyUnit == null)
            {
                Debug.Log("no ennemi unit: adding one");
                var newUnit = unitManager.GenerateUnit(isEnnemy, lane);
                lane.DropUnit(isEnnemy);
                lane.SetUnit(newUnit, isEnnemy);
                return;
            }
            if (!isEnnemy && lane.PlayerUnit == null)
            {
                Debug.Log("no plyer unit: adding one");
                var newUnit = unitManager.GenerateUnit(isEnnemy, lane);
                lane.DropUnit(isEnnemy);
                lane.SetUnit(newUnit, isEnnemy);
                return;
            }
        });

        isPlayerTurn = false;
        mngr.SendMessage("EndTurn");   
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage");
    }
}

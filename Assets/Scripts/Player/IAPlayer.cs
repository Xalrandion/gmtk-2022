using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPlayer : BasePlayer
{
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private bool isEnnemy = true;

    [SerializeField] private uint maxNumberOfUnitAddedByTurn = 1;
    [SerializeField] private uint numberOfUnitAddedThisTurn = 0;
    [SerializeField] private uint numberOfGraceTurn = 1;

    public override bool IsDead() => false;

    private void EndTurn(GameManager mngr)
    {
        isPlayerTurn = false;
        mngr.SendMessage("EndTurn");
        numberOfUnitAddedThisTurn = 0;
    }

    public override IEnumerator StartTurn(GameManager mngr)
    {
        var tmp = Mathf.Lerp(1, mngr.Lanes.Count, (float)mngr.TurnCount / 30);
        maxNumberOfUnitAddedByTurn = (uint)Mathf.Floor(tmp);
        isPlayerTurn = true;
        if (mngr.TurnCount < numberOfGraceTurn)
        {
            EndTurn(mngr);
            yield break;
        }
        mngr.Lanes.ForEach(lane =>
        {
            if (isEnnemy && lane.EnnemyUnit == null && numberOfUnitAddedThisTurn < maxNumberOfUnitAddedByTurn)
            {
                Debug.Log("no ennemi unit: adding one");
                numberOfUnitAddedThisTurn += 1;
                var newUnit = unitManager.GenerateUnit(isEnnemy, lane, Random.Range(1, 7), Random.Range(1, 7));
                lane.DropUnit(isEnnemy);
                lane.SetUnit(newUnit, isEnnemy);
                newUnit.gameObject.transform.position = lane.CalcEnnemySlotLocation() + new Vector3(0, 1, 0);
                return;
            }
            if (!isEnnemy && lane.PlayerUnit == null && numberOfUnitAddedThisTurn < maxNumberOfUnitAddedByTurn)
            {
                Debug.Log("no plyer unit: adding one");
                numberOfUnitAddedThisTurn += 1;
                var newUnit = unitManager.GenerateUnit(isEnnemy, lane, Random.Range(1, 7), Random.Range(1, 7));
                lane.DropUnit(isEnnemy);
                lane.SetUnit(newUnit, isEnnemy);
                newUnit.gameObject.transform.position = lane.CalcPlayerSlotLocation() + new Vector3(0, 1, 0);
                return;
            }
        });

        EndTurn(mngr);
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage");
    }
}

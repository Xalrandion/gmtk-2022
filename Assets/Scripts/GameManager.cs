using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private uint turnCounter = 0;
    [SerializeField] private BasePlayer player;
    [SerializeField] private BasePlayer ai;

    public List<Lane> Lanes
    {
        get => lanes;
        private set { lanes = value;  }
    }
    public uint TurnCount
    {
        get => turnCounter;
        private set { turnCounter = value; }
    }
    [SerializeField] private List<Lane> lanes;
    // Start is called before the first frame update
    void Start()
    {
        EndTurn();
    }

    // Update is called once per frame
    void CalcBeetweenTurn()
    {
        float playerDmg = 0;
        foreach (var lane in lanes)
        {
            var res = lane.CalcTurn(turnCounter);
            playerDmg += res.PlayerHPLoss;
        }

        player.TakeDamage(playerDmg);
    }

    void CheckPlayerStillAlive()
    {
        if (player.IsDead())
        {
            // do game over (I have no Idea)
        }
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1);
        CalcBeetweenTurn();
        CheckPlayerStillAlive();
        turnCounter += 1;
        if (turnCounter % 2 == 0)
        {
            Debug.Log("Start of player turn: " + turnCounter);
            player.StartTurn(this);
            
        }
        else
        {
            Debug.Log("Start of ai turn: " + turnCounter);
            ai.StartTurn(this);
        }

        
    }

    void EndTurn()
    {
        Debug.Log("End last of turn: " + turnCounter);
        StartCoroutine(Waiter());
        


    }
}

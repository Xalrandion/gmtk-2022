using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private uint turnCounter = 0;
    [SerializeField] private BasePlayer player;
    [SerializeField] private BasePlayer ai;
    [SerializeField] private uint subTurn = 0;

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

    IEnumerator StarBeetweenTurn()
    {
        yield return new WaitForSeconds(1);
        CalcBeetweenTurn();
        CheckPlayerStillAlive();
        turnCounter += 1;
        EndTurn(); 
    }

    IEnumerator StartPlayerTurn()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Start player sub turn...");
        player.StartTurn(this);
        yield return null;
    }

    IEnumerator StartAITurn()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Start ai sub turn...");
        ai.StartTurn(this);
        yield return null;
    }


    void EndTurn()
    {
        if (subTurn == 0)
        {
            subTurn += 1;
            StartCoroutine(StartPlayerTurn());
            return;
        }
        if (subTurn == 1)
        {
            subTurn += 1;
            StartCoroutine(StartAITurn());
            return;
        }

        subTurn = 0;
        Debug.Log("End last of turn: " + turnCounter);
        StartCoroutine(StarBeetweenTurn());
        


    }
}

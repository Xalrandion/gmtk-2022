using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool isPlayerTurn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void StartTurn(GameManager mngr);
    public abstract void TakeDamage(float damage);

    public abstract bool IsDead();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour, ISlot
{
    public BaseUnit PlayerUnit
    {
        get => playerUnit;
    }

    [SerializeField] private BaseUnit playerUnit;
    public BaseUnit EnnemyUnit
    {
        get => ennemyUnit;
    }
    [SerializeField] private BaseUnit ennemyUnit;
    [SerializeField] [Range(0.0f, 100.0f)] public float PlayerSideDistance;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<BoxCollider>().size = new Vector3(1.5f, 1, (PlayerSideDistance * 2) + 1);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider>().size = new Vector3(1.5f, 0.1f, (PlayerSideDistance * 2) + 5);
        //ennemyUnit?.transform.LookAt(CalcPlayerSlotLocation() + new Vector3(0, 1, 0));
        //playerUnit?.transform.LookAt(CalcEnnemySlotLocation() + new Vector3(0, 1, 0));

    }

    public delegate void AttackDoneCB(float overflowDmg);
    public delegate void TurnDoneCB(LaneTurnResult result);

    private IEnumerator DoAttackWithAnim(AttackDoneCB cb)
    {
        EnnemyUnit?.DoAttackAnim(CalcPlayerSlotLocation());
        yield return new WaitForSeconds(1);
        PlayerUnit?.DoAttackAnim(CalcEnnemySlotLocation());
        yield return new WaitForSeconds(1);
        var res = DoAttaks();
        cb(res);
    }
    


    // Returns dammage to player
    private float DoAttaks()
    {
        if (EnnemyUnit == null)
        {
            return 0f;
        }

        if (PlayerUnit == null)
        {
            return EnnemyUnit.CalcDamage();
        }


        PlayerUnit.Attack(EnnemyUnit);
        return EnnemyUnit.Attack(PlayerUnit);
    }


    private void DoDeath()
    {
        if (EnnemyUnit != null && EnnemyUnit.HPgauge.Current <= 0)
        {
            Debug.Log("Ennemy unit is dead");
            var unitToKill = EnnemyUnit;

            DropUnit(true);
            unitToKill?.KillUnit();
            // Destroy(EnnemyUnit.gameObject);

        }
        if (PlayerUnit != null && PlayerUnit.HPgauge.Current <= 0)
        {
            Debug.Log("Player unit is dead");
            var unitToKill = PlayerUnit;

            DropUnit(false);
            unitToKill?.KillUnit();
            //Destroy(PlayerUnit.gameObject);

        }

       
    }


    public void CalcTurn(uint turn, TurnDoneCB cb)
    {
        StartCoroutine(DoAttackWithAnim((r) => {
            LaneTurnResult res = new();
            res.PlayerHPLoss = r; 
            DoDeath(); 
            cb(res); 
        }));

        //res.PlayerHPLoss = DoAttaks();
        //DoDeath();

        //return res;
    }

    public void SetUnit(BaseUnit target, bool isEnnemy)
    {
        target.owner = this;
        if (isEnnemy)
        {
            ennemyUnit = target;
            ennemyUnit.SetRestingPlace(CalcEnnemySlotLocation());
            ennemyUnit.transform.Rotate(new Vector3(0, 1, 0), 180);
            //ennemyUnit.transform.LookAt(CalcPlayerSlotLocation());
            
            return;
        }

        playerUnit = target;
        playerUnit.SetRestingPlace(CalcPlayerSlotLocation());
        //playerUnit.transform.LookAt(CalcEnnemySlotLocation());
    }

    public void DropUnit(bool isEnnemy)
    {
       if (isEnnemy)
        {
            ennemyUnit = null;
            return;
        }
        playerUnit = null;
    }

    public Vector3 CalcEnnemySlotLocation() => (transform.forward * PlayerSideDistance * -1) + transform.position + new Vector3(0, 1.2f, 0);
    public Vector3 CalcPlayerSlotLocation() => (transform.forward * PlayerSideDistance) + transform.position + new Vector3(0, 1.2f, 0);


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawCube((transform.forward * PlayerSideDistance) + transform.position, new Vector3(1, 0.3f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawCube((transform.forward * PlayerSideDistance * -1) + transform.position, new Vector3(1, 0.3f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawLine((transform.forward * PlayerSideDistance) + transform.position, (transform.forward * PlayerSideDistance * -1) + transform.position);
    }

    public ISlot.SlotReqResponse GetOwnership(BaseUnit grabbed, BasePlayer grabber)
    {
        if (!grabber.isPlayerTurn) return ISlot.SlotReqResponse.KO;
        if (playerUnit != null) return ISlot.SlotReqResponse.KO;
        if (grabbed == null) return ISlot.SlotReqResponse.OK;
        SetUnit(grabbed, false);
        return ISlot.SlotReqResponse.OK;
    }

    public ISlot.SlotReqResponse DropOwnership(BasePlayer grabber)
    {
        if (!grabber.isPlayerTurn) return ISlot.SlotReqResponse.KO;
        DropUnit(false);
        return ISlot.SlotReqResponse.OK;
    }

    public BaseUnit GetSlotContent()
    {
        return playerUnit;
    }

    public GameObject GetGameObject() => this.gameObject;

    public Vector3 GetSlotLocation() => CalcPlayerSlotLocation();

    public Vector3 GetSlotLocation(BasePlayer player)
    {
        return Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns dammage to player
    private float DoAttcaks()
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
            Destroy(EnnemyUnit.gameObject);
            DropUnit(true);
        }
        if (PlayerUnit != null && PlayerUnit.HPgauge.Current <= 0)
        {
            Debug.Log("Player unit is dead");
            Destroy(PlayerUnit.gameObject);
            DropUnit(false);
        }
    }

    public LaneTurnResult CalcTurn(uint turn)
    {
        LaneTurnResult res = new();
        
        res.PlayerHPLoss = DoAttcaks();
        DoDeath();

        return res;
    }

    public void SetUnit(BaseUnit target, bool isEnnemy)
    {
        if (isEnnemy)
        {
            ennemyUnit = target;
            ennemyUnit.transform.position = CalcEnnemySlotLocation();
            ennemyUnit.transform.LookAt(CalcPlayerSlotLocation());
            return;
        }

        playerUnit = target;
        playerUnit.transform.position = CalcPlayerSlotLocation();
        playerUnit.transform.LookAt(CalcEnnemySlotLocation());
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

    private Vector3 CalcEnnemySlotLocation() => (transform.forward * PlayerSideDistance * -1) + transform.position;
    private Vector3 CalcPlayerSlotLocation() => (transform.forward * PlayerSideDistance) + transform.position;


    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawCube((transform.forward * PlayerSideDistance) + transform.position, new Vector3(1, 0.3f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawCube((transform.forward * PlayerSideDistance * -1) + transform.position, new Vector3(1, 0.3f, 1));
        Gizmos.color = Color.red;
        Gizmos.DrawLine((transform.forward * PlayerSideDistance) + transform.position, (transform.forward * PlayerSideDistance * -1) + transform.position);
    }
}

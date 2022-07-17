using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IGrabbable
{
    [SerializeField] public Gauge HPgauge = new(5 , 5);
    [SerializeField] public float speed = 5f;

    private Vector3 velocity;
    [SerializeField] private Vector3 restingPlace;

    public ISlot owner;
    public bool canSnapBack = false;

    private UnitController unitController;

    private void Awake()
    {
        unitController = GetComponent<UnitController>();
    }

    public BaseUnitData UnitData
    {
        get => unitData;
        protected set { unitData = value; }
    }
    [SerializeField] private BaseUnitData unitData;
    public float BaseDamage
    {
        get => baseDamage;
        private set { baseDamage = value; }
    }
    [SerializeField] private float baseDamage = 1;
    public bool isGrabbed;

    // Start is called before the first frame updat
    public float CalcDamage()
    {
        return BaseDamage;
    }

    private float CalcClassDmgBonus(BaseUnit target)
    {
        if (target.UnitData == unitData.BadMatch)
        {
            return unitData.BadMatchBonus;
        }
        if (target.UnitData == unitData.StrongMatch)
        {
            return unitData.StrongMatchBonus;
        }
        return 0;
    }

    public float CalcDamage(BaseUnit target)
    {
        var baseDmg = CalcDamage();
        return baseDmg + (baseDmg * CalcClassDmgBonus(target));
    }

    // Returns overflow damage
    public float Attack(BaseUnit target)
    {
        var inflictedDmg = CalcDamage(target);
        Debug.Log(gameObject.name + " attacks " + target.name + "for " + inflictedDmg + "damage has " + HPgauge.Current + "/" + HPgauge.Max + " hp");
        return target.ReceiveDamage(inflictedDmg, this);
    }
    
    public void DoAttackAnim(Vector3 target)
    {
        unitController?.Attack(target);
    }

    public void KillUnit()
    {
        if (unitController == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            unitController.Defeat();
        }
    }
    
    // Negative damage would heal
    // Returns overflow damage
    public float ReceiveDamage(float damage, BaseUnit attacker)
    {
        float nextHP = HPgauge.Current - damage;

        HPgauge.Current = nextHP;
        Debug.Log(gameObject.name + " attacked by " + attacker.name + " and received " + damage + " damages has " + HPgauge.Current + "/" + HPgauge.Max + " hp");
        return Mathf.Clamp(nextHP, float.MinValue, 0);
    }

    public void SetStats(float maxLife, float currentLife, float damage)
    {
        HPgauge.Max = maxLife;
        HPgauge.Current = currentLife;
        baseDamage = damage;
    }

    private void Update()
    {
        if (owner != null && !isGrabbed && restingPlace != Vector3.zero && canSnapBack)
        {
            if (Vector3.Distance(transform.position, restingPlace) < 0.1f)
            {
                canSnapBack = false;
            }
            transform.position = Vector3.SmoothDamp(transform.position, restingPlace, ref velocity, speed);
        }
    }

    public void OnGrab()
    {
        isGrabbed = true;
        canSnapBack = true;
    }

    public void OnDrop()
    {
        isGrabbed = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + (transform.TransformDirection((transform.up) * 50)), transform.position + (transform.up * -1 * 100));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * 1));
    }

    public void SetRestingPlace(Vector3 restingPlace)
    {
        this.restingPlace = restingPlace;
        canSnapBack = true;
    }

}

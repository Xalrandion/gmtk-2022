using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSlot : MonoBehaviour, ISlot
{
    [SerializeField] private BaseUnit unit;


    public ISlot.SlotReqResponse DropOwnership(BasePlayer grabbber)
    {
        if (unit == null) return ISlot.SlotReqResponse.OK;
        unit.owner = null;
        unit = null;
        return ISlot.SlotReqResponse.OK;
    }

    public ISlot.SlotReqResponse GetOwnership(BaseUnit grabbed, BasePlayer grabber)
    {
        unit = grabbed;
        unit.owner = this;
        unit.SetRestingPlace(transform.position);
        return ISlot.SlotReqResponse.OK;
    }

    public BaseUnit GetSlotContent()
    {
        return unit;
    }

    public GameObject GetGameObject() => this.gameObject;

    public Vector3 GetSlotLocation() => this.transform.position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(GetSlotLocation(), new Vector3(1, 0.1f, 1));
    }
}

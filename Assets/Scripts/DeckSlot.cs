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
        return ISlot.SlotReqResponse.OK;
    }

    public BaseUnit GetSlotContent()
    {
        return unit;
    }

    public GameObject GetGameObject() => this.gameObject;

    public Vector3 GetSlotLocation() => this.transform.position;
}

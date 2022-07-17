using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISlot
{
    enum SlotReqResponse { KO, OK};
    // 0 ok 1 not ok
    public SlotReqResponse GetOwnership(BaseUnit grabbed, BasePlayer grabber);

    // 0 ok 1 not ok
    public SlotReqResponse DropOwnership(BasePlayer grabbber);

    public BaseUnit GetSlotContent();

    public GameObject GetGameObject();

    public Vector3 GetSlotLocation();
}

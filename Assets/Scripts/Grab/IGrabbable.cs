using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void OnGrab();
    void OnDrop();
    void SetRestingPlace(Vector3 restingPlace);
}

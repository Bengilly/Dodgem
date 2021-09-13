using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupObject : MonoBehaviour
{
    public abstract void CollectPickup();
    public abstract void DeletePickup();
}

using UnityEngine;

public interface IReceiveGrabbable
{
    bool CanReceive(IGrabbable grabbable);
    void Receive(IGrabbable grabbable, Player player);
    void Drop();
}

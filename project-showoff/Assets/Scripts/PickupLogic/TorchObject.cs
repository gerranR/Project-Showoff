using UnityEngine;

public class TorchObject : PickupObject
{
    public override string HeldLayerName => "HeldTorch";
    public override string DroppedLayerName => "Torch";
}

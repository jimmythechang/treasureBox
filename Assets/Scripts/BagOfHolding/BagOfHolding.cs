using UnityEngine;

public class BagOfHolding : Item {

    private BagMouth bagMouth;

    protected override void Start() {
        base.Start();
        bagMouth = GetComponentInChildren<BagMouth>();
    }
}

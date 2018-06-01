using UnityEngine;
using System.Collections;

public class BagMouth : MonoBehaviour {

    private BagOfHolding bagOfHolding;

    protected void Start() {
        bagOfHolding = GetComponentInParent<BagOfHolding>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        /*
         * We're only interested in the collision of the Blade Tip.
         */
        Blade blade = collision.gameObject.GetComponentInParent<Blade>();
        if (blade != null && bladeIsCloseToBagMouth(blade)) {
            snapSwordToMouth(blade);
        }
    }

    private bool bladeIsCloseToBagMouth(Blade blade) {
        float bladeAngle = blade.transform.eulerAngles.z;
        float bagAngle = transform.eulerAngles.z;

        /*
         * Determine if the Sword is correctly pointing into the Bag.
         */
        return (Mathf.Abs(Mathf.Abs(bladeAngle - bagAngle) - 180) <= 5);
    }

    private void snapSwordToMouth(Blade blade) {
        Sword sword = blade.GetComponentInParent<Sword>();

        Transform snapPoint = transform.Find("Snap Point");
        float bagAngle = transform.eulerAngles.z;

        sword.lockSword(bagAngle, snapPoint.position);
    }

    
    
}



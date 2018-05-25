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
        float bagAngle = transform.eulerAngles.z;

        CircleCollider2D bagMouthCollider = GetComponent<CircleCollider2D>();
        CircleCollider2D bladeTipCollider = blade.GetComponentInChildren<CircleCollider2D>();

        float xDiff = bladeTipCollider.transform.position.x - bagMouthCollider.transform.position.x;
        float yDiff = bladeTipCollider.transform.position.y - bagMouthCollider.transform.position.y;

        Sword sword = blade.GetComponentInParent<Sword>();
        sword.transform.position = new Vector3(sword.transform.position.x - xDiff, sword.transform.position.y - yDiff, sword.transform.position.z);
        sword.lockSword(bagAngle);
    }

    
    
}



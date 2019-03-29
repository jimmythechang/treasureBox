﻿using UnityEngine;
 
public class BagMouth : MonoBehaviour {

    /***********************
     * EXTERNAL REFERENCES *
     ***********************/
     
    // A reference to a GameObject that will ultimately contain both the Sword and BagOfHolding.
    public GameObject swordAndBagParent;

    // A reference to the BagOfHolding object, which is the immediate parent of the BagMouth.
    private Item bagOfHolding;

    protected void Start() {
        bagOfHolding = GetComponentInParent<Item>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        /*
         * We're only interested in the collision of the Blade Tip.
         */
        Blade blade = collision.gameObject.GetComponentInParent<Blade>();
        if (blade != null && collision.gameObject.name == "Blade 0" && bladeIsCloseToBagMouth(blade)) {
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

        Sword sword = blade.GetComponentInParent<Sword>();
  
        sword.lockSword(bagAngle, this);

        float distanceFromBagMouth = sword.calculateDistanceFromBagMouth();
        sword.setInitialDistanceFromBagMouth(distanceFromBagMouth);
        setSwordAndBagParent(sword);
    }

    
    private void setSwordAndBagParent(Sword sword) {
        swordAndBagParent.transform.position = transform.position;
        bagOfHolding.setParentItem(swordAndBagParent.GetComponent<Item>());
        sword.setParentItem(swordAndBagParent.GetComponent<Item>());

        bagOfHolding.setItemSortingOrder(10);
    }
}



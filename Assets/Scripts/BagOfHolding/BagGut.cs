using UnityEngine;
using System.Collections;

public class BagGut : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        Blade blade = collision.gameObject.GetComponent<Blade>();
        if (swordIsSheathing(blade)) { 
                blade.isVisible = false;
        }

    }

    private bool swordIsSheathing(Blade blade) {
        if (blade != null) {
            Item bladeParent = blade.getParentItem();
            return bladeParent.GetComponentInChildren<Sword>() != null && bladeParent.GetComponentInChildren<Sword>().isSheathing;
        }

        return false;
    }
}

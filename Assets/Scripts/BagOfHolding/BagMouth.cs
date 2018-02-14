using UnityEngine;
using System.Collections;

public class BagMouth : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Blade>() != null) {
            Sword sword = collision.gameObject.GetComponentInParent<Sword>();

            float swordAngle = sword.transform.eulerAngles.z;
            float bagAngle = transform.eulerAngles.z;

            /*
             * Determine if the Sword is correctly pointing into the Bag.
             */
            if (Mathf.Abs(Mathf.Abs(swordAngle - bagAngle) - 180) <= 5) {
                Debug.Log("Sword at appropriate angle!");
            } 

        }
    }

    
}



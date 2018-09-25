using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Item heldItem { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && heldItem == null) {
            clickOnObject("leftClick");
        }
        else if (Input.GetMouseButtonDown(1)) {
            clickOnObject("rightClick");
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
            if (heldItem != null) {
                heldItem.release();
                heldItem = null;
            }
        }
	}

    /**
     * Sends a message to a clicked GameObject, if one is found.
     * <param name="message">Method to invoke on the clicked GameObject.</param>
     */
    private GameObject sendMessageToObject(string message) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit) {
            hit.transform.gameObject.SendMessage(message);
            return hit.transform.gameObject;
        }
        return null;
    }

    /**
     * Sets the heldItem for the Player if the object clicked was an Item.
     * 
     * <param name="message">Method to invoke on the clicked GameObject.</param>
     */
    private void clickOnObject(string message) {
        GameObject clickedObject = sendMessageToObject(message);
        if (clickedObject != null && clickedObject.GetComponent<Item>()) {
            heldItem = clickedObject.GetComponent<Item>();

            // If the Item has a parent, hold that instead!
            while (heldItem.getParentItem() != null) {
                heldItem = heldItem.getParentItem();
            }
        }
    }
}

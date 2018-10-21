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
    private void clickOnObject(string message) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit && hit.transform.gameObject != null) {
            holdItem(hit.transform.gameObject, message);
        }
    }

    /**
     * Sets the heldItem for the Player if the GameObject provided is an Item.
     */
    public void holdItem(GameObject gameObject, string message) {
        if (gameObject.GetComponent<Item>() != null) {
            heldItem = gameObject.GetComponent<Item>().getParentItem();
            heldItem.SendMessage(message);
        }
    }

    
}

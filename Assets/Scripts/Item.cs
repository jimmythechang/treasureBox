using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    /*
     * Calculation of the z-distance between the object and the Camera.
     */
    private float zDiff;

    private bool isDragging;
    private bool isRotating;

    private float initialZRotation;
    private float initialMouseAngleRelativeToObject;
    private Vector3 offset;

    private enum State { DRAGGING, ROTATING, UNTOUCHED };
    private State state = State.UNTOUCHED;


	void Start () {
        isDragging = false;
        zDiff = transform.position.z - Camera.main.transform.position.z;
	}
	
	void Update () {
        switch (state) {
            case State.DRAGGING: {
                if (Input.GetMouseButtonUp(0)) {
                    state = State.UNTOUCHED;
                    break;
                }
                dragItem();
                break;
            }
            case State.ROTATING: {
                if (Input.GetMouseButtonUp(1)) {
                    state = State.UNTOUCHED;
                    break;
                }
                rotateItem();
                break;
            }
            default: {
                if (Input.GetMouseButtonDown(0) && mouseOverItem()) {
                    offset = calculateOffset();
                    state = State.DRAGGING;
                }
                else if (Input.GetMouseButtonDown(1) && mouseOverItem()) {
                    /*
                     * Set the initial orientation of the Item and the initial angle of the mouse
                     * relative to that orientation.
                     */
                    initialZRotation = transform.eulerAngles.z;
                    initialMouseAngleRelativeToObject = calculateAngleOfMouseRelativeToGameObject();
                    state = State.ROTATING;
                }
                break;
            }
        }
	}

    /**
     * Determines if the mouse's position is over an Item.
     */
    protected bool mouseOverItem() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit) {
            return hit.collider.Equals(GetComponent<PolygonCollider2D>());
        }
        return false;
    }

    /**
     * Calculates the offset of the mouse relative to the Item's position, depending on where the player clicked it.
     */
    protected Vector3 calculateOffset() {
        return transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDiff));
    }

    protected void dragItem() {
        Vector3 mouseGamePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDiff));
        transform.position = mouseGamePosition + offset;
    }

    /**
     * Finds the value of the angle whose tangent is equal to 
     * [mouseGamePosition.y - gameObject.transform.position.y] / [mouseGamePosition.x - gameObject.transform.position.x].
     */
    protected float calculateAngleOfMouseRelativeToGameObject() {
        Vector3 mouseGamePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDiff));
        float yDiff = mouseGamePosition.y - gameObject.transform.position.y;
        float xDiff = mouseGamePosition.x - gameObject.transform.position.x;
        return Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;
    }

    protected void rotateItem() {
        float angle = calculateAngleOfMouseRelativeToGameObject();
        float zRotation = (initialZRotation + (angle - initialMouseAngleRelativeToObject)) % 360;
        transform.eulerAngles = new Vector3(0, 0, zRotation);
    }

}

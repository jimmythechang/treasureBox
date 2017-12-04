using UnityEngine;

public class Item : MonoBehaviour {
    /*
     * Calculation of the z-distance between the object and the Camera.
     */
    private float zDiff;

    private float initialZRotation;
    private float initialMouseAngleRelativeToObject;
    private Vector3 offset;

    private enum State { DRAGGING, ROTATING, UNTOUCHED };
    private State state = State.UNTOUCHED;

    public bool isInChest { get; set; }

    private GameObject chest;

	void Start () {
        zDiff = transform.position.z - Camera.main.transform.position.z;
        chest = GameObject.FindGameObjectWithTag("Chest");
	}
	
	void Update () {
        switch (state) {
            case State.DRAGGING: {
                dragItem();
                break;
            }
            case State.ROTATING: {
                rotateItem();
                break;
            }
            default: {
                break;
            }
        }
	}

    /**
     * Action to execute if the Item is clicked on with the left mouse button.
     */
    protected void leftClick() {
        offset = calculateOffset();
        state = State.DRAGGING;
    }

    /**
     * Action to execute if the Item is clicked on with the right mouse button.
     */
    protected void rightClick() {
        /*
         * Set the initial orientation of the Item and the initial angle of the mouse
         * relative to that orientation.
         */
        initialZRotation = transform.eulerAngles.z;
        initialMouseAngleRelativeToObject = calculateAngleOfMouseRelativeToGameObject();
        state = State.ROTATING;
    }

    /**
     * Action to execute if the Item is no longer being handled by the Player.
     */
    public void release() {
        state = State.UNTOUCHED;
        if (isWithinChest()) {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
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

    /**
     * Determines if an Item is fully tucked away in the Chest.
     */
    protected bool isWithinChest() {
        Bounds itemBounds = GetComponent<PolygonCollider2D>().bounds;
        Bounds chestBounds = chest.GetComponent<BoxCollider>().bounds;
        return chestBounds.Contains(itemBounds.min) && chestBounds.Contains(itemBounds.max); 
    }

}

using UnityEngine;

public class Item : MonoBehaviour {
    /*
     * Calculation of the z-distance between the object and the Camera.
     */
    protected float zDiff;

    protected float initialZRotation;
    protected float initialMouseAngleRelativeToObject;
    protected Vector3 offset;


    /*
     * A parent Item that this Item might be contained in.
     */
    protected Item parentItem;

    protected enum State { DRAGGING, ROTATING, UNTOUCHED };
    protected State state = State.UNTOUCHED;

    public bool isInChest { get; set; }
    public bool isTouchingAnotherItem { get; set; }

    private GameObject chest;
    protected Player player;

    protected virtual void Start() {
        zDiff = transform.position.z - Camera.main.transform.position.z;
        chest = GameObject.FindGameObjectWithTag("Chest");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isTouchingAnotherItem = false;
    }

    protected virtual void Update() {
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
    public virtual void leftClick() {
        offset = calculateOffset();
        state = State.DRAGGING;
        setItemSortingOrder(10);
    }

    /**
     * Action to execute if the Item is clicked on with the right mouse button.
     */
    public virtual void rightClick() {
        /*
            * Set the initial orientation of the Item and the initial angle of the mouse
            * relative to that orientation.
            */
        initialZRotation = transform.eulerAngles.z;
        initialMouseAngleRelativeToObject = calculateAngleOfMouseRelativeToPoint(transform.position);
        state = State.ROTATING;
    }

    /**
     * Action to execute if the Item is no longer being handled by the Player.
     */
    public virtual void release() {
       state = State.UNTOUCHED;
       setItemSortingOrder(0);
    }

    /**
     * Sets the sorting order for all SpriteRenderers associated with this Item, 
     * including those of its children.
     */
    private void setItemSortingOrder(int sortingOrder) {
        if (GetComponent<SpriteRenderer>() != null) {
            GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        }

        if (GetComponentsInChildren<SpriteRenderer>().Length > 0) {
            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < renderers.Length; i++) {
                renderers[i].sortingOrder = sortingOrder;
            }
        }
    }

    /**
     * Calculates the offset of the mouse relative to the Item's position, depending on where the player clicked it.
     */
    protected Vector3 calculateOffset() {
        Vector3 mouseGamePosition = calculateMouseGamePosition();
        return transform.position - mouseGamePosition;
    }

    protected virtual void dragItem() {
        Vector3 mouseGamePosition = calculateMouseGamePosition();
        transform.position = mouseGamePosition + offset;
    }

    /**
     * Finds the value of the angle whose tangent is equal to 
     * [mouseGamePosition.y - point.y] / [mouseGamePosition.x - point.x].
     */
    protected float calculateAngleOfMouseRelativeToPoint(Vector3 point) {
        Vector3 mouseGamePosition = calculateMouseGamePosition();
        float yDiff = mouseGamePosition.y - point.y;
        float xDiff = mouseGamePosition.x - point.x;
        return Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;
    }

    public virtual void rotateItem() {
        float angle = calculateAngleOfMouseRelativeToPoint(transform.position);
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

    protected void OnTriggerStay2D(Collider2D collision) {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().material.color = Color.red;
        }
        isTouchingAnotherItem = true;
    }

    protected void OnTriggerExit2D(Collider2D collision) {
        if (GetComponent<Renderer>() != null) {
            GetComponent<Renderer>().material.color = Color.white;
        }
        isTouchingAnotherItem = false;
    }
    /**
     * Converts the mouse's screen coordinates to an in-game position.
     */
    protected Vector3 calculateMouseGamePosition() {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDiff));
    }

    /**
     * Recursively returns the topmost parentItem of this Item; otherwise returns the Item itself.
     */
    public virtual Item getParentItem() {
        Item item = this;
        while (item.parentItem != null) {
            item = item.parentItem;
        }
        return item;
    }

    public virtual void setParentItem(Item parentItem) {
        release();
        this.parentItem = parentItem;
        transform.SetParent(parentItem.transform);
    }
}

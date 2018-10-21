using UnityEngine;

public class Sword : Item {

    public bool sheathingSword { get; set; }

    private Vector2 unitVector;

    protected override void Start() {
        base.Start();
        sheathingSword = false;
    }

    public override void leftClick() {
        if (sheathingSword) {
            calculateUnitVector();
            offset = calculateProjectedMouseVectorOntoUnitVector();
            state = State.DRAGGING;
        }
        else {
            base.leftClick();
        }
    }

    protected override void dragItem() {
        if (sheathingSword) {
            Vector2 projectedVector = calculateProjectedMouseVectorOntoUnitVector();
            transform.position += new Vector3(projectedVector.x, projectedVector.y, 0) - offset;
        }
        else {
            base.dragItem();
        }
    }

    public override void rotateItem() {
        if (!sheathingSword) {
            base.rotateItem();
        }
    }

    public override Item getParentItem() {
        return this;
    }

    /**
     * Calculates the unit vector facing in the direction that the blade is pointing.
     */
    private void calculateUnitVector() {
        Transform colliderTransform = GetComponentInChildren<CircleCollider2D>().transform;
        unitVector = new Vector2(colliderTransform.position.x - transform.position.x, colliderTransform.position.y - transform.position.y);
        unitVector.Normalize();
    }

    /**
     * Projects the vector formed between the location of the mouse and the center of the sword
     * onto the unit vector aligned with the sword's length. Used when movement is constrained.
     */
    private Vector2 calculateProjectedMouseVectorOntoUnitVector() {
        Vector3 mouseGamePosition = calculateMouseGamePosition();
        Vector2 mouseVector = new Vector2(mouseGamePosition.x - transform.position.x, mouseGamePosition.y - transform.position.y);
        return Vector2.Dot(unitVector, mouseVector) * unitVector;
    }

    /**
     * Constrains the sword along the axis of the bag. 
     */
    public void lockSword(float bagAngle, Vector3 position) {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, bagAngle + 180);
        transform.position = position;
        sheathingSword = true;
        offset = calculateProjectedMouseVectorOntoUnitVector();
    }

}

using UnityEngine;

public class Sword : Item {

    private Blade blade;
    public bool movementConstrained { get; set; }

    private Vector2 unitVector;

    protected override void Start() {
        base.Start();
        blade = GetComponentInChildren<Blade>();
        calculateUnitVector();
        movementConstrained = false;
    }

    protected override void leftClick() {
        base.leftClick();

        if (movementConstrained) {
            offset = calculateProjectedMouseVectorOntoUnitVector();
        }
    }

    protected override void dragItem() {
        if (movementConstrained) {
            Vector2 projectedVector = calculateProjectedMouseVectorOntoUnitVector();
            transform.position += new Vector3(projectedVector.x, projectedVector.y, 0) - offset;
        } 
        else {
            base.dragItem();
        }
    }

    protected override void rotateItem() {
        base.rotateItem();
        calculateUnitVector();
    }

    /**
     * Calculates the unit vector facing in the direction that the blade is pointing.
     */
    private void calculateUnitVector() {
        Transform colliderTransform = blade.GetComponent<CircleCollider2D>().transform;
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
}

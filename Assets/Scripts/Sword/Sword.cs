﻿using UnityEngine;

public class Sword : Item {


    /***********************
     * EXTERNAL REFERENCES *
     ***********************/

    // A reference to the BagMouth.
    private BagMouth bagMouth;


    public bool isSheathing { get; set; }

    private Vector2 unitVector;

    private float initialDistanceFromBagMouth;
    private float distanceFromBagMouth;

    protected override void Start() {
        base.Start();
        isSheathing = false;
    }

    protected override void Update() {
        base.Update();
    }

    public override void leftClick() {
        if (isSheathing) {
            calculateUnitVector();
            offset = calculateProjectedMouseVectorOntoUnitVector();
            state = State.DRAGGING;
        }
        else {
            base.leftClick();
        }
    }

    protected override void dragItem() {
        if (isSheathing) {
            Vector2 projectedVector = calculateProjectedMouseVectorOntoUnitVector();
            transform.position += new Vector3(projectedVector.x, projectedVector.y, 0) - offset;

            distanceFromBagMouth = getDistanceFromBagMouth();

            // Prevent the Sword from being dragged away from the BagMouth.
            if (isSwordBeingMovedTooFarAway()) {
                transform.position = bagMouth.transform.Find("Snap Point").position;
            }
        }
        else {
            base.dragItem();
        }
    }

    public override void rotateItem() {
        if (!isSheathing) {
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
        Transform colliderTransform = transform.Find("Blade Tip");
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
    public void lockSword(float bagAngle, Transform snapPoint) {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, bagAngle + 180);
        transform.position = snapPoint.position;

        bagMouth = snapPoint.GetComponentInParent<BagMouth>();

        isSheathing = true;

        offset = calculateProjectedMouseVectorOntoUnitVector();
    }

    /**
     * Determine if the Sword is being moved too far away from the SnapPoint present in the BagMouth.
     */
    private bool isSwordBeingMovedTooFarAway() {
        Transform snapPoint = bagMouth.transform.Find("Snap Point");

        float xSign = Mathf.Sign(unitVector.x) * -1;
        float ySign = Mathf.Sign(unitVector.y) * -1;
        return (transform.position.x * xSign > snapPoint.position.x * xSign) && 
               (transform.position.y * ySign > snapPoint.position.y * ySign);
    }

    public float getDistanceFromBagMouth() {
        return Vector2.Distance(transform.position, bagMouth.transform.position);
    }

    public void setInitialDistanceFromBagMouth(float distance) {
        initialDistanceFromBagMouth = distance;
    }

    public void setDistanceFromBagMouth(float distance) {
        distanceFromBagMouth = distance;
    }

    private float calculatePercentageOfDistanceFromBagMouth() {
        return distanceFromBagMouth / initialDistanceFromBagMouth;
    }
}

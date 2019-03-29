using UnityEngine;


public class Blade : Item {

    private Sword sword;

    public bool isVisible { get; set; }

    protected override void Start() {
        sword = GetComponentInParent<Sword>();
        setParentItem(sword);

        isVisible = true;
    }

    protected override void Update() {
        GetComponent<SpriteRenderer>().enabled = isVisible;
    }

    /**
     * If the Blade is invisible, we do not want to propagate clicks to its parentItem.
     */
    public override Item getParentItem() {
        if (isVisible) {
            return base.getParentItem();
        }
        else {
            return this;
        }
    }

    public override void leftClick() {
        if (!isVisible) {
            return;
        }
    }


}

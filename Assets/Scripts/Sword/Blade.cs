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
        //GetComponent<SpriteRenderer>().enabled = isVisible;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        
    }

}

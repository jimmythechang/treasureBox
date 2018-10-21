using UnityEngine;

public class Blade : Item {

    public bool isVisible { get; set; }

    void Start() {
        isVisible = true;    
    }

    void Update() {
        //GetComponent<SpriteRenderer>().enabled = isVisible;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

    }


}

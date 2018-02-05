using UnityEngine;

public class Blade : MonoBehaviour {

    private Sword sword;

    bool locked = false;

    // Use this for initialization
    void Start() {
        sword = GetComponentInParent<Sword>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        sword.movementConstrained = true;
    }

}

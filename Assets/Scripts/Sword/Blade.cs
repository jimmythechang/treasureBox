﻿using UnityEngine;

public class Blade : MonoBehaviour {

    private Sword sword;

    // Use this for initialization
    void Start() {
        sword = GetComponentInParent<Sword>();
    }

    // Update is called once per frame
    void Update() {

    }

}

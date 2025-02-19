using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CountainerCounterVisual : MonoBehaviour {

    [SerializeField] private CoutainerCounter coutainerCounter;

    private const string OPEN_CLOSE = "OpenClose";
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        coutainerCounter.OnPlayerGrabbedObject += CoutainerCounter_OnPlayerGrabbedObject;
    }

    private void CoutainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}

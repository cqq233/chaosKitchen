using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObjects;


    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool showVisual;
        showVisual = e.state == StoveCounter.State.Frying ||
                     e.state == StoveCounter.State.Fried;

        stoveOnGameObject.SetActive(showVisual);
        particlesGameObjects.SetActive(showVisual);
    }
}

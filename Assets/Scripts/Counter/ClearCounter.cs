using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no kitchenObject on ClearCounter
            if (player.HasKitchenObject()) {
                // player is carrying something
                // Set kitchenObject's parent to ClearCounter's
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                // Player is not carrying anything,do nothing
            }
        }
        else {
            // There is a kitchenObject on ClearCounter
            if (player.HasKitchenObject()) {
                // Player is carrying something, do nothing
            }
            else {
                // Player is not carrying anything
                // And the player will pick up something from the ClearCounter
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        
    }

    

}



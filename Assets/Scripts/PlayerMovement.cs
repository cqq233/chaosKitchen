using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventsArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventsArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking = false;
    private Vector3 lastMoveDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one play instance");
        }

        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {

        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }

    }

    private void Update() {
        HandleMovement();
        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
        HandleInteractions();
    }

    public bool GetIsWalking() {
        return isWalking;
    }

    private BaseCounter GetSelectedCounter() {
        return selectedCounter;
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveVector != Vector3.zero) {
            lastMoveDir = moveVector;
        }

        float interactDistance = 2.0f;

        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit hitinfo, interactDistance, counterLayerMask)) {

            if (hitinfo.transform.TryGetComponent(out BaseCounter baseCounter)) {

                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }

            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }

    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveVector = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2.0f;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + transform.up * playerHeight,
            playerRadius,
            moveVector,
            moveDistance);

        if (!canMove) {
            // Cannot move towards;

            //try to X movement;
            Vector3 moveVectorX = new Vector3(moveVector.x, 0f, 0f).normalized;
            canMove = (moveVector.x != 0) && !Physics.CapsuleCast(
                transform.position,
                transform.position + transform.up * playerHeight,
                playerRadius,
                moveVectorX,
                moveDistance);

            if (canMove) {
                // can move at X direction
                moveVector = moveVectorX;
            }
            else {
                // try to Z movement
                Vector3 moveVectorZ = new Vector3(0, 0f, moveVector.z);
                canMove = (moveVector.z != 0) && !Physics.CapsuleCast(
                    transform.position,
                    transform.position + transform.up * playerHeight,
                    playerRadius,
                    moveVectorZ,
                    moveDistance);
                if (canMove) {
                    // move at Z direction
                    moveVector = moveVectorZ;
                }
                else {
                    // cannot move at X or Z direction
                    // so do nothing
                }
            }
        }

        if (canMove) {
            transform.position += (moveDistance * moveVector);
        }


        isWalking = (moveVector != Vector3.zero);

        float rotationSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveVector, rotationSpeed * Time.deltaTime);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {

        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(
                        this,
                        new OnSelectedCounterChangedEventsArgs {
                            selectedCounter = selectedCounter
                        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }
     
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}

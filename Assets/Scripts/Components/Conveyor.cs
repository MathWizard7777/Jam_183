using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite normal;
    public Sprite upCorner;
    public Sprite downCorner;

    void Start() {
        InitializeSprite();
    }

    void Update() {
        UpdateConveyorSprite();
    }

    private void InitializeSprite() {
        sr.sprite = normal; // Set the default sprite to normal
    }

    private void UpdateConveyorSprite() {
        Vector3 direction = GetDirection();

        Collider2D colliderOutput = GetColliderAtPosition(transform.position + direction);
        Collider2D colliderNormalInput = GetColliderAtPosition(transform.position - direction);
        Collider2D colliderUpInput = GetColliderAtPosition(transform.position + GetRotatedDirection(direction, 90));
        Collider2D colliderDownInput = GetColliderAtPosition(transform.position + GetRotatedDirection(direction, 270));

        if (colliderOutput != null) {
            UpdateSpriteBasedOnInput(colliderNormalInput, colliderUpInput, colliderDownInput);
        }
    }

    private Vector3 GetDirection() {
        return transform.rotation * Vector3.right; // Calculate the direction based on the conveyor's rotation
    }

    private Collider2D GetColliderAtPosition(Vector3 position) {
        return Physics2D.OverlapCircle(position, 0.1f); // Get the collider at the specified position
    }

    private Vector3 GetRotatedDirection(Vector3 direction, float angle) {
        return Quaternion.Euler(0, 0, angle) * direction; // Rotate the direction by the specified angle
    }

    private void UpdateSpriteBasedOnInput(Collider2D normalInput, Collider2D upInput, Collider2D downInput) {
        if (normalInput != null) {
            sr.sprite = normal; // Set the sprite to normal if there is a normal input
        } else if (upInput != null) {
            sr.sprite = upCorner; // Set the sprite to upCorner if there is an upward input
        } else if (downInput != null) {
            sr.sprite = downCorner; // Set the sprite to downCorner if there is a downward input
        }
    }
}
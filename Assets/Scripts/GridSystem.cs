using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ObjectType {
    Conveyor,
    Extractor,
    Adder,
    Subtractor,
    Incrementer,
    Decrementer,
    Multiplier,
    Multx2,
    Multx3,
    Multx5,
    Multx7,
    Splitter,
    Sorter,
    Decolorizer,
    Jump1,
    Jump2,
    Jump3
}

public class GridSystem : MonoBehaviour {
    public Camera cam; // Reference to the camera
    public MachineDataScriptableObject gameData; // Reference to the game data scriptable object
    public int tilesUsed;

    public GameObject conveyorPrefab; // Prefab for the conveyor belt
    public GameObject extractorPrefab; // Prefab for the extractor
    public GameObject adderPrefab; // Prefab for the adder
    public GameObject subtractorPrefab; // Prefab for the subtractor
    public GameObject incrementerPrefab; // Prefab for the incrementer
    public GameObject decrementerPrefab; // Prefab for the decrementer
    public GameObject multiplierPrefab; // Prefab for the multiplier
    public GameObject multx2Prefab; // Prefab for the multiplier x2
    public GameObject multx3Prefab; // Prefab for the multiplier x3
    public GameObject multx5Prefab; // Prefab for the multiplier x5
    public GameObject multx7Prefab; // Prefab for the multiplier x7
    public GameObject splitterPrefab; // Prefab for the splitter
    public GameObject sorterPrefab; // Prefab for the sorter
    public GameObject decolorizerPrefab; // Prefab for the decolorizer
    public GameObject jump1Prefab; // Prefab for the jump1
    public GameObject jump2Prefab; // Prefab for the jump2
    public GameObject jump3Prefab; // Prefab for the jump3

    private Quaternion objectRotation; // Rotation of the object
    private Vector3 objectPosition; // Position of the object
    private ObjectType objectSelection; // Type of object selected
    private GameObject selectedPrefab; // Prefab to be instantiated

    private bool spaceAtSelectedPosition; // If space is available at the selected position
    private List<GameObject> objectList; // List of objects in the scene
    private GameObject ghost; // Ghost object to show the position of the object

    private Color ghostColor; // Color of the ghost object
    private Color errorColor; // Color to indicate an error

    private GameObject lastPlacedObject; // Reference to the last placed object

    void Start() {
        InitializeVariables();
    }

    void Update() {
        UpdateMousePosition();
        UpdateSelectedPrefab();
        CheckSpaceAvailability();
        UpdateGhostObject();
        ConfigureGhostObject();
        HandlePlacement();
        HandleRotation();
        HandleDeletion();
        HandleHotKeySwitch();
        HandlePlacementSwitching();
    }

    private void InitializeVariables() {
        objectList = new List<GameObject>(); // Initialize the object list
        objectSelection = ObjectType.Conveyor; // Default object type
        objectRotation = Quaternion.identity; // Default rotation
        tilesUsed = 0; // Initialize tiles used

        ghostColor = Color.white;
        ghostColor.a = 0.5f; // Set the alpha value for transparency
        errorColor = Color.red; // Set the error color
        errorColor.a = 0.5f; // Set the alpha value for transparency

        gameData = LevelDataHolder.selectedLevelData;
    }

    private void UpdateMousePosition() {
        Vector2 mousepos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // Get the mouse position in world coordinates
        objectPosition = new Vector3(Mathf.Round(mousepos.x), Mathf.Round(mousepos.y), 0); // Snap to nearest grid point
    }

    private void UpdateSelectedPrefab() {
        if (objectSelection == ObjectType.Conveyor) {
            selectedPrefab = conveyorPrefab;
        } else if (objectSelection == ObjectType.Extractor) {
            selectedPrefab = extractorPrefab;
        } else if (objectSelection == ObjectType.Adder) {
            selectedPrefab = adderPrefab;
        } else if (objectSelection == ObjectType.Subtractor) {
            selectedPrefab = subtractorPrefab;
        } else if (objectSelection == ObjectType.Incrementer) {
            selectedPrefab = incrementerPrefab;
        } else if (objectSelection == ObjectType.Decrementer) {
            selectedPrefab = decrementerPrefab;
        } else if (objectSelection == ObjectType.Multiplier) {
            selectedPrefab = multiplierPrefab;
        } else if (objectSelection == ObjectType.Multx2) {
            selectedPrefab = multx2Prefab;
        } else if (objectSelection == ObjectType.Multx3) {
            selectedPrefab = multx3Prefab;
        } else if (objectSelection == ObjectType.Multx5) {
            selectedPrefab = multx5Prefab;
        } else if (objectSelection == ObjectType.Multx7) {
            selectedPrefab = multx7Prefab;
        } else if (objectSelection == ObjectType.Splitter) {
            selectedPrefab = splitterPrefab;
        } else if (objectSelection == ObjectType.Sorter) {
            selectedPrefab = sorterPrefab;
        } else if (objectSelection == ObjectType.Decolorizer) {
            selectedPrefab = decolorizerPrefab;
        } else if (objectSelection == ObjectType.Jump1) {
            selectedPrefab = jump1Prefab;
        } else if (objectSelection == ObjectType.Jump2) {
            selectedPrefab = jump2Prefab;
        } else if (objectSelection == ObjectType.Jump3) {
            selectedPrefab = jump3Prefab;
        }
    }

    private void CheckSpaceAvailability() {
        spaceAtSelectedPosition = true; // Space is available by default

        if (objectPosition.x < -9 || objectPosition.y < -4 || objectPosition.x > 4 || objectPosition.y > 5) {
            spaceAtSelectedPosition = false; // Space is not available if out of bounds
        }

        if (!IsSpaceAtPosition(objectPosition)) {
            spaceAtSelectedPosition = false; // Space is not available if an object is already at the position
        }

        if (objectSelection == ObjectType.Adder || objectSelection == ObjectType.Subtractor || objectSelection == ObjectType.Multiplier || objectSelection == ObjectType.Splitter) {
            Vector3 position2 = objectPosition + objectRotation * Vector3.down;
            if (!IsSpaceAtPosition(position2)) {
                spaceAtSelectedPosition = false; // Space is not available if an object is already at the position
            }
        }

        if (objectSelection == ObjectType.Sorter) {
            Vector3 position3 = objectPosition + objectRotation * Vector3.down * 2;
            Vector3 position4 = objectPosition + objectRotation * Vector3.down * 3;
            if (!IsSpaceAtPosition(position3) || !IsSpaceAtPosition(position4)) {
                spaceAtSelectedPosition = false; // Space is not available if an object is already at the position
            }
        }

        if (objectSelection == ObjectType.Extractor) {
            Collider2D resourceCollider = Physics2D.OverlapPoint(objectPosition, LayerMask.GetMask("Resource"));
            if (resourceCollider == null) {
                spaceAtSelectedPosition = false; // Space is not available if no resource is present
            }
        }
    }

    private void UpdateGhostObject() {
        if (ghost == null) {
            ghost = Instantiate(selectedPrefab, objectPosition, objectRotation); // Create a ghost object at the selected position
            ghost.GetComponent<Renderer>().material.color = ghostColor; // Set the ghost color
        }

        ghost.transform.position = objectPosition; // Move the ghost object to the new position
        ghost.transform.rotation = objectRotation; // Rotate the ghost object to match the selected rotation

        if (spaceAtSelectedPosition) {
            ghost.GetComponent<Renderer>().material.color = ghostColor; // Set the ghost color
        } else {
            ghost.GetComponent<Renderer>().material.color = errorColor; // Set the ghost color to error
        }
    }

    private void ConfigureGhostObject() {
        // Assign the ghost to a "Ghost" layer
        ghost.layer = LayerMask.NameToLayer("Ghost");

        // Disable colliders to prevent interaction
        Collider2D collider = ghost.GetComponent<Collider2D>();
        if (collider != null) {
            collider.enabled = false;
        }

        // Loop through and disable each script component on a GameObject
        MonoBehaviour[] scripts = ghost.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts) {
            script.enabled = false; // Disable the script
        }
        
        ghost.GetComponent<SpriteRenderer>().sortingLayerName = "Ghost";
    }

    private void HandlePlacement() {
        if (Mouse.current.leftButton.isPressed && spaceAtSelectedPosition) {
            if (lastPlacedObject != null && !Mouse.current.leftButton.wasPressedThisFrame && objectSelection == ObjectType.Conveyor) {
                AdjustRotationBasedOnLastObject();
            }
            lastPlacedObject = Instantiate(selectedPrefab, objectPosition, objectRotation);
            objectList.Add(lastPlacedObject);
            tilesUsed ++; // Increment the number of tiles used
        }
    }

    private bool IsSpaceAtPosition(Vector3 position) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.2f);
        bool isOnObject = false;
        foreach (var collider in colliders) {
            if (collider.CompareTag("Component")) {
                isOnObject = true;
                break;
            }
        }
        return !isOnObject; // Return true if the position is occupied by an object
    }

    private void AdjustRotationBasedOnLastObject() {
        if (Mathf.Abs(lastPlacedObject.transform.position.x - objectPosition.x + 1) < 0.01f && Mathf.Abs(lastPlacedObject.transform.position.y - objectPosition.y) < 0.01f) {
            objectRotation = Quaternion.Euler(0, 0, 0); // Right
            lastPlacedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if (Mathf.Abs(lastPlacedObject.transform.position.x - objectPosition.x) < 0.01f && Mathf.Abs(lastPlacedObject.transform.position.y - objectPosition.y - 1) < 0.01f) {
            objectRotation = Quaternion.Euler(0, 0, 270); // Down
            lastPlacedObject.transform.rotation = Quaternion.Euler(0, 0, 270);
        } else if (Mathf.Abs(lastPlacedObject.transform.position.x - objectPosition.x) < 0.01f && Mathf.Abs(lastPlacedObject.transform.position.y - objectPosition.y + 1) < 0.01f) {
            objectRotation = Quaternion.Euler(0, 0, 90); // Up
            lastPlacedObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (Mathf.Abs(lastPlacedObject.transform.position.x - objectPosition.x - 1) < 0.01f && Mathf.Abs(lastPlacedObject.transform.position.y - objectPosition.y) < 0.01f) {
            objectRotation = Quaternion.Euler(0, 0, 180); // Left
            lastPlacedObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void HandleRotation() {
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            objectRotation *= Quaternion.Euler(0, 0, 270); // Rotate the object by 90 degrees
        }
    }

    private void HandleDeletion() {
        if (Mouse.current.rightButton.isPressed) {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            for (int i = 0; i < objectList.Count; i++) {
                GameObject obj = objectList[i];
                if (Mathf.Abs(obj.transform.position.x - mousePosition.x) < 0.49f && Mathf.Abs(obj.transform.position.y - mousePosition.y) < 0.49f) {
                    Destroy(obj); // Destroy the object if it exists at the position
                    objectList.RemoveAt(i); // Remove the object from the list
                    tilesUsed --; // Decrement the number of tiles used 
                    break;
                }
            }
        }
    }

    private void CyclePlacemenmt() {
        if (objectSelection == ObjectType.Conveyor) {
            objectSelection = ObjectType.Extractor;
        } else if (objectSelection == ObjectType.Extractor) {
            objectSelection = ObjectType.Adder;
            if (!gameData.isAdderUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Adder) {
            objectSelection = ObjectType.Subtractor;
            if (!gameData.isSubtractorUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Subtractor) {
            objectSelection = ObjectType.Incrementer;
            if (!gameData.isIncrementerUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Incrementer) {
            objectSelection = ObjectType.Decrementer;
            if (!gameData.isDecrementerUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Decrementer) {
            objectSelection = ObjectType.Multiplier;
            if (!gameData.isMultiplierUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Multiplier) {
            objectSelection = ObjectType.Multx2;
            if (!gameData.isMultx2Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Multx2) {
            objectSelection = ObjectType.Multx3;
            if (!gameData.isMultx3Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Multx3) {
            objectSelection = ObjectType.Multx5;
            if (!gameData.isMultx5Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Multx5) {
            objectSelection = ObjectType.Multx7;
            if (!gameData.isMultx7Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Multx7) {
            objectSelection = ObjectType.Splitter;
            if (!gameData.isSplitterUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Splitter) {
            objectSelection = ObjectType.Sorter;
            if (!gameData.isSorterUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Sorter) {
            objectSelection = ObjectType.Decolorizer;
            if (!gameData.isDecolorizerUnlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Decolorizer) {
            objectSelection = ObjectType.Jump1;
            if (!gameData.isJump1Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Jump1) {
            objectSelection = ObjectType.Jump2;
            if (!gameData.isJump2Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Jump2) {
            objectSelection = ObjectType.Jump3;
            if (!gameData.isJump3Unlocked) { CyclePlacemenmt(); }
        } else if (objectSelection == ObjectType.Jump3) {
            objectSelection = ObjectType.Conveyor;
        }
    }

    private void HandlePlacementSwitching() {
        if (Keyboard.current.tabKey.wasPressedThisFrame) {
            // Cycle through the object types
            CyclePlacemenmt(); // Cycle through the object types
            // Update the selected prefab based on the new object type

            UpdateSelectedPrefab(); // Update the selected prefab
            Destroy(ghost); // Destroy the ghost object
            ghost = Instantiate(selectedPrefab, objectPosition, objectRotation); // Create a ghost object at the selected position
        }
    }

    private void HandleHotKeySwitch() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            objectSelection = ObjectType.Conveyor;
        } else if (Keyboard.current.digit1Key.wasPressedThisFrame) {
            objectSelection = ObjectType.Extractor;
        } else if (Keyboard.current.digit2Key.wasPressedThisFrame && gameData.isAdderUnlocked) {
            objectSelection = ObjectType.Adder;
        } else if (Keyboard.current.digit3Key.wasPressedThisFrame && gameData.isSubtractorUnlocked) {
            objectSelection = ObjectType.Subtractor;
        } else if (Keyboard.current.digit4Key.wasPressedThisFrame && gameData.isIncrementerUnlocked) {
            objectSelection = ObjectType.Incrementer;
        } else if (Keyboard.current.digit5Key.wasPressedThisFrame && gameData.isDecrementerUnlocked) {
            objectSelection = ObjectType.Decrementer;
        } else if (Keyboard.current.digit6Key.wasPressedThisFrame && gameData.isMultiplierUnlocked) {
            objectSelection = ObjectType.Multiplier;
        } else if (Keyboard.current.numpad2Key.wasPressedThisFrame && gameData.isMultx2Unlocked) {
            objectSelection = ObjectType.Multx2;
        } else if (Keyboard.current.numpad3Key.wasPressedThisFrame && gameData.isMultx3Unlocked) {
            objectSelection = ObjectType.Multx3;
        } else if (Keyboard.current.numpad5Key.wasPressedThisFrame && gameData.isMultx5Unlocked) {
            objectSelection = ObjectType.Multx5;
        } else if (Keyboard.current.numpad7Key.wasPressedThisFrame && gameData.isMultx7Unlocked) {
            objectSelection = ObjectType.Multx7;
        } else if (Keyboard.current.digit7Key.wasPressedThisFrame && gameData.isSplitterUnlocked) {
            objectSelection = ObjectType.Splitter;
        } else if (Keyboard.current.digit8Key.wasPressedThisFrame && gameData.isSorterUnlocked) {
            objectSelection = ObjectType.Sorter;
        } else if (Keyboard.current.digit0Key.wasPressedThisFrame && gameData.isDecolorizerUnlocked) {
            objectSelection = ObjectType.Decolorizer;
        } else if (Keyboard.current.numpad4Key.wasPressedThisFrame && gameData.isJump1Unlocked) {
            objectSelection = ObjectType.Jump1;
        } else if (Keyboard.current.numpad8Key.wasPressedThisFrame && gameData.isJump2Unlocked) {
            objectSelection = ObjectType.Jump2;
        } else if (Keyboard.current.numpad9Key.wasPressedThisFrame && gameData.isJump3Unlocked) {
            objectSelection = ObjectType.Jump3;
        } else {
            return;
        }

        UpdateSelectedPrefab(); // Update the selected prefab
        Destroy(ghost); // Destroy the ghost object
        ghost = Instantiate(selectedPrefab, objectPosition, objectRotation); // Create a ghost object at the selected position
    }
}
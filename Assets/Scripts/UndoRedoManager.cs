using UnityEngine;
using System.Collections.Generic;

// Represents one movement action (from old to new transform).
public class MovementAction
{
    public DragCharacter character;
    public Vector3 oldPosition;
    public Quaternion oldRotation;
    public Vector3 newPosition;
    public Quaternion newRotation;
}

public class UndoRedoManager : MonoBehaviour
{
    private Stack<MovementAction> undoStack = new Stack<MovementAction>();
    private Stack<MovementAction> redoStack = new Stack<MovementAction>();

    // UI buttons to enable/disable
    public UnityEngine.UI.Button undoButton;
    public UnityEngine.UI.Button redoButton;

    private void Start()
    {
        UpdateButtonInteractable();  // At the start, likely both are empty
    }
    private void Update()
    {
        UpdateButtonInteractable();
    }

    /// <summary>
    /// Call this when a character finishes moving.
    /// We'll record the movement in the undo stack and clear the redo stack.
    /// </summary>
    public void RecordMove(DragCharacter character, Vector3 oldPos, Quaternion oldRot, Vector3 newPos, Quaternion newRot)
    {
        // Don't record trivial moves
        if (oldPos == newPos && oldRot == newRot) return;

        MovementAction action = new MovementAction()
        {
            character = character,
            oldPosition = oldPos,
            oldRotation = oldRot,
            newPosition = newPos,
            newRotation = newRot
        };

        // Push to Undo stack
        undoStack.Push(action);

        // Once a new move is made, we clear the Redo stack 
        // (typical undo/redo behavior in many apps).
        redoStack.Clear();

        UpdateButtonInteractable();
    }

    /// <summary>
    /// Undo the last move: pop from undoStack, revert, push onto redoStack.
    /// </summary>
    public void Undo()
    {
        UpdateButtonInteractable();

        if (undoStack.Count > 0)
        {
            MovementAction action = undoStack.Pop();

            // Revert the position and rotation
            action.character.transform.position = action.oldPosition;
            action.character.transform.rotation = action.oldRotation;

            // Push this action onto redoStack so we can reapply it if needed
            redoStack.Push(action);

            UpdateButtonInteractable();
        }
    }

    /// <summary>
    /// Redo the last undone move: pop from redoStack, apply, push onto undoStack.
    /// </summary>
    public void Redo()
    {
        UpdateButtonInteractable();

        if (redoStack.Count > 0)
        {
            MovementAction action = redoStack.Pop();

            // Re-apply the move
            action.character.transform.position = action.newPosition;
            action.character.transform.rotation = action.newRotation;

            // Push back onto undoStack
            undoStack.Push(action);

            UpdateButtonInteractable();
        }
    }

    /// <summary>
    /// Clears undo/redo stacks, typically after a Save, so no further undo/redo is possible.
    /// </summary>
    public void ClearHistory()
    {
        undoStack.Clear();
        redoStack.Clear();
        UpdateButtonInteractable();
    }

    private void UpdateButtonInteractable()
{
    if (undoButton != null)
    {
        bool canUndo = (undoStack.Count > 0);
        if (undoButton.interactable != canUndo)
        {
            Debug.Log("Setting Undo button interactable to: " + canUndo);
        }
        undoButton.interactable = canUndo;
    }

    if (redoButton != null)
    {
        bool canRedo = (redoStack.Count > 0);
        if (redoButton.interactable != canRedo)
        {
            Debug.Log("Setting Redo button interactable to: " + canRedo);
        }
        redoButton.interactable = canRedo;
    }
}

}

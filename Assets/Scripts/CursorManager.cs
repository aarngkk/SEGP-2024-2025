using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture; // Custom cursor texture to be used

    private Vector2 cursorHotspot; // The active point of the cursor (pivot)

    // Initialize custom cursor when the game starts
    void Start()
    {
        cursorHotspot = new Vector2(0, 64); // Set hotspot offset (x,y from top-left)
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto); // Apply custom cursor
    }
}

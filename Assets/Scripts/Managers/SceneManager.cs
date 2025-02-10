using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerCustom : MonoBehaviour
{
    void Start()
    {
        // Hides the cursor
        Cursor.visible = false;

        // Locks the cursor to the center of the game screen
        Cursor.lockState = CursorLockMode.Locked;
    }
}

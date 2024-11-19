using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

namespace Assets.Scripts
{
    public class LevelUnlocker : MonoBehaviour
    {
        public Button level1, level2, level3, level4, level5;
    
        // Start is called before the first frame update
        void Start()
        {
            //Return Cursor control
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            int lastUnlockedLevel = PlayerPrefs.GetInt("FurthestLevel");
            if (lastUnlockedLevel >= 2)
                level2.interactable = true;
            if (lastUnlockedLevel >= 3)
                level3.interactable = true;
            if (lastUnlockedLevel >= 4)
                level4.interactable = true;
            if (lastUnlockedLevel >= 5)
                level5.interactable = true;

        }

    
    }
}

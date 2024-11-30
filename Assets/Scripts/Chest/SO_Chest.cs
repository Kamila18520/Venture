using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Chect", menuName = "ScriptableObjects/SO_Chest", order = 1)]
public class SO_Chest : ScriptableObject
{
    public bool allChestAreOpen;

    public List<bool> chestsOpened = new List<bool>();

    public void ClearSOChest()
    {
        chestsOpened.Clear();
        allChestAreOpen = false;
    }
    public void AddChest()
    {
        chestsOpened.Add(false);
    }

    public void OpenChest(ChestUIController chestUIController)
    {
        for (int i = 0; i < chestsOpened.Count; i++)
        {
            if (!chestsOpened[i])
            {
                chestsOpened[i] = true;

                if (i == chestsOpened.Count - 1)
                {
                    allChestAreOpen = true;
                }

               
                chestUIController.UpdateChestCount();

                return;
            }
        }
    }


}

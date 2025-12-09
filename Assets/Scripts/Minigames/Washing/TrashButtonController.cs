using System.Collections.Generic;
using UnityEngine;

public class TrashButtonController : MonoBehaviour
{
    public void Trash()
    {
        List<VeggieController> veggies = new List<VeggieController>(FindObjectsByType(typeof(VeggieController), FindObjectsSortMode.None) as VeggieController[]);
        foreach (var veggie in veggies)
        {
            if (veggie.dirtyness > 0f)
            {
                Destroy(veggie.gameObject);
            }
        }
    }
}

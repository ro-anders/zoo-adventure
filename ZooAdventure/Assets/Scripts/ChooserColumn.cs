using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooserColumn : MonoBehaviour
{
    public AnimalTile topTile;
    public AnimalTile bottomTile;

    public void SetContents(AnimalConfig animal1, AnimalConfig animal2,
        AnimalChooser chooser)
    {
        topTile.Name = animal1.Title;
        if (animal1.ResourceName != "")
        {
            topTile.Image = animal1.ResourceName;
        }
        topTile.Chooser = chooser;
        if (animal2 != null)
        {
            bottomTile.Name = animal2.Title;
            if (animal2.ResourceName != "")
            {
                bottomTile.Image = animal2.ResourceName;
            }
            bottomTile.Chooser = chooser;
        }
        else
        {
            bottomTile.gameObject.SetActive(false);
        }
    }
}

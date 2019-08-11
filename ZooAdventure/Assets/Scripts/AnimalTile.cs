using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalTile : MonoBehaviour
{
    public Text nameText;
    public Image image;
    private string animalName;
    private string imageName;
    private AnimalChooser chooser;

    public string Name
    {
        set
        {
            animalName = value; 
            nameText.text = animalName;
        }
    }
    public string Image
    {
        set
        {
            imageName = value;
            if (imageName != "")
            {
                Sprite loaded = Resources.Load<Sprite>("Sprites/" + imageName);
                if (loaded != null)
                {
                    image.sprite = loaded;
                }
            } 
        }
    }
    public AnimalChooser Chooser
    {
        set { chooser = value; }
    }

    public void OnClick()
    {
        chooser.OnAnimalClick(animalName);
    }
}

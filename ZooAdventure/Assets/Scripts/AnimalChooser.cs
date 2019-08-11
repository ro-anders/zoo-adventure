using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalChooser : MonoBehaviour
{
    

    public GameObject columnContainer;
    public GameObject columnPrefab;
    public AnimalView animalView;
    private AnimalConfig[] animals;
    private bool animalViewOpen = false;

    void Start()
    {
        animalView.gameObject.SetActive(false);
    }

    public void LoadConfig(AnimalConfig[] inAnimals)
    {
        animals = inAnimals;
        for (int i = 1; i <= animals.Length; i += 2)
        {
            AnimalConfig animal1 = animals[i - 1];
            AnimalConfig animal2 = (i < animals.Length ? animals[i] : null);
            // Create a column 
            GameObject nextGameObject = Instantiate(columnPrefab);
            nextGameObject.transform.SetParent(columnContainer.transform, false);
            ChooserColumn nextEntry = nextGameObject.GetComponent<ChooserColumn>();
            nextEntry.SetContents(animal1, animal2, this);
        }
    }

    public void OnAnimalClick(string animal)
    {
        AnimalConfig lookingFor = null;
        foreach (AnimalConfig next in animals)
        {
            if (next.Title == animal)
            {
                lookingFor = next;
                break;
            }
        }
        UnityEngine.Debug.Log(animal + " clicked.");
        if (!animalViewOpen && (lookingFor != null))
        {
            animalViewOpen = true;
            animalView.gameObject.SetActive(true);
            animalView.FindAnimal(lookingFor);
        }

    }

    public void OnAnimalViewClosed()
    {
        animalView.gameObject.SetActive(false);
        animalViewOpen = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooAdventure : MonoBehaviour
{
    public AnimalChooser animalChooser;

    // Start is called before the first frame update
    void Start()
    {
        AnimalConfig[] animals = new AnimalConfig[] {
            new AnimalConfig("Sumatran Tiger", "tiger"),
            new AnimalConfig("Sloth Bear", "slothbear"),
            new AnimalConfig("American Bison", "bison"),
            new AnimalConfig("Bald Eagle", ""),
            new AnimalConfig("Lion", ""),
            new AnimalConfig("Indian Elephant", ""),
            new AnimalConfig("Serval", "")
        };
        animalChooser.LoadConfig(animals);
    }

}

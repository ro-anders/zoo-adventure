using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooAdventure : MonoBehaviour
{
    public AnimalChooser animalChooser;

    // Start is called before the first frame update
    void Start()
    {
        AnimalConfig[] animals =
        {
             new AnimalConfig(
                "Sloth Bear", "slothbear",
                new Region[]{ new Region(38.929753f, -77.053509f, 38.930164f, -77.054554f), // Zoo
                              new Region(38.973269f, -76.999597f, 38.972892f, -76.999382f) } // Home
            ),
            new AnimalConfig(
                "Red Panda", "redpanda",
                new Region[]{new Region(38.930775f, -77.053425f, 38.930507f, -77.052835f) } // Zoo
            ),
            new AnimalConfig(
                "Beaver", "beaver",
                new Region[]{new Region(38.973392f, -77.000546f, 38.973108f, -77.000170f) } // Home
            ),
            new AnimalConfig(
                "Otter", "otter",
                    new Region[]{ new Region(38.930263f, -77.053329f, 38.930545f, -77.053567f), // Zoo
                    new Region(38.973269f, -76.999597f, 38.972892f, -76.999382f) } // Home
            )
        };
        // Add the tiger as a sample animal when we're developing
        if (Application.isEditor)
        {
            var animalList = new List<AnimalConfig>(animals);
            animalList.Insert(0, new AnimalConfig("Tiger", "tiger", new Region[] { }));
            animals = animalList.ToArray();

        }
        animalChooser.LoadConfig(animals);
    }

}

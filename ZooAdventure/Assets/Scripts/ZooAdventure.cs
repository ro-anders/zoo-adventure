﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooAdventure : MonoBehaviour
{
    public AnimalChooser animalChooser;

    // Start is called before the first frame update
    void Start()
    {
        AnimalConfig[] animals = {
            new AnimalConfig(
                "Sumatran Tiger", "tiger",
                new Region[]{new Region(-100, 50, 0.4f, 0.5f)}
            ),
            new AnimalConfig(
                "Beaver", "beaver",
                new Region[]{new Region(-100, 50, 0.4f, 0.5f)}
            ),
            new AnimalConfig(
                "Sloth Bear", "slothbear",
                 new Region[]{new Region(-100, 50, 0.4f, 0.5f)}
            ),
            new AnimalConfig(
                "American Bison", "bison",
                new Region[]{new Region(-100, 50, 0.4f, 0.5f)}
            ),
            new AnimalConfig("Bald Eagle", "", new Region[]{}),
            new AnimalConfig("Indian Elephant", "", new Region[]{}),
            new AnimalConfig("Serval", "", new Region[]{})
        };
        animalChooser.LoadConfig(animals);
    }

}

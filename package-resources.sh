#!/bin/bash

rm -rf /tmp/zoo-adventure-resources
mkdir /tmp/zoo-adventure-resources
mkdir /tmp/zoo-adventure-resources/Sounds
mkdir /tmp/zoo-adventure-resources/Sprites
mkdir /tmp/zoo-adventure-resources/Videos
cp ZooAdventure/Assets/Resources/Videos/*.mov /tmp/zoo-adventure-resources/Videos
cp ZooAdventure/Assets/Resources/Sounds/*.mp3 /tmp/zoo-adventure-resources/Sounds
cp ZooAdventure/Assets/Resources/Sprites/*.jpg /tmp/zoo-adventure-resources/Sprites
cd /tmp/zoo-adventure-resources
tar cvfz /tmp/zoo-adventure-resources.zip *

using System;

public class Region
{
    public readonly float north;
    public readonly float south;
    public readonly float east;
    public readonly float west;

    public Region(float inNorth, float inSouth, float inWest, float inEast)
    {
        north = inNorth;
        south = inSouth;
        west = inWest;
        east = inEast;
    }

    public Region(Region other)
    {
        north = other.north;
        south = other.south;
        west = other.west;
        east = other.east;
    }
}

public class AnimalConfig
{
    private string title;
    private string resourceName;
    private Region[] regions;

    public string Title
    {
        get { return title; }
    }
    public string ResourceName
    {
        get { return resourceName; }
    }
    public Region[] Regions
    {
        get { return regions; }
    }

    public AnimalConfig(string inTitle, string inResourceName, Region[] inRegions)
    {
        title = inTitle;
        resourceName = inResourceName;
        regions = new Region[inRegions.Length];
        for (int ctr=0; ctr<inRegions.Length; ++ctr)
        {
            regions[ctr] = new Region(inRegions[ctr]);
        }
    }
}

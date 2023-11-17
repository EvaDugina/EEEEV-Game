
using System;
using System.Collections.Generic;

public class Level
{
    public Area MainArea { get; private set; }
    public List<Area> SecondaryAreas { get; private set; }

    public Level(Area mainArea)
    {
        MainArea = mainArea;
        SecondaryAreas = new List<Area>();
    }

    public List<Area> GetAllAreas()
    {
        List<Area> areas = new List<Area>
        {
            MainArea
        };
        areas.AddRange(SecondaryAreas);
        return areas;
    }

    public Area GetAreaById(int id) { 
        if (id == MainArea.Id) 
            return MainArea;
        
        foreach (Area area in SecondaryAreas)
            if (id == area.Id)
                return area;

        throw new ArgumentException("Area с таким Name не существует!");
    }


}

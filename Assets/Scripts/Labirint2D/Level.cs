
using System.Collections.Generic;

public class Level
{
    public Area MainArea;
    public List<Area> SecondaryAreas;

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

    public Area GetAreaByName(int id) { 
        if (id == MainArea.Id) 
            return MainArea;
        
        foreach (Area area in SecondaryAreas)
            if (id == area.Id)
                return area;

        return null;
    }


}

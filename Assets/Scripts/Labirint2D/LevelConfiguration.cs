using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Parameters
{
    [NonSerialized] public AreaType Type;
    public bool Status;

    public GenerateParams GenerateParams;
    public SpawnParams SpawnParams;
}

[System.Serializable]
public struct GenerateParams
{
    [Range(0.1f, 1.0f)]
    public float SizeKoeff;
}

[System.Serializable]
public struct SpawnParams
{
    public CellDecoration Decoration;
    public CellSpawnParameters CellParameters;
}

[System.Serializable]
public struct CellSpawnParameters {
    public Vector3Int Size;
}

public enum CellDecoration
{
    Empty, WheatField, BirchField, RedRoom
}


public class LevelConfiguration
{

    private List<Parameters> LevelParameters;


    public LevelConfiguration()
    {
        LevelParameters = new List<Parameters>
        {
            //GenerateMainAreaParams()
        };
    }



    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   Methods
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */

    public void AddAreaParamsToList(Parameters parameters)
    {
        LevelParameters.Add(parameters);
    }


    public List<Parameters> GetParametersList()
    {
        return LevelParameters;
    }


    public Parameters GetParametersByAreaType(AreaType type) {
        foreach (Parameters parameters in LevelParameters)
            if (type == parameters.Type) return parameters;
        return new Parameters();
    }

    public List<GenerateParams> GetAreasGenerateParams()
    {
        List<GenerateParams> generateParams = new List<GenerateParams> ();
        foreach (Parameters parameters in LevelParameters) {
            generateParams.Add(parameters.GenerateParams);
        }
        return generateParams;
    }

    public SpawnParams GetAreaSpawnParamsByType(AreaType type)
    {
        foreach (Parameters areaParams in LevelParameters)
            if (areaParams.Type == type)
                return areaParams.SpawnParams;
        return new Parameters().SpawnParams;
    }


    /* 
    ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
    |   Utilities
    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── 
    */


    private Parameters GenerateMainAreaParams()
    {
        return new Parameters()
        {
            Type = AreaType.Main,
            Status = true,

            GenerateParams = new GenerateParams()
            {
                SizeKoeff = 1,
            },
            SpawnParams = new SpawnParams()
            {
                Decoration = CellDecoration.Empty,
                CellParameters = new CellSpawnParameters()
                {
                    Size = new Vector3Int(1, 1, 1)
                }
            }
        };
    }

}
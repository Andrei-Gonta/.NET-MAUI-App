using System.Collections.Generic;
using CarMeetApp.Models;

namespace CarMeetApp.Services;

public class CarDataService
{
    private static readonly Dictionary<string, Dictionary<string, List<CarGenerationOption>>> _carData = new()
    {
        ["AUDI"] = new()
        {
            ["A4"] = new()
            {
                new("B8 (2007-2015)", 211, 2.0),
                new("B9 (2015-2020)", 248, 2.0),
                new("B9.5 (2020+)", 261, 2.0)
            },
            ["A6"] = new()
            {
                new("C7 (2011-2018)", 252, 2.0),
                new("C8 (2018-2023)", 335, 3.0),
                new("C8.5 (2023+)", 340, 3.0)
            },
            ["Q5"] = new()
            {
                new("8R (2008-2017)", 220, 2.0),
                new("FY (2017-2020)", 248, 2.0),
                new("FY.5 (2020+)", 361, 3.0)
            },
            ["TT"] = new()
            {
                new("8J (2006-2014)", 211, 2.0),
                new("8S (2014-2018)", 220, 2.0),
                new("8S.5 (2018+)", 228, 2.0)
            }
        },
        ["BMW"] = new()
        {
            ["3 Series"] = new()
            {
                new("E90 (2005-2013)", 230, 3.0),
                new("F30 (2012-2019)", 248, 2.0),
                new("G20 (2019+)", 255, 2.0)
            },
            ["5 Series"] = new()
            {
                new("E60 (2003-2010)", 300, 3.0),
                new("F10 (2010-2017)", 315, 3.0),
                new("G30 (2017+)", 335, 3.0)
            },
            ["X5"] = new()
            {
                new("E70 (2006-2013)", 300, 3.0),
                new("F15 (2013-2018)", 300, 3.0),
                new("G05 (2018+)", 335, 3.0)
            },
            ["M3"] = new()
            {
                new("E92 (2007-2013)", 414, 4.0),
                new("F80 (2014-2020)", 425, 3.0),
                new("G80 (2020+)", 473, 3.0)
            }
        },
        ["VW"] = new()
        {
            ["Golf"] = new()
            {
                new("Mk6 (2008-2013)", 170, 2.0),
                new("Mk7 (2012-2020)", 210, 2.0),
                new("Mk8 (2020+)", 241, 2.0)
            },
            ["Passat"] = new()
            {
                new("B7 (2010-2014)", 170, 2.0),
                new("B8 (2014-2019)", 174, 1.8),
                new("B8.5 (2019+)", 238, 2.0)
            },
            ["Tiguan"] = new()
            {
                new("Mk1 (2007-2016)", 200, 2.0),
                new("Mk2 (2016-2020)", 184, 2.0),
                new("Mk2.5 (2020+)", 228, 2.0)
            },
            ["Polo"] = new()
            {
                new("Mk5 (2009-2017)", 95, 1.2),
                new("Mk6 (2017-2021)", 95, 1.0),
                new("Mk6.5 (2021+)", 110, 1.0)
            }
        },
        ["MERCEDES"] = new()
        {
            ["C-Class"] = new()
            {
                new("W204 (2007-2014)", 201, 1.8),
                new("W205 (2014-2021)", 241, 2.0),
                new("W206 (2021+)", 255, 2.0)
            },
            ["E-Class"] = new()
            {
                new("W212 (2009-2016)", 302, 3.0),
                new("W213 (2016-2020)", 329, 3.0),
                new("W213.5 (2020+)", 362, 3.0)
            },
            ["GLC"] = new()
            {
                new("X253 (2015-2019)", 241, 2.0),
                new("X253.5 (2019-2022)", 255, 2.0),
                new("X254 (2022+)", 258, 2.5)
            },
            ["A-Class"] = new()
            {
                new("W176 (2012-2018)", 156, 1.6),
                new("W177 (2018-2022)", 221, 2.0),
                new("W177.5 (2022+)", 302, 2.0)
            }
        }
    };

    public async Task<List<string>> GetBrandsAsync()
    {
        return await Task.FromResult(_carData.Keys.ToList());
    }

    public async Task<List<string>> GetModelsAsync(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand) || !_carData.ContainsKey(brand))
            return await Task.FromResult(new List<string>());

        return await Task.FromResult(_carData[brand].Keys.ToList());
    }

    public async Task<List<CarGenerationOption>> GetGenerationsAsync(string brand, string model)
    {
        if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
            return await Task.FromResult(new List<CarGenerationOption>());

        if (!_carData.ContainsKey(brand) || !_carData[brand].ContainsKey(model))
            return await Task.FromResult(new List<CarGenerationOption>());

        return await Task.FromResult(_carData[brand][model]);
    }
}

using System;
using ExcelDna.Integration; // Assuming Excel-DNA for your add-in
using FSvolLib.Interop;     // Points to your interop DLL
using System.Collections.Generic;
using System.Text;

namespace FSvolLib.Xl;

public static class TreeFunctions
{
    // --- Helper Methods to Eliminate Duplication ---

    internal static VolumeCalculationOptions CreateOptions(int region, int forest, int district, int fiaCode, int product, string? volumeEquationOverride = null)
    {
        return new VolumeCalculationOptions
        {
            Region = region,
            Forest = forest,
            District = district,
            FiaCode = fiaCode,
            PrimaryProduct = product,
            SecondaryProduct = 2,
            VolumeEquationNumberOverride = volumeEquationOverride
        };
    }

    internal static VolumeCalculationOptions CreateOptions2(int region, int forest, int district, int fiaCode, int product, string volumeEquationOverride)
    {
        return new VolumeCalculationOptions
        {
            Region = region,
            Forest = forest,
            District = district,
            FiaCode = fiaCode,
            PrimaryProduct = product,
            SecondaryProduct = 2,
            VolumeEquationNumberOverride = volumeEquationOverride
        };
    }

    internal static TreeMeasurment CreateTreeMeasurement(
        double dbh, double totalHeight, 
        int product, 
        double merchHeight, 
        double merchTopDiameter, 
        double referenceHeight, 
        double referenceTopDiameter, 
        double heightToTopBroken, 
        double topBrokenDiameter,
        int formClass,
        double drc,
        int numberOfStems,
        double cull)
    {
        return new TreeMeasurment
        {
            Dbh = dbh,
            TotalHeight = totalHeight,
            MerchHeightSaw = (product == 1) ? merchHeight : 0.0,
            MinTopDibSawOverride = (product == 1) ? merchTopDiameter : -1.0,
            MerchHeightNonsaw = (product != 1) ? merchHeight : 0.0,
            MinTopDibNonSawOverride = (product != 1) ? merchTopDiameter : -1.0,
            ReferenceHeight = referenceHeight,
            ReferenceDiameter = referenceTopDiameter,
            HeightToTopBroken = heightToTopBroken,
            TopBrokenDiameter = topBrokenDiameter,
            FormClass = formClass,
            Drc = drc,
            Stems = numberOfStems,
            Cull = cull
        };
    }

    internal static T ExecuteAndExtract<T>(VolumeCalculationOptions options, TreeMeasurment measurement, Func<TreeOutput, T> valueExtractor, T defaultValue)
    {
        FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);
        return (result.Success && result.TreeOutput != null) ? valueExtractor(result.TreeOutput) : defaultValue;
    }

    internal static double ExecuteAndExtractBiomass(
        VolumeCalculationOptions options,
        TreeMeasurment measurement,
        Func<BiomassOutput, double> componentSelector,
        string greenOrDry)
    {
        return ExecuteAndExtract<double>(options, measurement, t =>
        {
            // Parse once for all components
            bool isDry = !string.IsNullOrEmpty(greenOrDry) &&
                         greenOrDry.StartsWith("D", StringComparison.OrdinalIgnoreCase);

            // Pick the correct biomass sub-struct (DryBio vs GreenBio) and run the selector
            return componentSelector(isDry ? t.DryBio : t.GreenBio);
        }, 0.0);
    }

    internal static double ExecuteAndExtractLog(
        VolumeCalculationOptions options,
        TreeMeasurment measurement,
        int logNumber,
        Func<LogOutput, double> logPropertySelector)
    {
        return ExecuteAndExtract<double>(options, measurement, t =>
        {
            // Guard against null collections or invalid log number requests
            if (t.Logs == null || logNumber < 1 || logNumber > t.Logs.Count)
            {
                return 0.0; // Return zero safely if the log doesn't exist
            }

            // Convert Excel 1-based index to C# 0-based index
            LogOutput targetLog = t.Logs[logNumber - 1];

            return logPropertySelector(targetLog);
        }, 0.0);
    }


    [ExcelFunction(Description = "Calculate Tree Stem Total Cubic Volume from ground to tip")]
    public static double Tree_TotalCubicVolume2(
        int region, 
        int forest, 
        int fiaSpeciesCode, 
        double dbh, 
        double totalHeight, 
        int district = 0,
        int product = 1,
        double merchHeight = 0.0,
        double merchTopDiameter = 0.0,
        int formClass = 0,
        double referenceHeight = 0.0,
        double referenceTopDiameter = 0.0,
        double heightToTopBroken = 0.0,
        double topBrokenDiameter = 0.0,
        string volumeEquationOverride = "",
        double drc = 0.0,
        int numberOfStems = 1,
        double cull = 0.0
        )
    {
        //var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var measurementWithOverrides = CreateTreeMeasurement(
            dbh, 
            totalHeight, 
            product, 
            merchHeight, 
            merchTopDiameter, 
            referenceHeight, 
            referenceTopDiameter, 
            heightToTopBroken, 
            topBrokenDiameter,
            formClass,
            drc,
            numberOfStems,
            cull
        );
        var options = CreateOptions2(region, forest, district, fiaSpeciesCode, product, volumeEquationOverride);

        return ExecuteAndExtract<double>(options, measurementWithOverrides, result => result.TotalCubicFoot, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Total Cubic Volume from ground to tip")]
    public static double Tree_TotalCubicVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, 
        int district = 1, int product = 1, string volumeEquationOverride = "")
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product, volumeEquationOverride);

        return ExecuteAndExtract<double>(options, measurement, result => result.TotalCubicFoot, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Merch Cubic Volume")]
    public static double Tree_MerchCubicVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameter = 0.0, int district = 1, int product = 1)
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh,
            //MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0,
            //MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product);
        if(topDiameter > 0.0)
        {
            if (product == 1)
            {
                measurement.MinTopDibSawOverride = topDiameter;
            }
            else
            {
                measurement.MinTopDibNonSawOverride = topDiameter;
            }
        }
        return ExecuteAndExtract<double>(options, measurement, result => result.GrossCubicFootPrimary, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Merch Cubic Volume")]
    public static double Tree_MerchCubicVolume2(
        int region,
        int forest,
        int fiaSpeciesCode,
        double dbh,
        double totalHeight,
        int district = 0,
        int product = 1,
        double merchHeight = 0.0,
        double merchTopDiameter = 0.0,
        int formClass = 0,
        double referenceHeight = 0.0,
        double referenceTopDiameter = 0.0,
        double heightToTopBroken = 0.0,
        double topBrokenDiameter = 0.0,
        string volumeEquationOverride = "",
        double drc = 0.0,
        int numberOfStems = 1,
        double cull = 0.0)
    {
        var measurement = CreateTreeMeasurement(
            dbh,
            totalHeight,
            product,
            merchHeight,
            merchTopDiameter,
            referenceHeight,
            referenceTopDiameter,
            heightToTopBroken,
            topBrokenDiameter,
            formClass,
            drc,
            numberOfStems,
            cull
        );
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product, volumeEquationOverride);

        return ExecuteAndExtract<double>(options, measurement, result => result.GrossCubicFootPrimary, 0.0);
    }


    [ExcelFunction(Description = "Calculate Tree Boardfoot Volume")]
    public static double Tree_GrossBoardFootVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameter, int district = 1, int product = 1)
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh,
            MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0,
            MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product);

        return ExecuteAndExtract<double>(options, measurement, result => result.GrossBoardFootPrimary, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree International Boardfoot Volume")]
    public static double Tree_GrossInternationalBoardFootVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameter, int district = 1, int product = 1)
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh,
            MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0,
            MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product);

        return ExecuteAndExtract<double>(options, measurement, result => result.GrossInternationalBoardFoot, 0.0);
    }

    [ExcelFunction(Description = "Calculate Saw Tree Topwood Cubic Volume")]
    public static double Tree_TopwoodCubicVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameterSaw, double topDiameterNonsaw, int district)
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh,
            MinTopDibSawOverride = topDiameterSaw,
            MinTopDibNonSawOverride = topDiameterNonsaw
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product: 1); // Fixed primary product to 1

        return ExecuteAndExtract<double>(options, measurement, result => result.GrossCubicFootSecondary, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Stump Cubic Volume")]
    public static double Tree_StumpCubicVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double stumpHeight, int district, int product)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh, StumpHeightOverride = stumpHeight };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product);

        return ExecuteAndExtract<double>(options, measurement, result => result.StumpCubicFoot, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Tip Cubic Volume")]
    public static double Tree_TipCubicVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameter, int district)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh, MinTopDibNonSawOverride = topDiameter };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product: 1); // Fixed primary product to 1

        return ExecuteAndExtract<double>(options, measurement, result => result.TipCubicFoot, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Merch Cord Volume")]
    public static double Tree_CordVolume(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameter, int district)
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh,
            MinTopDibNonSawOverride = topDiameter
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product: 2); // Fixed primary product to 2

        return ExecuteAndExtract<double>(options, measurement, t => t.CordMerchantable, 0.0);
    }

    [ExcelFunction(Description = "Calculate Tree Number of logs")]
    public static int Tree_NumberOfLogs(int region, int forest, int fiaSpeciesCode, double dbh, double totalHeight, double topDiameter, int district, int product)
    {
        var measurement = new TreeMeasurment
        {
            TotalHeight = totalHeight,
            Dbh = dbh,
            MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0,
            MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
        };
        var options = CreateOptions(region, forest, district, fiaSpeciesCode, product);

        return ExecuteAndExtract<int>(options, measurement, t => t.NumberOfLogs, 0);
    }

    //[ExcelFunction(Description = "Calculate Tree Height to a given top diameter")]
    //public static double Tree_HeightToTopDiameter(string voleq, double dbh, double totalHeight, double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh
    //    };
    //    var options = new VolumeCalculationOptions
    //    {
    //        VolumeEquationNumberOverride = voleq
    //    };

    //    return 0.0;
    //    //return VolumeLibrary.Get
    //}

    //[ExcelFunction(Description = "Calculate Tree Total Cubic Volume")]
    //public static double TotalCubicVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight)
    //{
    //    // 1. Pack variables into the interop classes
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = product
    //    };

    //    // 2. Call the interop (which intercepts via cache automatically)
    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    // 3. Return result if successful, otherwise return 0.0 (or handle error)
    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.TotalCubicFoot;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree Merch Cubic Volume")]
    //public static double MerchCubicVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0, // Mapping topDiameter to the override
    //        MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = product
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.GrossCubicFootPrimary;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree Boardfoot Volume")]
    //public static double GrossBoardFootVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0, // Mapping topDiameter to the override
    //        MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = product
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.GrossBoardFootPrimary;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree International Boardfoot Volume")]
    //public static double GrossInternationalBoardFootVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0, // Mapping topDiameter to the override
    //        MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = product
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.GrossInternationalBoardFoot;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Saw Tree Topwood Cubic Volume")]
    //public static double TopwoodCubicVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameterSaw,
    //    double topDiameterNonsaw)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibSawOverride = topDiameterSaw, // Mapping topDiameter to the override
    //        MinTopDibNonSawOverride = topDiameterNonsaw
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = 1
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.GrossCubicFootSecondary;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree Stump Cubic Volume")]
    //public static double StumpCubicVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight,
    //    double stumpHeight)
    //{
    //    // 1. Pack variables into the interop classes
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        StumpHeightOverride = stumpHeight
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = product
    //    };

    //    // 2. Call the interop (which intercepts via cache automatically)
    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    // 3. Return result if successful, otherwise return 0.0 (or handle error)
    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.StumpCubicFoot;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree Tip Cubic Volume")]
    //public static double TipCubicVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibNonSawOverride = topDiameter
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = 1
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.TipCubicFoot;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree Merch Cord Volume")]
    //public static double MerchCordVolume(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibNonSawOverride = topDiameter
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = 2
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.CordMerchantable;
    //    }

    //    return 0.0;
    //}

    //[ExcelFunction(Description = "Calculate Tree Number of logs")]
    //public static int NumberOfLogs(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight,
    //    double topDiameter)
    //{
    //    var measurement = new TreeMeasurment
    //    {
    //        TotalHeight = totalHeight,
    //        Dbh = dbh,
    //        MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0, // Mapping topDiameter to the override
    //        MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
    //    };

    //    var options = new VolumeCalculationOptions
    //    {
    //        FiaCode = fiaCode,
    //        Region = region,
    //        Forest = forest,
    //        District = district,
    //        PrimaryProduct = product
    //    };

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        return result.TreeOutput.NumberOfLogs;
    //    }

    //    return 0;
    //}
}

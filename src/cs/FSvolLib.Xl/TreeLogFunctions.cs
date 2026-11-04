using System;
using ExcelDna.Integration; // Assuming Excel-DNA for your add-in
using FSvolLib.Interop;     // Points to your interop DLL
using System.Collections.Generic;
using System.Text;


namespace FSvolLib.Xl;

public static class TreeLogFunctions
{
    //[ExcelFunction(Description = "Calculate Gross Cubic Foot Volume for a specific log number (1-indexed)")]
    //public static double LogGrossCubicFoot(
    //int region,
    //int forest,
    //int district,
    //int fiaCode,
    //int product,
    //double dbh,
    //double totalHeight,
    //double topDiameter,
    //int logNumber) // 1 for the first element, 2 for the second, etc.
    //{
    //    // 1. Pack variables into the shared input records
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

    //    // 2. Fetch the calculation matrix (checks memory first)
    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null && result.TreeOutput.Logs != null)
    //    {
    //        // Convert Excel's 1-based sequence to C#'s 0-based array index
    //        int targetIndex = logNumber - 1;

    //        // Safety check: ensure the requested log number exists and index is valid
    //        if (targetIndex >= 0 && targetIndex < result.TreeOutput.Logs.Count)
    //        {
    //            return result.TreeOutput.Logs[targetIndex].GrossCubicFoot;
    //        }
    //    }

    //    // Return 0.0 if the calculation failed or if the requested log index doesn't exist
    //    return 0.0;
    //}

    [ExcelFunction(Description = "Get Log Small End Diameter Scaled")]
    public static double LogSmallEndDiameterScaled(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.SmallEndDiameterScaled);
    }

    [ExcelFunction(Description = "Get Log Large End Diameter Scaled")]
    public static double LogLargeEndDiameterScaled(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.LargeEndDiameterScaled);
    }

    [ExcelFunction(Description = "Get Log Length")]
    public static double LogLength(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.Length);
    }

    [ExcelFunction(Description = "Get Log Height To Large End Diameter")]
    public static double LogHeightToLargeEndDiameter(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.HeightToLargeEndDiameter);
    }

    [ExcelFunction(Description = "Get Log Gross Board Foot Volume")]
    public static double LogGrossBoardFoot(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.GrossBoardFoot);
    }

    [ExcelFunction(Description = "Get Log Gross Cubic Foot Volume")]
    public static double LogGrossCubicFoot(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.GrossCubicFoot);
    }

    [ExcelFunction(Description = "Get Log International Board Foot Volume")]
    public static double LogInternationalBoardFoot(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.InternationalBoardFoot);
    }

    [ExcelFunction(Description = "Get Log Green Weight")]
    public static double LogGreenWeight(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.GreenWeight);
    }

    [ExcelFunction(Description = "Get Log Dry Weight")]
    public static double LogDryWeight(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, int logNumber)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractLog(options, measurement, logNumber, l => l.DryWeight);
    }
}
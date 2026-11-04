using System;
using ExcelDna.Integration; // Assuming Excel-DNA for your add-in
using FSvolLib.Interop;     // Points to your interop DLL
using System.Collections.Generic;
using System.Text;


namespace FSvolLib.Xl;

public static class BiomassFunctions
{
    //[ExcelFunction(Description = "Calculate Tree Total Above Ground Biomass")]
    //public static double TotalAboveGroundBiomass(
    //    int region,
    //    int forest,
    //    int district,
    //    int fiaCode,
    //    int product,
    //    double dbh,
    //    double totalHeight,
    //    string greenOrDry)
    //{
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

    //    FsvolResult result = VolumeLibrary.CalculateVolume(options, measurement);

    //    if (result.Success && result.TreeOutput != null)
    //    {
    //        // Evaluate string input: fallback to GreenBio if not explicitly "Dry"
    //        if (!string.IsNullOrEmpty(greenOrDry) &&
    //            greenOrDry.StartsWith("D", StringComparison.OrdinalIgnoreCase))
    //        {
    //            return result.TreeOutput.DryBio.AboveGroundTotal;
    //        }

    //        return result.TreeOutput.GreenBio.AboveGroundTotal;
    //    }

    //    return 0.0;
    //}

    [ExcelFunction(Description = "Calculate Tree Total Above Ground Biomass")]
    public static double Bio_AboveGroundTotal(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.AboveGroundTotal, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Branches Biomass")]
    public static double Bio_Branches(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.Branches, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Foliage Biomass")]
    public static double Bio_Foliage(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.Foliage, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Wood Total Biomass")]
    public static double Bio_StemWoodTotal(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemWoodTotal, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Bark Total Biomass")]
    public static double Bio_StemBarkTotal(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemBarkTotal, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stump Wood Biomass")]
    public static double Bio_StumpWood(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StumpWood, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stump Bark Biomass")]
    public static double Bio_StumpBark(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StumpBark, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Primary Wood Biomass")]
    public static double Bio_StemPrimaryWood(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, double topDiameter, string greenOrDry)
    {
        var measurement = new TreeMeasurment { 
            TotalHeight = totalHeight, 
            Dbh = dbh,
            MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0,
            MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
        };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemPrimaryWood, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Primary Bark Biomass")]
    public static double Bio_StemPrimaryBark(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, double topDiameter, string greenOrDry)
    {
        var measurement = new TreeMeasurment { 
            TotalHeight = totalHeight, 
            Dbh = dbh,
            MinTopDibSawOverride = (product == 1) ? topDiameter : -1.0,
            MinTopDibNonSawOverride = (product != 1) ? topDiameter : -1.0
        };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemPrimaryBark, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Secondary Wood Biomass")]
    public static double Bio_StemSecondaryWood(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, double topDiameterSaw, double topDiameterNonsaw, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh, MinTopDibSawOverride = topDiameterSaw, MinTopDibNonSawOverride = topDiameterNonsaw };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemSecondaryWood, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Secondary Bark Biomass")]
    public static double Bio_StemSecondaryBark(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, double topDiameterSaw, double topDiameterNonsaw, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh, MinTopDibSawOverride = topDiameterSaw, MinTopDibNonSawOverride = topDiameterNonsaw };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemSecondaryBark, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Tip Wood Biomass")]
    public static double Bio_StemTipWood(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemTipWood, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Tip Bark Biomass")]
    public static double Bio_StemTipBark(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemTipBark, greenOrDry);
    }

    [ExcelFunction(Description = "Calculate Tree Stem Top And Limb Biomass")]
    public static double Bio_StemTopAndLimb(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, string greenOrDry)
    {
        var measurement = new TreeMeasurment { TotalHeight = totalHeight, Dbh = dbh };
        var options = TreeFunctions.CreateOptions(region, forest, district, fiaCode, product);
        return TreeFunctions.ExecuteAndExtractBiomass(options, measurement, b => b.StemTopAndLimb, greenOrDry);
    }
}

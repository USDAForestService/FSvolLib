using ExcelDna.Integration;
using FSvolLib.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSvolLib.Xl
{
    public static partial class Functions
    {
        [ExcelFunction(Description = "Gets the volume library version")]
        public static string GetVersion()
        {
            return VolumeLibrary.GetVersion();
        }

        [ExcelFunction(Description = "Calculate Tree Volume")]
        [return: ExcelHandle]
        public static FsvolResult? CalculateVolume(int region, int forest, int district, int fiaCode, int product, double dbh, double totalHeight, double minTopDiaSaw)
        {

            var tree = new TreeMeasurment()
            {
                Dbh = dbh,
                TotalHeight = totalHeight,
            };
            if (minTopDiaSaw > 0.0) { tree.MinTopDibSawOverride = minTopDiaSaw; }

            var options = new VolumeCalculationOptions()
            {
                Region = region,
                Forest = forest,
                District = district,
                FiaCode = fiaCode,
                PrimaryProduct = product,
                SecondaryProduct = 2,
            };

            var result = VolumeLibrary.CalculateVolume(options, tree);

            return result;
        }

        [ExcelFunction(Description = "Calculate Tree Volume with extended options")]
        [return: ExcelHandle]
        public static FsvolResult? CalculateVolumeEx(int region, int forest, int district, int fiaCode, int product, 
            double dbh, double drc, double refDia, double topBrokenDia,
            double totalHeight, double refHt, 
            double merchHtSaw, double merchHtNs, 
            double htFirstLiveLimb, double htBrokenTop,
            int formClass, double cull, int decayed, double crownRatio, int stems,
            double minTopDiaSaw, double minTopDiaNs)
        {

            var tree = new TreeMeasurment()
            {
                Dbh = dbh,
                Drc = drc,
                ReferenceDiameter = refDia,
                TopBrokenDiameter = topBrokenDia,
                TotalHeight = totalHeight,
                ReferenceHeight = refHt,
                MerchHeightSaw = merchHtSaw,
                MerchHeightNonsaw = merchHtNs,
                HeightToFirstLiveLimb = htFirstLiveLimb,
                HeightToTopBroken = htBrokenTop,

                FormClass = formClass,
                Cull = cull,
                Decaycd = decayed,
                CrownRatio = crownRatio,
                Stems = stems,
            };
            if (minTopDiaSaw > 0.0) { tree.MinTopDibSawOverride = minTopDiaSaw; }
            if (minTopDiaNs > 0.0) { tree.MinTopDibNonSawOverride = minTopDiaNs; }


            var options = new VolumeCalculationOptions()
            {
                Region = region,
                Forest = forest,
                District = district,
                FiaCode = fiaCode,
                PrimaryProduct = product,
                SecondaryProduct = 2,
            };

            var result = VolumeLibrary.CalculateVolume(options, tree);

            return result;
        }

        [ExcelFunction(Description = "Get volume equation number")]
        public static string? GetVolumeEquationNumber(int region, int forest, int district, int fiaCode, int product)
        { 
            var options = new VolumeCalculationOptions()
            {
                Region = region,
                Forest = forest,
                District = district,
                FiaCode = fiaCode,
                PrimaryProduct = product,
                SecondaryProduct = 2,
            };

            return VolumeLibrary.GetVolumeEquationNumber(options);
        }

    }
}

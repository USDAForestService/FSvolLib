using ExcelDna.Integration;
using FSvolLib.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSvolLib.Xl
{
    public partial class Functions
    {
        [ExcelFunction(Description = "Gets the above-ground total of the biomass data")]
        public static object? Bio_GetAboveGroundTotal([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.AboveGroundTotal);
        }

        [ExcelFunction(Description = "Gets the branches value of the biomass data")]
        public static object? Bio_GetBranches([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.Branches);
        }

        [ExcelFunction(Description = "Gets the foliage value of the biomass data")]
        public static object? Bio_GetFoliage([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.Foliage);
        }

        [ExcelFunction(Description = "Gets the stump wood value of the biomass data")]
        public static object? Bio_GetStumpWood([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StumpWood);
        }

        [ExcelFunction(Description = "Gets the stump bark value of the biomass data")]
        public static object? Bio_GetStumpBark([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StumpBark);
        }

        [ExcelFunction(Description = "Gets the total stem wood value of the biomass data")]
        public static object? Bio_GetStemWoodTotal([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemWoodTotal);
        }

        [ExcelFunction(Description = "Gets the total stem bark value of the biomass data")]
        public static object? Bio_GetStemBarkTotal([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemBarkTotal);
        }

        [ExcelFunction(Description = "Gets the primary stem wood value of the biomass data")]
        public static object? Bio_GetStemPrimaryWood([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemPrimaryWood);
        }

        [ExcelFunction(Description = "Gets the primary stem bark value of the biomass data")]
        public static object? Bio_GetStemPrimaryBark([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemPrimaryBark);
        }

        [ExcelFunction(Description = "Gets the secondary stem wood value of the biomass data")]
        public static object? Bio_GetStemSecondaryWood([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemSecondaryWood);
        }

        [ExcelFunction(Description = "Gets the secondary stem bark value of the biomass data")]
        public static object? Bio_GetStemSecondaryBark([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemSecondaryBark);
        }

        [ExcelFunction(Description = "Gets the stem tip wood value of the biomass data")]
        public static object? Bio_GetStemTipWood([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemTipWood);
        }

        [ExcelFunction(Description = "Gets the stem tip bark value of the biomass data")]
        public static object? Bio_GetStemTipBark([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemTipBark);
        }

        [ExcelFunction(Description = "Gets the stem top and limb value of the biomass data")]
        public static object? Bio_GetStemTopAndLimb([ExcelHandle] BiomassOutput? bio)
        {
            return GetBiomassOutputProperty(bio, x => x.StemTopAndLimb);
        }


        static object? GetBiomassOutputProperty([ExcelHandle] BiomassOutput? biomass, Func<BiomassOutput, object> propertySelector)
        {
            if (biomass is null)
            { return ExcelDna.Integration.ExcelError.ExcelErrorNull; }

            return propertySelector(biomass);
        }
    }
}

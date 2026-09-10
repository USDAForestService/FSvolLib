using ExcelDna.Integration;
using FSvolLib.Interop;

namespace FSvolLib.Xl;

public partial class Functions
{
    [ExcelFunction(Description = "Gets the gross board foot primary volume of a tree")]
    public static object? Tree_GetGrossBoardFootPrimary([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.GrossBoardFootPrimary);
    }

    [ExcelFunction(Description = "Gets the gross board foot secondary volume of a tree")]
    public static object? Tree_GetGrossBoardFootSecondary([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.GrossBoardFootSecondary);
    }

    [ExcelFunction(Description = "Gets the gross cubic foot primary volume of a tree")]
    public static object? Tree_GetGrossCubicFootPrimary([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.GrossCubicFootPrimary);
    }

    [ExcelFunction(Description = "Gets the gross cubic foot secondary volume of a tree")]
    public static object? Tree_GetGrossCubicFootSecondary([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.GrossCubicFootSecondary);
    }

    [ExcelFunction(Description = "Gets the gross international board foot volume of a tree")]
    public static object? Tree_GetGrossInternationalBoardFoot([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.GrossInternationalBoardFoot);
    }

    [ExcelFunction(Description = "Gets the total cubic foot volume of a tree")]
    public static object? Tree_GetTotalCubicFoot([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.TotalCubicFoot);
    }

    [ExcelFunction(Description = "Gets the stump cubic foot volume of a tree")]
    public static object? Tree_GetStumpCubicFoot([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.StumpCubicFoot);
    }

    [ExcelFunction(Description = "Gets the tip cubic foot volume of a tree")]
    public static object? Tree_GetTipCubicFoot([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.TipCubicFoot);
    }

    [ExcelFunction(Description = "Gets the cord merchantable volume of a tree")]
    public static object? Tree_GetCordMerchantable([ExcelHandle] FsvolResult? result)
    {
        return GetTreeOutputProperty(result, x => x.CordMerchantable);
    }

    [ExcelFunction(Description = "Gets the green biomass of tree ")]
    [return: ExcelHandle]
    public static BiomassOutput? GetGreenBiomass([ExcelHandle] FsvolResult? result)
    {
        if(result is null || result.Success is false) { return null; }

        return result.TreeOutput!.GreenBio;
    }

    [ExcelFunction(Description = "Gets the dry biomass of tree ")]
    [return: ExcelHandle]
    public static BiomassOutput? GetDryBiomass([ExcelHandle] FsvolResult? result)
    {
        if (result is null || result.Success is false) { return null; }

        return result.TreeOutput!.DryBio;
    }

    static object? GetTreeOutputProperty([ExcelHandle] FsvolResult? result, Func<TreeOutput, object> propertySelector)
    {
        if (result is null)
        { return ExcelDna.Integration.ExcelError.ExcelErrorNull; }
        if (result.Success is false)
        { return result.ErrorMessage; }


        var tree = result.TreeOutput!;
        return propertySelector(tree);
    }
}

using Xunit;

namespace FSvolLib.Interop.Test;

public class VolumeLibrary_Test
{
    [Fact]
    public void Version_Should_Returns_Something()
    {
        var versionStr = VolumeLibrary.GetVersion();

        Assert.False(string.IsNullOrWhiteSpace(versionStr));

        Assert.True(Version.TryParse(versionStr, out var version), $"{versionStr} should be formated like major.minor[.build[.revision]]");
    }

    [Fact]
    public void CalculateVolume_Should_Return_Success_For_Valid_Input()
    {
        var options = new VolumeCalculationOptions
        {
            FiaCode = 100,
            Region = 2,
            Forest = 1,
            District = 0,
            PrimaryProduct = 1,
            SecondaryProduct = 2,
            //VolumeEquationNumberOverride = "R03CHO0066",
            //EcoRegion = "M260"
        };

        var tree = new TreeMeasurment
        {
            Dbh = 19.7,
            TotalHeight = 76.0
        };

        var result = VolumeLibrary.CalculateVolume(options, tree);

        Assert.True(result.Success, "The native interop call unexpectedly failed.");
        Assert.NotNull(result.TreeOutput);
        Assert.True(result.TreeOutput!.NumberOfLogs >= 0);
        Assert.True(result.TreeOutput.TotalCubicFoot >= 0.0 || result.TreeOutput.TotalCubicFoot == 0.0);
    }

    [Fact]
    public void CalculateVolume_With_ErrorMessage()
    {
        var options = new VolumeCalculationOptions
        {
            FiaCode = 100,
            Region = 2,
            Forest = 1,
            District = 0,
            PrimaryProduct = 1,
            SecondaryProduct = 2,
        };

        var tree = new TreeMeasurment
        {
            Dbh = 0,
            TotalHeight = 0
        };

        var result = VolumeLibrary.CalculateVolume(options, tree);

        Assert.False(result.Success, "The native interop call return non-success");
        Assert.Null(result.TreeOutput);
        Assert.NotNull(result.ErrorMessage);

    }

}

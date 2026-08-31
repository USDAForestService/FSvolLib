using FSvolLib.Interop;

using Xunit;

public class InteropSmokeTests
{
    [Fact]
    public void Version_Should_Return_Expected_String()
    {
        var version = FsvolLibrary.GetVersion();

        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    [Fact]
    public void TryCalculateVolume_Should_Return_Success_For_Valid_Input()
    {
        var options = FsvolLibrary.CreateVolumeCalculationOptions(
            fiaCode: 100,
            region: 2,
            forest: 1,
            district: 0,
            primaryProduct: 1,
            secondaryProduct: 2,
            volumeEquationNumberOverride: "R03CHO0066",
            ecoRegion: "M260");

        var tree = FsvolLibrary.CreateTreeMeasurement(dbh: 19.7, totalHeight: 76.0);
        tree.referenceHeight = 0.0;
        tree.referenceDiameter = 0.0;
        tree.minTopDibNonSawOverride = 0.0;
        tree.minTopDibSawOverride = 0.0;
        tree.formClass = 80;
        tree.heightToTopBroken = 0.0;
        tree.topBrokenDiameter = 0.0;

        var result = FsvolLibrary.TryCalculateVolume(options, tree);

        Assert.True(result.Success, "The native interop call unexpectedly failed.");
        Assert.NotNull(result.TreeOutput);
        Assert.True(result.TreeOutput!.numberOfLogs >= 0);
        Assert.True(result.TreeOutput.totalCubicFoot >= 0.0 || result.TreeOutput.totalCubicFoot == 0.0);
    }
}

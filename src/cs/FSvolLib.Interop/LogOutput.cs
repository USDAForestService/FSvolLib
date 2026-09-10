using FSvolLib.Interop.Native.CModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSvolLib.Interop;

public class LogOutput
{
    public int LogNumber;
    public int Product;
    public bool IsSecondary;
    public double SmallEndDiameterActual;
    public double LargeEndDiameterActual;
    public double SmallEndDiameterScaled;
    public double LargeEndDiameterScaled;
    public double Length;
    public double HeightToLargeEndDiameter;
    public double GrossBoardFoot;
    public double GrossCubicFoot;
    public double InternationalBoardFoot;
    public double GreenWeight;
    public double DryWeight;

    public LogOutput()
    {
    }

    public LogOutput(LogOutput_C logOutput)
    {
        LogNumber = logOutput.logNumber;
        Product = logOutput.product;
        IsSecondary = logOutput.isSecondary;
        SmallEndDiameterActual = logOutput.smallEndDiameterActual;
        LargeEndDiameterActual = logOutput.largeEndDiameterActual;
        SmallEndDiameterScaled = logOutput.smallEndDiameterScaled;
        LargeEndDiameterScaled = logOutput.largeEndDiameterScaled;
        Length = logOutput.length;
        HeightToLargeEndDiameter = logOutput.heightToLargeEndDiameter;
        GrossBoardFoot = logOutput.grossBoardFoot;
        GrossCubicFoot = logOutput.grossCubicFoot;
        InternationalBoardFoot = logOutput.internationalBoardFoot;
        GreenWeight = logOutput.greenWeight;
        DryWeight = logOutput.dryWeight;
    }
}

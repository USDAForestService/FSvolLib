using FSvolLib.Interop.Native.CModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FSvolLib.Interop;

public class BiomassOutput
{
    public double AboveGroundTotal;
    public double Branches;
    public double Foliage;
    public double StumpWood;
    public double StumpBark;
    public double StemWoodTotal;
    public double StemBarkTotal;
    public double StemPrimaryWood;
    public double StemPrimaryBark;
    public double StemSecondaryWood;
    public double StemSecondaryBark;
    public double StemTipWood;
    public double StemTipBark;
    public double StemTopAndLimb;

    public BiomassOutput()
    {
    }

    public BiomassOutput(BiomassOutput_C biomassOutput)
    {
        AboveGroundTotal = biomassOutput.aboveGroundTotal;
        Branches = biomassOutput.branches;
        Foliage = biomassOutput.foliage;
        StumpWood = biomassOutput.stumpWood;
        StumpBark = biomassOutput.stumpBark;
        StemWoodTotal = biomassOutput.stemWoodTotal;
        StemBarkTotal = biomassOutput.stemBarkTotal;
        StemPrimaryWood = biomassOutput.stemPrimaryWood;
        StemPrimaryBark = biomassOutput.stemPrimaryBark;
        StemSecondaryWood = biomassOutput.stemSecondaryWood;
        StemSecondaryBark = biomassOutput.stemSecondaryBark;
        StemTipWood = biomassOutput.stemTipWood;
        StemTipBark = biomassOutput.stemTipBark;
        StemTopAndLimb = biomassOutput.stemTopAndLimb;
    }
}

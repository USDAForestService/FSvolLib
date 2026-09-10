using FSvolLib.Interop.Native.CModels;

namespace FSvolLib.Interop
{
    public class TreeOutput
    {
        public TreeOutput()
        {
        }

        public TreeOutput(TreeOutput_C treeOutput)
        {
            GrossBoardFootPrimary = treeOutput.grossBoardFootPrimary;
            GrossBoardFootSecondary = treeOutput.grossBoardFootSecondary;
            GrossCubicFootPrimary = treeOutput.grossCubicFootPrimary;
            GrossCubicFootSecondary = treeOutput.grossCubicFootSecondary;
            GrossInternationalBoardFoot = treeOutput.grossInternationalBoardFoot;
            TotalCubicFoot = treeOutput.totalCubicFoot;
            StumpCubicFoot = treeOutput.stumpCubicFoot;
            TipCubicFoot = treeOutput.tipCubicFoot;
            CordMerchantable = treeOutput.cordMerchantable;
            NumberOfLogs = treeOutput.numberOfLogs;
            Errflag = treeOutput.errflag;
            GreenBio = new BiomassOutput(treeOutput.greenBio);
            DryBio = new BiomassOutput(treeOutput.dryBio);
            Logs = TreeOutput_C.ReadLogs(treeOutput.logs, treeOutput.logCount)
                .Select(x => new LogOutput(x)).ToArray();
        }

        public double GrossBoardFootPrimary { get; set; }
        public double GrossBoardFootSecondary { get; set; }
        public double GrossCubicFootPrimary { get; set; }
        public double GrossCubicFootSecondary { get; set; }
        public double GrossInternationalBoardFoot { get; set; }
        public double TotalCubicFoot { get; set; }
        public double StumpCubicFoot { get; set; }
        public double TipCubicFoot { get; set; }
        public double CordMerchantable { get; set; }
        public int NumberOfLogs { get; set; }
        public int Errflag { get; set; }

        public BiomassOutput GreenBio { get; set; } = new BiomassOutput();
        public BiomassOutput DryBio { get; set; } = new BiomassOutput();

        public IReadOnlyList<LogOutput> Logs { get; set; } = Array.Empty<LogOutput>();
    }
}

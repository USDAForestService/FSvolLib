#include "VolumeLibrary.interop.h"

#include "VolumeLibrary.h"

#include <cstring>
#include <stdexcept>
#include <string>

namespace
{
static std::string safe_string(const char* value)
{
    return value ? std::string(value) : std::string();
}

static void set_error(ErrorInfo_C** error, const std::string& message)
{
    if (error == nullptr)
    {
        return;
    }

    *error = new ErrorInfo_C();
    (*error)->errorCode = 1;
    (*error)->errorMessage = new char[message.size() + 1];
    std::strncpy((*error)->errorMessage, message.c_str(), message.size() + 1);
}

static VolumeCalculationOptions convert_options(const VolumeCalculationOptions_C& in)
{
    VolumeCalculationOptions out{};
    out.fiaCode = in.fiaCode;
    out.auxFlag = static_cast<VolumeCalculationOptions::AuxFlag>(in.auxFlag);
    out.region = in.region;
    out.forest = in.forest;
    out.district = in.district;
    out.primaryProduct = in.primaryProduct;
    out.secondaryProduct = in.secondaryProduct;
    out.volumeCalculationOptions = static_cast<VolumeCalculationOptions::VolumeCalculationType>(in.volumeCalculationOptions);
    out.volumeEquationNumberOverride = safe_string(in.volumeEquationNumberOverride);
    out.ecoRegion = safe_string(in.ecoRegion);
    out.basalArea = in.basalArea;
    out.siteIndex = in.siteIndex;
    return out;
}

static TreeMeasurment convert_tree(const TreeMeasurment_C& in)
{
    TreeMeasurment out{};
    out.totalHeight = in.totalHeight;
    out.referenceHeight = in.referenceHeight;
    out.merchHeightSaw = in.merchHeightSaw;
    out.merchHeightNonsaw = in.merchHeightNonsaw;
    out.merchHeightUnit = static_cast<TreeMeasurment::MerchHeightUnit>(in.merchHeightUnit);
    out.heightToFirstLiveLimb = in.heightToFirstLiveLimb;
    out.heightToTopBroken = in.heightToTopBroken;
    out.isLive = in.isLive;
    out.dbh = in.dbh;
    out.drc = in.drc;
    out.referenceDiameter = in.referenceDiameter;
    out.topBrokenDiameter = in.topBrokenDiameter;
    out.formClass = in.formClass;
    out.stumpHeightOverride = in.stumpHeightOverride;
    out.minTopDibSawOverride = in.minTopDibSawOverride;
    out.minTopDibNonSawOverride = in.minTopDibNonSawOverride;
    out.cull = in.cull;
    out.decaycd = in.decaycd;
    out.crownRatio = in.crownRatio;
    out.stems = in.stems;
    return out;
}

static MerchRules convert_merch_rules(const MerchRules_C& in)
{
    MerchRules out{};
    out.evenOdd = in.evenOdd;
    out.segmentationOption = in.segmentationOption;
    out.maxLogLength = in.maxLogLength;
    out.minLogLength = in.minLogLength;
    out.minLengthTop = in.minLengthTop;
    out.minTopDibSaw = in.minTopDibSaw;
    out.minTopDibNonSaw = in.minTopDibNonSaw;
    out.minMerchLength = in.minMerchLength;
    out.stumpHeight = in.stumpHeight;
    out.trim = in.trim;
    out.barkThicknessRatio = in.barkThicknessRatio;
    out.doubleBarkThicknessAtBrestHeight = in.doubleBarkThicknessAtBrestHeight;
    out.minimumBoardFootDiameter = in.minimumBoardFootDiameter;
    out.useCorrectedFactor = in.useCorrectedFactor;
    return out;
}

static BiomassOutput_C convert_biomass_output(const BiomassOutput& in)
{
    BiomassOutput_C out{};
    out.aboveGroundTotal = in.aboveGroundTotal;
    out.branches = in.branches;
    out.foliage = in.foliage;
    out.stumpWood = in.stumpWood;
    out.stumpBark = in.stumpBark;
    out.stemWoodTotal = in.stemWoodTotal;
    out.stemBarkTotal = in.stemBarkTotal;
    out.stemPrimaryWood = in.stemPrimaryWood;
    out.stemPrimaryBark = in.stemPrimaryBark;
    out.stemSecondaryWood = in.stemSecondaryWood;
    out.stemSecondaryBark = in.stemSecondaryBark;
    out.stemTipWood = in.stemTipWood;
    out.stemTipBark = in.stemTipBark;
    out.stemTopAndLimb = in.stemTopAndLimb;
    return out;
}

static LogOutput_C convert_log_output(const LogOutput& in)
{
    LogOutput_C out{};
    out.logNumber = in.logNumber;
    out.product = in.product;
    out.isSecondary = in.isSecondary ? 1 : 0;
    out.smallEndDiameterActual = in.smallEndDiameterActual;
    out.largeEndDiameterActual = in.largeEndDiameterActual;
    out.smallEndDiameterScaled = in.smallEndDiameterScaled;
    out.largeEndDiameterScaled = in.largeEndDiameterScaled;
    out.length = in.length;
    out.heightToLargeEndDiameter = in.heightToLargeEndDiameter;
    out.grossBoardFoot = in.grossBoardFoot;
    out.grossCubicFoot = in.grossCubicFoot;
    out.internationalBoardFoot = in.internationalBoardFoot;
    out.greenWeight = in.greenWeight;
    out.dryWeight = in.dryWeight;
    return out;
}

static TreeOutput_C* convert_tree_output(const TreeOutput& in)
{
    TreeOutput_C* out = new TreeOutput_C();
    out->grossBoardFootPrimary = in.grossBoardFootPrimary;
    out->grossBoardFootSecondary = in.grossBoardFootSecondary;
    out->grossCubicFootPrimary = in.grossCubicFootPrimary;
    out->grossCubicFootSecondary = in.grossCubicFootSecondary;
    out->grossInternationalBoardFoot = in.grossInternationalBoardFoot;
    out->totalCubicFoot = in.totalCubicFoot;
    out->stumpCubicFoot = in.stumpCubicFoot;
    out->tipCubicFoot = in.tipCubicFoot;
    out->cordMerchantable = in.cordMerchantable;
    out->greenWeightPrimary = in.greenWeightPrimary;
    out->greenWeightSecondary = in.greenWeightSecondary;
    out->dryWeightPrimary = in.dryWeightPrimary;
    out->dryWeightSecondary = in.dryWeightSecondary;
    out->greenBio = convert_biomass_output(in.greenBio);
    out->dryBio = convert_biomass_output(in.dryBio);
    out->carbonContent = in.carbonContent;
    out->numberOfLogs = in.numberOfLogs;
    out->errflag = in.errflag;

    const size_t log_count = in.logs.size();
    out->logs = nullptr;
    out->log_count = log_count;

    if (log_count > 0)
    {
        out->logs = new LogOutput_C[log_count];
        for (size_t i = 0; i < log_count; ++i)
        {
            out->logs[i] = convert_log_output(in.logs[i]);
        }
    }

    return out;
}

static bool ensure_valid_input(const VolumeCalculationOptions_C* options,
                              const TreeMeasurment_C* tree,
                              TreeOutput_C** out,
                              ErrorInfo_C** error)
{
    // TODO add individual check for each field and indicate null field in error message
    if (options == nullptr || tree == nullptr || out == nullptr)
    {
        if (error != nullptr)
        {
            set_error(error, "Null input passed to FSvolLib interop API.");
        }
        return false;
    }

    *out = nullptr;
    if (error != nullptr)
    {
        *error = nullptr;
    }

    return true;
}
} // namespace

extern "C" bool FSVOLLIB_INTEROP_API CalculateVolume(const VolumeCalculationOptions_C* options,
                                                      const TreeMeasurment_C* tree,
                                                      TreeOutput_C** out,
                                                      ErrorInfo_C** error)
{
    if (!ensure_valid_input(options, tree, out, error))
    {
        return false;
    }

    try
    {
        const auto nativeOptions = convert_options(*options);
        const auto nativeTree = convert_tree(*tree);
        const auto result = VolumeLibrary::getInstance().CalculateVolume(nativeOptions, nativeTree);
        *out = convert_tree_output(result);
        return true;
    }
    catch (const std::exception& ex)
    {
        if (error != nullptr)
        {
            set_error(error, ex.what());
        }
        *out = nullptr;
        return false;
    }
}

extern "C" bool FSVOLLIB_INTEROP_API CalculateVolumeWithMerchRules(const VolumeCalculationOptions_C* options,
                                                                  const TreeMeasurment_C* tree,
                                                                  const MerchRules_C* merchRules,
                                                                  TreeOutput_C** out,
                                                                  ErrorInfo_C** error)
{
    if (!ensure_valid_input(options, tree, out, error))
    {
        return false;
    }

    if (merchRules == nullptr)
    {
        if (error != nullptr)
        {
            set_error(error, "MerchRules input was null.");
        }
        return false;
    }

    try
    {
        const auto nativeOptions = convert_options(*options);
        const auto nativeTree = convert_tree(*tree);
        const auto nativeMerchRules = convert_merch_rules(*merchRules);
        const auto result = VolumeLibrary::getInstance().CalculateVolume(nativeOptions, nativeTree, nativeMerchRules);
        *out = convert_tree_output(result);
        return true;
    }
    catch (const std::exception& ex)
    {
        if (error != nullptr)
        {
            set_error(error, ex.what());
        }
        *out = nullptr;
        return false;
    }
}

extern "C" void FSVOLLIB_INTEROP_API GetVolumeEquationNumber(const VolumeCalculationOptions_C* options,
                                                            char* outBuffer,
                                                            int bufferLen)
{
    if (options == nullptr || outBuffer == nullptr || bufferLen <= 0)
    {
        return;
    }

    const auto nativeOptions = convert_options(*options);
    const std::string volumeEquationNumber = VolumeLibrary::getInstance().GetVolumeEquationNumber(nativeOptions);
    std::strncpy(outBuffer, volumeEquationNumber.c_str(), static_cast<size_t>(bufferLen));
    outBuffer[bufferLen - 1] = '\0';
}

extern "C" double FSVOLLIB_INTEROP_API GetHeightAtDiameter(const char* volEqNumber,
                                                          const TreeMeasurment_C* tree,
                                                          double diameter)
{
    if (tree == nullptr)
    {
        return 0.0;
    }

    return VolumeLibrary::getInstance().GetHeightAtDiameter(
        safe_string(volEqNumber),
        convert_tree(*tree),
        diameter);
}

extern "C" double FSVOLLIB_INTEROP_API GetDiameterAtHeight(const char* volEqNumber,
                                                          const TreeMeasurment_C* tree,
                                                          double height)
{
    if (tree == nullptr)
    {
        return 0.0;
    }

    return VolumeLibrary::getInstance().GetDiameterAtHeight(
        safe_string(volEqNumber),
        convert_tree(*tree),
        height);
}

extern "C" int FSVOLLIB_INTEROP_API GetNumberOfLogs(const VolumeCalculationOptions_C* options,
                                                    const TreeMeasurment_C* tree)
{
    if (options == nullptr || tree == nullptr)
    {
        return 0;
    }

    return VolumeLibrary::getInstance().GetNumberOfLogs(convert_options(*options), convert_tree(*tree));
}

extern "C" void FSVOLLIB_INTEROP_API GetVersion(char* outBuffer, int bufferLen)
{
    if (outBuffer == nullptr || bufferLen <= 0)
    {
        return;
    }

    const std::string version = VolumeLibrary::getInstance().GetVersion();
    std::strncpy(outBuffer, version.c_str(), static_cast<size_t>(bufferLen));
    outBuffer[bufferLen - 1] = '\0';
}

extern "C" void FSVOLLIB_INTEROP_API free_tree_output_c(TreeOutput_C* value)
{
    if (value == nullptr)
    {
        return;
    }

    delete[] value->logs;
    delete value;
}

extern "C" void FSVOLLIB_INTEROP_API free_error_info_c(ErrorInfo_C* value)
{
    if (value == nullptr)
    {
        return;
    }

    delete[] value->errorMessage;
    delete value;
}

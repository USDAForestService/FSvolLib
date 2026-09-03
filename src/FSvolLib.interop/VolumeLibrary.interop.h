#pragma once

#include "export.h"

#include <stdbool.h>
#include <stddef.h>

#ifdef __cplusplus
extern "C" {
#endif

typedef enum VolumeCalculationOptions_C_VolumeCalculationType
{
    VOLUME_CALCULATION_TYPE_FVS = 0,
    VOLUME_CALCULATION_TYPE_FIA,
    VOLUME_CALCULATION_TYPE_CRUISE,
    VOLUME_CALCULATION_TYPE_VARIABLE_LOG_LENGTH
} VolumeCalculationOptions_C_VolumeCalculationType;

typedef enum VolumeCalculationOptions_C_AuxFlag
{
    VOLUME_CALCULATION_OPTIONS_AUX_FLAG_NONE = 0,
    VOLUME_CALCULATION_OPTIONS_AUX_FLAG_R10_YOUNG_GROWTH = 'Y',
    VOLUME_CALCULATION_OPTIONS_AUX_FLAG_R6_DOUGFIR = 'F',
    VOLUME_CALCULATION_OPTIONS_AUX_FLAG_PLANTATION = 'P'
} VolumeCalculationOptions_C_AuxFlag;

typedef enum TreeMeasurment_C_MerchHeightUnit
{
    TREE_MEASURMENT_MERCH_HEIGHT_UNIT_FEET = 0,
    TREE_MEASURMENT_MERCH_HEIGHT_UNIT_LOGS8 = 8,
    TREE_MEASURMENT_MERCH_HEIGHT_UNIT_LOGS16 = 16,
    TREE_MEASURMENT_MERCH_HEIGHT_UNIT_LOGS32 = 32
} TreeMeasurment_C_MerchHeightUnit;

typedef struct BiomassOutput_C
{
    double aboveGroundTotal;
    double branches;
    double foliage;
    double stumpWood;
    double stumpBark;
    double stemWoodTotal;
    double stemBarkTotal;
    double stemPrimaryWood;
    double stemPrimaryBark;
    double stemSecondaryWood;
    double stemSecondaryBark;
    double stemTipWood;
    double stemTipBark;
    double stemTopAndLimb;
} BiomassOutput_C;

typedef struct LogOutput_C
{
    int logNumber;
    int product;
    int isSecondary;

    double smallEndDiameterActual;
    double largeEndDiameterActual;
    double smallEndDiameterScaled;
    double largeEndDiameterScaled;

    double length;
    double heightToLargeEndDiameter;

    double grossBoardFoot;
    double grossCubicFoot;
    double internationalBoardFoot;

    double greenWeight;
    double dryWeight;
} LogOutput_C;

typedef struct TreeOutput_C
{
    double grossBoardFootPrimary;
    double grossBoardFootSecondary;
    double grossCubicFootPrimary;
    double grossCubicFootSecondary;
    double grossInternationalBoardFoot;

    double totalCubicFoot;
    double stumpCubicFoot;
    double tipCubicFoot;

    double cordMerchantable;

    double greenWeightPrimary;
    double greenWeightSecondary;
    double dryWeightPrimary;
    double dryWeightSecondary;

    BiomassOutput_C greenBio;
    BiomassOutput_C dryBio;

    double carbonContent;

    LogOutput_C* logs;
    size_t log_count;

    int numberOfLogs;
    int errflag;
} TreeOutput_C;

typedef struct TreeMeasurment_C
{
    double totalHeight;
    double referenceHeight;
    double merchHeightSaw;
    double merchHeightNonsaw;
    TreeMeasurment_C_MerchHeightUnit merchHeightUnit;
    double heightToFirstLiveLimb;
    double heightToTopBroken;
    bool isLive;

    double dbh;
    double drc;
    double referenceDiameter;
    double topBrokenDiameter;
    int formClass;

    double stumpHeightOverride;
    double minTopDibSawOverride;
    double minTopDibNonSawOverride;

    double cull;
    int decaycd;
    double crownRatio;
    int stems;
} TreeMeasurment_C;

typedef struct MerchRules_C
{
    int evenOdd;
    int segmentationOption;
    double maxLogLength;
    double minLogLength;
    double minLengthTop;
    double minTopDibSaw;
    double minTopDibNonSaw;
    double minMerchLength;

    double stumpHeight;
    double trim;
    double barkThicknessRatio;
    double doubleBarkThicknessAtBrestHeight;
    double minimumBoardFootDiameter;
    bool useCorrectedFactor;
} MerchRules_C;

typedef struct VolumeCalculationOptions_C
{
    int fiaCode;
    VolumeCalculationOptions_C_AuxFlag auxFlag;
    int region;
    int forest;
    int district;
    int primaryProduct;
    int secondaryProduct;
    VolumeCalculationOptions_C_VolumeCalculationType volumeCalculationOptions;
    char* volumeEquationNumberOverride;
    char* ecoRegion;
    int basalArea;
    int siteIndex;
} VolumeCalculationOptions_C;

typedef struct ErrorInfo_C
{
    int errorCode;
    char* errorMessage;
} ErrorInfo_C;

FSVOLLIB_INTEROP_API bool CalculateVolume(const VolumeCalculationOptions_C* options,
                                            const TreeMeasurment_C* tree,
                                            TreeOutput_C** out,
                                            ErrorInfo_C** error);

FSVOLLIB_INTEROP_API bool CalculateVolumeWithMerchRules(const VolumeCalculationOptions_C* options,
                                                      const TreeMeasurment_C* tree,
                                                      const MerchRules_C* merchRules,
                                                      TreeOutput_C** out,
                                                      ErrorInfo_C** error);

FSVOLLIB_INTEROP_API void GetVolumeEquationNumber(const VolumeCalculationOptions_C* options,
                                                char* outBuffer,
                                                int bufferLen);

FSVOLLIB_INTEROP_API double GetHeightAtDiameter(const char* volEqNumber,
                                              const TreeMeasurment_C* tree,
                                              double diameter);

FSVOLLIB_INTEROP_API double GetDiameterAtHeight(const char* volEqNumber,
                                              const TreeMeasurment_C* tree,
                                              double height);

FSVOLLIB_INTEROP_API int GetNumberOfLogs(const VolumeCalculationOptions_C* options,
                                        const TreeMeasurment_C* tree);

FSVOLLIB_INTEROP_API void GetVersion(char* outBuffer, int bufferLen);

FSVOLLIB_INTEROP_API void free_tree_output_c(TreeOutput_C* value);
FSVOLLIB_INTEROP_API void free_error_info_c(ErrorInfo_C* value);

#ifdef __cplusplus
}
#endif

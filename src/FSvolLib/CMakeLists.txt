
cmake_minimum_required(VERSION 3.21)
project(FSvolLib LANGUAGES CXX)

# Gather all headers and sources
file(GLOB_RECURSE FSvolLib_HEADERS
    "array_helper.h"
    "DefaultFormClassForFVS.h"
    "MerchRulesResolver.h"
    "Models/*.h"
    "TaperModels/*.h"
    "VolumeCalculators/*.h"
    "VolumeEquation.h"
    "VolumeEquationResolver.h"
    "VolumeLibrary.h"
    "WeightFactorAndRefData.h"
    "WeightFactorAndRefDataResolver.h"
    "WoodlandBiomass.h"
)

file(GLOB_RECURSE FSvolLib_SOURCES
    "MerchRulesResolver.cpp"
    "SmalianScribnerIntl14.cpp"
    "TaperModels/*.cpp"
    "VolumeCalculators/*.cpp"
    "VolumeEquationResolver.cpp"
    "VolumeEquationResolver.*.cpp"
    "VolumeLibrary.cpp"
    "WeightFactorAndRefDataResolver.cpp"
    "WoodlandBiomass.cpp"
)

# Library type may vary by configuration; STATIC for Debug|x64 in original
# Here we default to SHARED; platform presets will override
add_library(FSvolLib SHARED ${FSvolLib_HEADERS} ${FSvolLib_SOURCES})

# Language standard
set_target_properties(FSvolLib PROPERTIES CXX_STANDARD 17 CXX_STANDARD_REQUIRED YES)

# Definitions
if(CMAKE_BUILD_TYPE STREQUAL "Debug")
    target_compile_definitions(FSvolLib PRIVATE _DEBUG FSVOLLIB_EXPORTS)
else()
    target_compile_definitions(FSvolLib PRIVATE NDEBUG FSVOLLIB_EXPORTS)
endif()

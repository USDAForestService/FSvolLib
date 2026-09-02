
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

add_library(FSvolLib STATIC ${FSvolLib_HEADERS} ${FSvolLib_SOURCES})

# Expose headers so dependent targets (e.g. tests) get include paths automatically
target_include_directories(FSvolLib PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})

# Language standard
set_target_properties(FSvolLib PROPERTIES CXX_STANDARD 17 CXX_STANDARD_REQUIRED YES)

# Definitions
target_compile_definitions(FSvolLib PRIVATE
    $<$<CONFIG:Debug>:_DEBUG>
    $<$<NOT:$<CONFIG:Debug>>:NDEBUG>
)

# Tests are only built for host (not cross-compiled to WASM/Android)
include(CTest)
if(BUILD_TESTING AND NOT CMAKE_CROSSCOMPILING)
    add_subdirectory(../FSvolLib.test ${CMAKE_CURRENT_BINARY_DIR}/FSvolLib.test)
endif()

# ---------------------------------------------------------------------------
# Interop DLL target (Windows P/Invoke surface for .NET consumers)
# Enable with: cmake --preset win-... -DBUILD_INTEROP=ON
# Not available for cross-compiled targets (WASM, Android).
# ---------------------------------------------------------------------------
option(BUILD_INTEROP "Build FSvolLibInterop shared library for .NET P/Invoke" OFF)
if(BUILD_INTEROP)
    if(WIN32 OR ANDROID)
        add_subdirectory(../FSvolLib.interop ${CMAKE_CURRENT_BINARY_DIR}/FSvolLib.interop)
    else()
        message(WARNING "BUILD_INTEROP is only supported on Windows and Android")
    endif()
endif()

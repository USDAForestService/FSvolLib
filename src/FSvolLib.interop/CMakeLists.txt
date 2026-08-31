cmake_minimum_required(VERSION 3.21)
project(FSvolLibInterop LANGUAGES CXX)

add_library(FSvolLibInterop SHARED
    VolumeLibrary.interop.cpp
)

target_include_directories(FSvolLibInterop
    PUBLIC
        ${CMAKE_CURRENT_SOURCE_DIR}
        ${CMAKE_CURRENT_SOURCE_DIR}/../FSvolLib
)

target_link_libraries(FSvolLibInterop PRIVATE FSvolLib)

set_target_properties(FSvolLibInterop PROPERTIES
    CXX_STANDARD 17
    CXX_STANDARD_REQUIRED YES
)

if(CMAKE_BUILD_TYPE STREQUAL "Debug")
    target_compile_definitions(FSvolLibInterop PRIVATE _DEBUG)
else()
    target_compile_definitions(FSvolLibInterop PRIVATE NDEBUG)
endif()

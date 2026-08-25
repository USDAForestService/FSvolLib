include(FetchContent)
FetchContent_Declare(
    googletest
    GIT_REPOSITORY https://github.com/google/googletest.git
    GIT_TAG        v1.15.2
)
# Prevent GTest from overriding compiler/linker settings on Windows
set(gtest_force_shared_crt ON CACHE BOOL "" FORCE)
FetchContent_MakeAvailable(googletest)

set(FSvolLibTest_SOURCES
    CzaplewskiTaperModel.test.cpp
    DeMarsTaperModel.test.cpp
    FSvolLib.test.cpp
    NSVB.TaperModel.test.cpp
    NSVB.test.cpp
    VolumeEquation.test.cpp
    VolumeLibrary.test.cpp
    WenselOlsonTaperModel.test.cpp
)

add_executable(FSvolLibTests ${FSvolLibTest_SOURCES})

target_link_libraries(FSvolLibTests PRIVATE FSvolLib GTest::gtest_main)

set_target_properties(FSvolLibTests PROPERTIES CXX_STANDARD 17 CXX_STANDARD_REQUIRED YES)

include(GoogleTest)
gtest_discover_tests(FSvolLibTests)

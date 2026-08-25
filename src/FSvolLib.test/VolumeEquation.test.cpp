#include <gtest/gtest.h>

#include "TaperModels/WenselOlsonTaperModel.h"
#include "VolumeEquation.h"

#include <string>


TEST(VolumeEquationTest, ParseVolumeEquationNumber)
{
    // Arrange
    std::string volumeEquationNumber = "500WO2W122";

    // Act
    VolumeEquation volEq = VolumeEquation::ParseVolumeEquationNumber(volumeEquationNumber);

    // Assert
    EXPECT_TRUE(volEq.geoCode == VolumeEquation::GeoCode::R5);
    EXPECT_TRUE(volEq.modelType == VolumeEquation::ModelType::WO2);
    EXPECT_TRUE(volEq.usRegion == 'W');
    EXPECT_TRUE(volEq.fiaCode == 122);
}

TEST(VolumeEquationTest, GetVolumeEquationNumber)
{
    // Arrange
    std::string expectedVolumeEquationNumber = "500WO2W122";

    VolumeEquation volEq;
    volEq.geoCode = VolumeEquation::GeoCode::R5;
    volEq.modelType = VolumeEquation::ModelType::WO2;
    volEq.usRegion = 'W';
    volEq.fiaCode = 122;

    // Act
    auto volumeEquationNumber = volEq.GetVolumeEquationNumber();

    // Assert
    EXPECT_TRUE(volumeEquationNumber == expectedVolumeEquationNumber);
}
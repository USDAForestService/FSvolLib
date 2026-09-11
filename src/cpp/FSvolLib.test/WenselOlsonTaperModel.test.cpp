#include <gtest/gtest.h>

#include "TaperModels/WenselOlsonTaperModel.h"
#include "VolumeEquation.h"

#include <string>


TEST(WenselOlsonTaperModelTest, GetDiameterAtHeightTest_With_FiaCode_122)
{
        // Arrange
        VolumeEquation volEq = VolumeEquation::ParseVolumeEquationNumber("500WO2W122");
        //volEq.geoCode = VolumeEquation::GeoCode::R5;
        //volEq.modelType = VolumeEquation::ModelType::WO2;
        //volEq.usRegion = 'W';
        //volEq.fiaCode = 122;

        TreeMeasurment tree;
        tree.dbh = 15;
        tree.totalHeight = 60;
        // ...

        WenselOlsonTaperModel& taperModel = WenselOlsonTaperModel(volEq);

        // Act
        auto result = taperModel.GetDiameterAtHeight(tree, 20);
        auto result2 = taperModel.GetHeightAtDiameter(tree, 6.0);

        //// Assert
        //EXPECT_NEAR(result, 6.287, 0.005); // TODO wire up a value once reference output is confirmed

}
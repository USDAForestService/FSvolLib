#include <gtest/gtest.h>

#include "TaperModels/CzaplewskiTaperModel.h"
#include "VolumeEquation.h"

#include <string>


TEST(CzaplewskiTaperModelTest, GetDiameterAtHeightTest_With_FiaCode_015)
{
        // Arrange
        VolumeEquation volEq = VolumeEquation::ParseVolumeEquationNumber("200CZ3W015");

        TreeMeasurment tree;
        tree.dbh = 19.7;
        tree.totalHeight = 76.0;
        tree.referenceHeight = 30.0;
        tree.referenceDiameter = 14.0;
        double d2 = 13.4;
        double ht2 = 40;
        // ...

        //Test CZ2 and CZ3 model
        CzaplewskiTaperModel& taperModel = CzaplewskiTaperModel(volEq);

        // Act
        auto dib = taperModel.GetDiameterAtHeight(tree, ht2);
        auto ht = taperModel.GetHeightAtDiameter(tree, d2);

        //// Assert
        //EXPECT_NEAR(result, 6.287, 0.005); // TODO wire up a value once reference output is confirmed

}
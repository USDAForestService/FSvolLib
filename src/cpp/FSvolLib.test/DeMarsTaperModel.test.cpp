#include <gtest/gtest.h>

#include "TaperModels/DeMarsTaperModel.h"
#include "VolumeEquation.h"

#include <string>


TEST(DeMarsTaperModelTest, GetDiameterAtHeightTest_With_FiaCode_351)
{
        // Arrange
        VolumeEquation volEq = VolumeEquation::ParseVolumeEquationNumber("A16DEMW042");

        TreeMeasurment tree;
        tree.dbh = 19.7;
        tree.totalHeight = 76.0;
        double d2 = 13.4;
        double ht2 = 40;
        // ...

        DeMarsTaperModel& taperModel = DeMarsTaperModel(volEq);
        //Test CZ2 and CZ3 model
        //CzaplewskiTaperModel& taperModel = CzaplewskiTaperModel(volEq);
        
        // Act
        auto dib = taperModel.GetDiameterAtHeight(tree, ht2);
        auto ht = taperModel.GetHeightAtDiameter(tree, d2);

        //// Assert
        //EXPECT_NEAR(result, 6.287, 0.005); // TODO wire up a value once reference output is confirmed

}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using IoTPlatform.Domain;

namespace IoTPlatform.UnitTests
{
    [TestClass]
    public class DomainInvariantTests
    {
        [TestMethod]
        public void DeviceAsset_WithValidSerialNumber_ShouldSucceed()
        {
            // Arrange & Act
            var asset = new DeviceAsset("CNC Mill 01", "CNC-1004-9988", "Milling Machine", "Floor A");

            // Assert
            Assert.AreEqual("CNC-1004-9988", asset.SerialNumber);
            Assert.AreEqual(AssetStatus.Active, asset.Status);
        }

        [TestMethod]
        [DataRow("INVALID")]
        [DataRow("cnc-1004-9988")] // lowercase check
        [DataRow("CNC-12-3456")]   // incorrect digit lengths
        public void DeviceAsset_WithInvalidSerialNumber_ShouldThrowException(string invalidSerial)
        {
            // Act & Assert
            bool exceptionThrown = false;
            try
            {
                var asset = new DeviceAsset("CNC Mill", invalidSerial, "Milling", "Floor A");
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentException was not thrown.");
        }

        [TestMethod]
        public void DeviceAsset_DecommissionedToActiveTransition_ShouldThrowException()
        {
            // Arrange
            var asset = new DeviceAsset("CNC Lathe", "CNC-2002-4433", "Lathe", "Floor B");
            asset.UpdateStatus(AssetStatus.Decommissioned);

            // Act & Assert
            bool exceptionThrown = false;
            try
            {
                asset.UpdateStatus(AssetStatus.Active);
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected InvalidOperationException was not thrown.");
        }

        [TestMethod]
        public void TelemetryMetric_WithNegativeValue_ShouldThrowException()
        {
            // Arrange
            var assetId = Guid.NewGuid();

            // Act & Assert
            bool exceptionThrown = false;
            try
            {
                var metric = new TelemetryMetric(assetId, "SpindleSpeed", -150.0, "RPM");
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentOutOfRangeException was not thrown.");
        }

        [TestMethod]
        public void AlertRule_CriticalThresholdLowerThanWarning_ShouldThrowException()
        {
            // Arrange
            var assetId = Guid.NewGuid();

            // Act & Assert
            bool exceptionThrown = false;
            try
            {
                var rule = new AlertRule(assetId, "Vibration", 50.0, 30.0);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentException was not thrown.");
        }
    }
}
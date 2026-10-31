using System;
using System.Text.RegularExpressions;

namespace IoTPlatform.Domain
{
    public enum AssetStatus
    {
        Active,
        UnderMaintenance,
        Decommissioned
    }

    public class DeviceAsset
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string SerialNumber { get; private set; }
        public string AssetType { get; private set; }
        public string Location { get; private set; }
        public AssetStatus Status { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        // Regex mask for serial number format: e.g., CNC-1234-5678
        private static readonly Regex SerialNumberRegex = new(@"^[A-Z]{3}-\d{4}-\d{4}$", RegexOptions.Compiled);

        public DeviceAsset(string name, string serialNumber, string assetType, string location)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetSerialNumber(serialNumber);
            SetAssetType(assetType);
            SetLocation(location);
            Status = AssetStatus.Active;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Asset name cannot be null or empty.", nameof(name));
            Name = name;
        }

        public void SetSerialNumber(string serialNumber)
        {
            if (!SerialNumberRegex.IsMatch(serialNumber))
                throw new ArgumentException("Serial number must follow the format mask: ABC-1234-5678.", nameof(serialNumber));
            SerialNumber = serialNumber;
        }

        public void SetAssetType(string assetType)
        {
            if (string.IsNullOrWhiteSpace(assetType))
                throw new ArgumentException("Asset type cannot be null or empty.", nameof(assetType));
            AssetType = assetType;
        }

        public void SetLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be null or empty.", nameof(location));
            Location = location;
        }

        public void UpdateStatus(AssetStatus newStatus)
        {
            // Enforce business lifecycle rules:
            // Cannot go straight from Decommissioned back to Active without proper transition rules.
            if (Status == AssetStatus.Decommissioned && newStatus == AssetStatus.Active)
            {
                throw new InvalidOperationException("Cannot transition an asset directly from Decommissioned to Active.");
            }
            Status = newStatus;
        }
    }
}
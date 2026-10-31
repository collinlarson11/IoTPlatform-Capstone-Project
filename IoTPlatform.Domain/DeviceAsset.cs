using System;

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
            // Format requirement: ABC-1234-5678 (Total length = 13 characters)
            if (string.IsNullOrWhiteSpace(serialNumber) || serialNumber.Length != 13)
            {
                throw new ArgumentException("Serial number must follow the format mask: ABC-1234-5678.", nameof(serialNumber));
            }

            // Check that dashes are at index 3 and index 8
            if (serialNumber[3] != '-' || serialNumber[8] != '-')
            {
                throw new ArgumentException("Serial number must contain dashes at the correct positions.", nameof(serialNumber));
            }

            // Check that the first 3 characters are uppercase letters (indices 0, 1, 2)
            for (int i = 0; i < 3; i++)
            {
                if (!char.IsUpper(serialNumber[i]))
                {
                    throw new ArgumentException("The first 3 characters of the serial number must be uppercase letters.", nameof(serialNumber));
                }
            }

            // Check that the first digit group consists of numbers (indices 4, 5, 6, 7)
            for (int i = 4; i <= 7; i++)
            {
                if (!char.IsDigit(serialNumber[i]))
                {
                    throw new ArgumentException("The first number block of the serial number must be digits.", nameof(serialNumber));
                }
            }

            // Check that the final digit group consists of numbers (indices 9, 10, 11, 12)
            for (int i = 9; i <= 12; i++)
            {
                if (!char.IsDigit(serialNumber[i]))
                {
                    throw new ArgumentException("The final number block of the serial number must be digits.", nameof(serialNumber));
                }
            }

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
            if (Status == AssetStatus.Decommissioned && newStatus == AssetStatus.Active)
            {
                throw new InvalidOperationException("Cannot transition an asset directly from Decommissioned to Active.");
            }
            Status = newStatus;
        }
    }
}
using System;

namespace IoTPlatform.Domain
{
    public class MaintenanceLog
    {
        public Guid Id { get; private set; }
        public Guid DeviceAssetId { get; private set; }
        public string Description { get; private set; }
        public string TechnicianName { get; private set; }
        public DateTime PerformedAtUtc { get; private set; }

        public MaintenanceLog(Guid deviceAssetId, string description, string technicianName)
        {
            Id = Guid.NewGuid();
            DeviceAssetId = deviceAssetId;
            SetDescription(description);
            SetTechnicianName(technicianName);
            PerformedAtUtc = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Maintenance description cannot be empty.", nameof(description));
            Description = description;
        }

        public void SetTechnicianName(string technicianName)
        {
            if (string.IsNullOrWhiteSpace(technicianName))
                throw new ArgumentException("Technician name cannot be empty.", nameof(technicianName));
            TechnicianName = technicianName;
        }
    }
}
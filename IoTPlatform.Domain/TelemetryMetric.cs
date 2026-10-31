using System;

namespace IoTPlatform.Domain
{
    public class TelemetryMetric
    {
        public Guid Id { get; private set; }
        public Guid DeviceAssetId { get; private set; }
        public string MetricName { get; private set; }
        public double Value { get; private set; }
        public string Unit { get; private set; }
        public DateTime TimestampUtc { get; private set; }

        public TelemetryMetric(Guid deviceAssetId, string metricName, double value, string unit)
        {
            Id = Guid.NewGuid();
            DeviceAssetId = deviceAssetId;
            SetMetricName(metricName);
            SetValue(value);
            SetUnit(unit);
            TimestampUtc = DateTime.UtcNow;
        }

        public void SetMetricName(string metricName)
        {
            if (string.IsNullOrWhiteSpace(metricName))
                throw new ArgumentException("Metric name cannot be null or empty.", nameof(metricName));
            MetricName = metricName;
        }

        public void SetValue(double value)
        {
            // Enforce business invariant: operating limits must be positive ranges[cite: 2, 8]
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Telemetry metric value must be a positive number.");
            Value = value;
        }

        public void SetUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit cannot be null or empty.", nameof(unit));
            Unit = unit;
        }
    }
}
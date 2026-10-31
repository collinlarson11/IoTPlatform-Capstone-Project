using System;

namespace IoTPlatform.Domain
{
    public class AlertRule
    {
        public Guid Id { get; private set; }
        public Guid DeviceAssetId { get; private set; }
        public string MetricName { get; private set; }
        public double WarningThreshold { get; private set; }
        public double CriticalThreshold { get; private set; }
        public bool IsEnabled { get; private set; }

        public AlertRule(Guid deviceAssetId, string metricName, double warningThreshold, double criticalThreshold)
        {
            Id = Guid.NewGuid();
            DeviceAssetId = deviceAssetId;
            SetMetricName(metricName);
            SetThresholds(warningThreshold, criticalThreshold);
            IsEnabled = true;
        }

        public void SetMetricName(string metricName)
        {
            if (string.IsNullOrWhiteSpace(metricName))
                throw new ArgumentException("Metric name cannot be null or empty.", nameof(metricName));
            MetricName = metricName;
        }

        public void SetThresholds(double warningThreshold, double criticalThreshold)
        {
            if (warningThreshold < 0 || criticalThreshold < 0)
                throw new ArgumentException("Thresholds must be positive values.");
            if (criticalThreshold <= warningThreshold)
                throw new ArgumentException("Critical threshold must be strictly greater than warning threshold.");

            WarningThreshold = warningThreshold;
            CriticalThreshold = criticalThreshold;
        }

        public void ToggleRule(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}
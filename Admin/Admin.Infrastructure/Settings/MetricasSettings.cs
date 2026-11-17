using Admin.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Admin.Infrastructure.Settings;

public class MetricasSettings : IMetricasSettings
{
    public MetricasSettings(IConfiguration config) =>
        UsarDatosDemostracion = config.GetValue<bool>("Metricas:UsarDatosDemostracion");

    public bool UsarDatosDemostracion { get; }
}

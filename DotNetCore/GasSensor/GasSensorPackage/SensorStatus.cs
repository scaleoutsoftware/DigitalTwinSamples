using GasSensorPackage.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasSensorPackage
{
    /// <summary>
    /// Hypothetical gas sensor status.
    /// </summary>
    public enum SensorStatus
    {
        Unknown,
        Active,
        Alarmed,
        Inactive
    }
}

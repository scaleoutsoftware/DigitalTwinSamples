using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindTurbineMessages
{
    /// <summary>
    /// Ranges used for generating random WindTurbine gearbox temperature readings.
    /// </summary>
    public class TemperatureRange
    {
        public static readonly int Idle = 20;
        public static readonly int NormalOperationMin = 100;
        public static readonly int NormalOperationMax = 150;
        public static readonly int OverheatMax = 200;
    }
}

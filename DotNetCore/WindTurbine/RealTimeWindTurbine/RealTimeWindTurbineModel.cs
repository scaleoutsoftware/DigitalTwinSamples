using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeWindTurbine
{
    /// <summary>
    /// Define state for the module here. This class defines the objects
    /// stored in the ScaleOut StateServer (SOSS) service that hold state for the 
    /// messaging module.
    /// </summary>
    public class RealTimeWindTurbineModel : DigitalTwinBase<RealTimeWindTurbineModel>
    {
        public List<double> GearboxTemps { get; } = new List<double>();

        public List<double> RPMs { get; } = new List<double>();

    }
}

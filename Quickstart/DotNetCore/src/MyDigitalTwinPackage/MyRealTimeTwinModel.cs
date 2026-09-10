using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalTwinPackage
{
    /// <summary>
    /// Define any state for the digital twin here. This class defines the objects
    /// stored in the ScaleOut StateServer (SOSS) service that hold state for the 
    /// digital twin module.
    /// </summary>
    public class MyRealTimeTwinModel : DigitalTwinBase<MyRealTimeTwinModel>
    {
        public string? CurrentValue { get; set; }


        public override async Task InitAsync(string id, string model, InitContext<MyRealTimeTwinModel> initContext)
        {
            await base.InitAsync(id, model);

            // Initialization logic for a digital twin instance goes here.
            // This method is called when a new digital twin instance is created.
        }

    }
}

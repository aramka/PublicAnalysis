using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.Visuals.Models
{
    public record Visual(
        VisualType VisualType,
        string DataSetName,
        string DatapointName
    );
}

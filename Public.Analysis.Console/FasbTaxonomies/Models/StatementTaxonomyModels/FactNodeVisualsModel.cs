using Public.Analysis.Console.Visuals.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    public class FactNodeVisualsModel
    {
        public FactNodeModel FactNode { get; set; } = new FactNodeModel();
        public Visual[] Visuals { get; set; } = [];
    }
}

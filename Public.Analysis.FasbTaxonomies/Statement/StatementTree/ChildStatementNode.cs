using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public record ChildStatementNode(ElementId ChildElementId, string ChildLabel, decimal Order)
    {

    }
}

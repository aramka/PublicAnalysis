using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public record ChildStatementNode(ElementId Id, string Label, decimal Order) // TODO: rename Id to ChildId. Will have to fix json tree and possibly UI
    {

    }
}

using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public interface IStatementNode
    {
        IXsElement XsElement { get; }

        /// <summary>
        /// Sets the parent if not already set. If the parent has already been set <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        ElementId?  ParentElementId { get; set; }

        /// <summary>
        /// If the child does not exist it is added. If the child exists <see cref="InvalidOperationException"/> is thrown. 
        /// </summary>
        /// <param name="child"></param>
        void AddChild(IStatementNode child);

        IEnumerable<IStatementNode> Children { get; }

        string Label { get; }

        /// <summary>
        /// Sets the order if not order is equal to zero. If the order is not equal zero then <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        decimal Order { get; set; }
        ElementId ElementId { get; }
    }
}

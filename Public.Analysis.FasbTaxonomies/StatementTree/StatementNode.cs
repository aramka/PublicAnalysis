using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public interface IStatementNode
    {
        IXsElement USGaapElement { get; }

        /// <summary>
        /// Sets the parent if not already set. If the parent has already been set <see cref="InvalidOperationException"/> is throw.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        IStatementNode?  Parent { get; set; }

        /// <summary>
        /// Adds the child does not exist it is added. If the child exists <see cref="InvalidOperationException"/> is throw. 
        /// </summary>
        /// <param name="child"></param>
        void AddChild(IStatementNode child);

        public string Label { get; }

    }
}

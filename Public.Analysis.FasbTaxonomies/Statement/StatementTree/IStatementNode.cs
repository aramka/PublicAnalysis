using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public interface IStatementNode
    {
        [JsonIgnore]
        IXsElement FactInfo { get; }

        IEnumerable<ElementId> ParentsElementIds { get; }

        public void AddParent(ElementId parentElementId);

        /// <summary>
        /// If the child does not exist it is added. If the child exists <see cref="InvalidOperationException"/> is thrown. 
        /// </summary>
        /// <param name="child"></param>
        void AddChild(ElementId childElementId, string childLabel, decimal order);
        void RemoveParent(ElementId id);
        void RemoveChild(ElementId childId);

        IEnumerable<ChildStatementNode> Children{ get; }

        string Label { get; }

        ElementId Id { get; }

        string Name { get; }
    }
}

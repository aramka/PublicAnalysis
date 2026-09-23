using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement
{
    public class StatementModel
    {
        public StatementModel(string name, string description, string id,IReadOnlyDictionary<ElementId,IStatementNode> tree)
        {
            this.Name = name;
            this.Description = description;
            this.Id = id;
            this.Tree = tree;
        }
        public string Name { get; }
        public string Description { get; }

        public string Id { get; }

        public IReadOnlyDictionary<ElementId, IStatementNode> Tree { get; }

    }
}

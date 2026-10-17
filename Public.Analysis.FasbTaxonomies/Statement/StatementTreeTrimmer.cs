using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement
{
    public class StatementTreeTrimmer
    {
        public IReadOnlyDictionary<ElementId, IStatementNode> RemoveNodeMoveChildrenUp(ElementId id, IReadOnlyDictionary<ElementId, IStatementNode> tree)
        {
            if(!tree.TryGetValue(id, out IStatementNode? nodeToRemove))
            {
                throw new InvalidOperationException($"Node with id {id} does not exist in tree.");
            }

            foreach(ElementId parentId in nodeToRemove.ParentsElementIds)
            {
                if(!tree.TryGetValue(parentId, out IStatementNode? newParent))
                {
                    throw new InvalidOperationException($"Parent with {nameof(ElementId)} {parentId} not found in tree");
                }

                foreach(var parentChild in nodeToRemove.Children)
                {
                    if(!tree.TryGetValue(parentChild.Id, out IStatementNode? childNode))
                    {
                        throw new InvalidOperationException($"Child with {nameof(ElementId)} {parentChild.Id} not found in tree");
                    }

                    childNode.RemoveParent(nodeToRemove.Id);

                    childNode.AddParent(newParent.Id);
                    newParent.AddChild(childNode.Id, parentChild.Label, parentChild.Order);
                }

                newParent.RemoveChild(nodeToRemove.Id);
            }

            return tree.Where(kvp => kvp.Key != id).ToDictionary();
        }
    }
}

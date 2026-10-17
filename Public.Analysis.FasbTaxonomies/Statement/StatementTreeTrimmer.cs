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
                if(!tree.TryGetValue(parentId, out IStatementNode? parent))
                {
                    throw new InvalidOperationException($"Parent with {nameof(ElementId)} {parentId} not found in tree");
                }
                parent.RemoveChild(nodeToRemove.Id);
            }

            foreach (var parentChild in nodeToRemove.Children)
            {
                if (!tree.TryGetValue(parentChild.Id, out IStatementNode? childNode))
                {
                    throw new InvalidOperationException($"Child with {nameof(ElementId)} {parentChild.Id} not found in tree");
                }
                
                childNode.RemoveParent(nodeToRemove.Id);

                foreach (ElementId parentId in nodeToRemove.ParentsElementIds)
                {
                    
                    childNode.AddParent(parentId);
                    tree[parentId].AddChild(childNode.Id, parentChild.Label, parentChild.Order);
                }
            }

            return tree.Where(kvp => kvp.Key != id).ToDictionary();
        }
    }
}

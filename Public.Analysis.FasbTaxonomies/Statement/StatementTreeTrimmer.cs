using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement
{
    public class StatementTreeTrimmer
    {
        public IReadOnlyDictionary<ElementId, IStatementNode> RemoveBranch(ElementId id, IReadOnlyDictionary<ElementId, IStatementNode> tree)
        {
            // ThrowIfNodeDoesNotExists(id, tree, out IStatementNode? node);

            HashSet<ElementId> toRemove = new HashSet<ElementId>();
            List<(ElementId parentId, ElementId childId)> parentsOfToRemove = new();

            Queue<ElementId> queue = new Queue<ElementId>();
            queue.Enqueue(id);

            while (queue.Any())
            {
                var currentId = queue.Dequeue();
                
                ThrowIfNodeDoesNotExists(currentId, tree, out IStatementNode? currentNode);

                toRemove.Add(currentId);

                parentsOfToRemove.AddRange(currentNode.ParentsElementIds.Select(pId => (pId, currentId)));

                foreach(ChildStatementNode child in currentNode.Children)
                {
                    queue.Enqueue(child.Id);
                }
            }

            var parentsNotInBranch = parentsOfToRemove
                .Where(parentChild => !toRemove.Contains(parentChild.parentId));

            foreach((ElementId parentId, ElementId childId) in parentsNotInBranch)
            {
                ThrowIfNodeDoesNotExists(parentId, tree, out IStatementNode parentNode, "Parent");

                parentNode.RemoveChild(childId);
            }

            return tree.Where(kvp=>!toRemove.Contains(kvp.Key)).ToDictionary();
        }

        public IReadOnlyDictionary<ElementId, IStatementNode> RemoveNodeMoveChildrenUp(ElementId id, IReadOnlyDictionary<ElementId, IStatementNode> tree)
        {
            
            ThrowIfNodeDoesNotExists(id, tree, out IStatementNode? nodeToRemove);

            foreach(ElementId parentId in nodeToRemove.ParentsElementIds)
            {
                ThrowIfNodeDoesNotExists(parentId, tree, out IStatementNode parent, nodeName: "Parent");
                parent.RemoveChild(nodeToRemove.Id);
            }

            foreach (var parentChild in nodeToRemove.Children)
            {

                ThrowIfNodeDoesNotExists(parentChild.Id, tree, out IStatementNode childNode, nodeName: "Child");
                
                childNode.RemoveParent(nodeToRemove.Id);

                foreach (ElementId parentId in nodeToRemove.ParentsElementIds)
                {
                    
                    childNode.AddParent(parentId);
                    tree[parentId].AddChild(childNode.Id, parentChild.Label, parentChild.Order);
                }
            }
            return tree.Where(kvp => kvp.Key != id).ToDictionary();
        }

        private void ThrowIfNodeDoesNotExists(ElementId id, IReadOnlyDictionary<ElementId, IStatementNode> tree,  out IStatementNode node, string nodeName = "Node")
        {
            if (!tree.TryGetValue(id, out IStatementNode? outNode))
            {
                throw new InvalidOperationException($"{nodeName} with {nameof(ElementId)} {id} does not exist in tree");
            }
            node = outNode;
        }
    }
}

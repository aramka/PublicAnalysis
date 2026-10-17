using Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public interface INodesBuilder
    {
        Dictionary<ElementId, IStatementNode> BuildNodes(PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, IReadOnlyDictionary<LabelRole, string>> labelsByElementId);
    }
}
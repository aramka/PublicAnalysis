using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.Xml;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    public static class Extensions
    {
        public static IEnumerable<HRef> ToHRef(this IEnumerable<Loc>? loc) => loc?.Select(l => new HRef(l.href)) ?? Enumerable.Empty<HRef>();
    }
}

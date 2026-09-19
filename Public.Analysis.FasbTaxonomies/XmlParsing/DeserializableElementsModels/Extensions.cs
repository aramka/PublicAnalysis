using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    public static class Extensions
    {
        public static IEnumerable<HRef> ToHRef(this IEnumerable<Loc>? loc) => loc?.Select(l => new HRef(l.Href)) ?? Enumerable.Empty<HRef>();
    }
}

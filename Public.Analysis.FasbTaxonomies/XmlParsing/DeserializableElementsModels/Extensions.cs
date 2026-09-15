using Public.Analysis.FasbTaxonomies.XmlParsing.Xml;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    public static class Extensions
    {
        public static IEnumerable<HRef> ToHRef(this IEnumerable<linkbasePresentationLinkLoc>? loc) => loc?.Select(l => new HRef(l.href)) ?? Enumerable.Empty<HRef>();
    }
}

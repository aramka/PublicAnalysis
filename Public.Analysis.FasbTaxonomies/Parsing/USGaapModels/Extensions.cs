using Public.Analysis.FasbTaxonomies.Parsing.USGaapModels;
using Public.Analysis.FasbTaxonomies.Parsing.Xml;

namespace Public.Analysis.FasbTaxonomies.Parsing.USGaapModels
{
    public static class Extensions
    {
        public static IEnumerable<HRef> ToHRef(this IEnumerable<linkbasePresentationLinkLoc>? loc) => loc?.Select(l => new HRef(l.href)) ?? Enumerable.Empty<HRef>();
    }
}

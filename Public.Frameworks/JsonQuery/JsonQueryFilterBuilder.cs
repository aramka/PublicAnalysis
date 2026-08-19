using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryFilterBuilder
    {
        readonly List<string> paths = new List<string>();
        private List<string> filters = new List<string>();

        public IEnumerable<string> Paths => paths.Select(a=>a).ToArray();

        public IEnumerable<string> Filters => this.filters.Select(a => a).ToArray();

        public JsonQueryFilterBuilder AddFilter(IJsonQueryFilterExpression filter)
        {
            if(filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            var filterString = filter.AsJsonPathQueryExpression();
            if(string.IsNullOrWhiteSpace(filterString))
            {
                throw new ArgumentException("Filter expression cannot be empty.", nameof(filter));
            }
            this.filters.Add(filterString);
            return this;
        }

        public JsonQueryFilterBuilder AddPath(string v)
        {
            paths.Add($"['{v}']");
            return this;
        }

        public string AsJsonPathQueryString()
        {
            throw new NotImplementedException();
        }
    }
}
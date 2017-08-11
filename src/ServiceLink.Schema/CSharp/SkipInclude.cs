using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ServiceLink.Schema.CSharp
{
    public abstract class SkipInclude
    {
        public IReadOnlyCollection<string> Names { get; }

        private SkipInclude(IEnumerable<string> names)
        {
            Names = new ReadOnlyCollection<string>(names.ToList());
        }

        public class SkipClass : SkipInclude
        {
            public SkipClass(IEnumerable<string> names) : base(names)
            {
            }

            public override bool IsSkip(string name)
                => Names.Contains(name);
        }

        public class IncludeClass : SkipInclude
        {
            public IncludeClass(IEnumerable<string> names) : base(names)
            {
            }

            public override bool IsSkip(string name)
                => !Names.Contains(name);
        }

        public abstract bool IsSkip(string name);
        public static readonly SkipInclude IncludeAll = new SkipInclude.SkipClass(new string[0]);
    }

    public static class SkipIncludeExtensions
    {
        public static SkipInclude Skip(this IEnumerable<string> names)
            => new SkipInclude.SkipClass(names);
        public static SkipInclude Include(this IEnumerable<string> names)
            => new SkipInclude.IncludeClass(names);

       
    }
}
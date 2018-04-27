using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegularToy
{
    public class MatchResult
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public MatchResult[] Items { get; private set; }

        public static MatchResult[] Build(MatchCollection matches)
        {
            if (matches == null)
                return null;

            return (from m in matches.Cast<Match>()
                    select new MatchResult()
                    {
                        Type = "M",
                        Value = m.Value,
                        Items = (from g in m.Groups.Cast<Group>()
                                 where !(g is Match)
                                 select new MatchResult()
                                 {
                                     Type = "G",
                                     Value = g.Value,
                                     Items = (from c in g.Captures.Cast<Capture>()
                                              select new MatchResult()
                                              {
                                                  Type = "C",
                                                  Value = c.Value
                                              }).ToArray()
                                 }).ToArray()
                    }).ToArray();
        }
    }
}

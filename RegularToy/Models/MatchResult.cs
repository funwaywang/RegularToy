using System.Linq;
using System.Text.RegularExpressions;

namespace RegularToy
{
    public class MatchResult
    {
        public int Index { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public MatchResult[] Items { get; private set; }

        public static MatchResult[] Build(MatchCollection matches)
        {
            if (matches == null)
            {
                return null;
            }

            var list = (from m in matches.Cast<Match>()
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
                                                      Value = c.Value,
                                                  }).ToArray()
                                     }).ToArray()
                        }).ToArray();
            SetIndex(list);
            return list;
        }

        private static void SetIndex(MatchResult[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i].Index = i + 1;
                if (list[i].Items != null && list[i].Items.Any())
                {
                    SetIndex(list[i].Items);
                }
            }
        }
    }
}

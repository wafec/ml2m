using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Model
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public List<SongItem> Items { get; set; }
        public bool HasErrors { get; set; }

        public string QualifiedName
        {
            get
            {
                return string.Format("{0} - {1}", Name, Album);
            }
        }

        public string VerticalName
        {
            get
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    var parts = Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 3 || true || (Name.Length > 20 && parts.Length == 3))
                    {
                        var i = (int)Math.Ceiling(parts.Length / 2.0);
                        var j = i;
                        return string.Format("{0}\n{1}",
                            string.Join(" ", parts.Take(i)),
                            string.Join(" ", parts.Skip(j))
                            );                        
                    }
                    else
                    {
                        return Name;
                    }
                }
                return "";
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Album) && Items != null && Items.Count > 0;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Album: {1}, Size: {2}, HasErrors: {3}", 
                Name, Album, Items != null ? Items.Count : 0, HasErrors);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JNL.Model
{
    public partial class ViewLine
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int Sort { get; set; }
    }
}

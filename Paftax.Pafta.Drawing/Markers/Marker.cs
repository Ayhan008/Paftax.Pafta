using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paftax.Pafta.Drawing.Markers
{
    internal abstract class Marker
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Number { get; set; }
    }
}

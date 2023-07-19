using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsChallengeApp.Models
{
    public class GameInfo
    {
        public List<Boss>? Bosses { get; set; }
        public List<Restriction>? Restrictions { get; set; }
    }
}

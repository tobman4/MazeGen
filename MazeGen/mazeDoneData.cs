using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGen {
    public class mazeDoneData : EventArgs {
        public TimeSpan timeUsed;
        public int seed;
        public int width;
        public int heigth;
    }
}

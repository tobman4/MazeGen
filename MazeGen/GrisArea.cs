using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MazeGen {
    class GridArea {
        public Point home;
        public Size size;

        public Point Entry;

        public int cellCount;
       
        public GridArea(Point homei, Size sizei) {
            this.home = homei;
            this.size = sizei;
            cellCount = size.Width * size.Height;
            Entry = new Point(0,0);
        }

        public GridArea(int homeX, int homeY, int w, int h) {
            this.home = new Point(homeX,homeY);
            this.size = new Size();
            cellCount = size.Width * size.Height;
            Entry = new Point(0, 0);
        }

    }
}

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

        public Point TL {
            get {
                return new Point(home.X, home.Y);
            }
        }
        public Point TR {
            get {
                return new Point(home.X+size.Width, home.Y);
            }
        }
        public Point BL {
            get {
                return new Point(home.X, home.Y+size.Height);
            }
        }
        public Point BR {
            get {
                return new Point(home.X + size.Width, home.Y+size.Height);
            }
        }

        public int cellCount;
        public int doneCells = 0;
       
        public GridArea(Point homei, Size sizei) {
            this.home = homei;
            this.size = sizei;
            cellCount = size.Width * size.Height;
        }

        public GridArea(int homeX, int homeY, int w, int h) {
            this.home = new Point(homeX,homeY);
            this.size = new Size(w,h);
            cellCount = size.Width * size.Height;
        }

    }
}

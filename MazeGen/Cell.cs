using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace MazeGen {

    public enum wall : int {
        Top = 0,
        Rigth = 1,
        Down = 2,
        Left = 3,
    }

    public class Cell : IDisposable {

        public bool DBG = false;
        public bool isDone = false;
        
        public int x;
        public int y;


        private bool[] walls;

        static public bool startOpen = false;
        /*
         * wall array
           0
        3 pos 1
           2
        */

        public Cell(int xi, int yi) {
            x = xi;
            y = yi;

            walls = new bool[] { !startOpen, !startOpen, !startOpen, !startOpen };
        }

        public void setWall(wall w, bool state) => walls[(int)w] = state;
        public bool getWall(wall w) => walls[(int)w];

        public int openings() {
            int o = 0;
            o += walls[0] ? 1 : 0;
            o += walls[1] ? 1 : 0;
            o += walls[2] ? 1 : 0;
            o += walls[3] ? 1 : 0;
            return o;
        }

        public override string ToString() {
            return $"Cell({x},{y})";
        }

        public void Dispose() {
        }
    }
}

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

    class Cell : IDisposable {

        public bool DBG = false;
        
        public int x;
        public int y;

        public bool isDone = false;

        private bool[] walls = new bool[] { true, true, true, true };
        /*
         * wall array
           0
        3 pos 1
           2
        */

        public Cell(int xi, int yi) {
            x = xi;
            y = yi;
        }

        public void setWall(wall w, bool state) => walls[(int)w] = state;
        public bool getWall(wall w) => walls[(int)w];

        public override string ToString() {
            return $"Cell({x},{y})";
        }

        public void Dispose() {
        }
    }
}

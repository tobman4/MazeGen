using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MazeGen {
    public class Maze : IDisposable {

        public event EventHandler<mazeDoneData> mazeDoneEvent;

        public Cell[,] grid;
        private int width, height;

        public float prosentDone = 0;
        public int cellCount;
        public int cellsDone = 0;

        public Mode mode {
            get;
            private set;
        }

        public Maze(int width, int height) {

            mode = Mode.Preping;

            grid = new Cell[width, height];

            this.width = width;
            this.height = height;

            cellCount = width * height;

            ThreadPool.QueueUserWorkItem(this.initGrid);


        }

        private void initGrid(object stateData) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    grid[i, j] = new Cell(i, j);
                }
            }
            mode = Mode.NotMade;
        }

        private List<Cell> getFrends(int x, int y) {
            List<Cell> o = new List<Cell>();
            try {
                o.Add(grid[x + 1, y]);
            } catch { }
            try {
                o.Add(grid[x - 1, y]);
            } catch { }
            try {
                o.Add(grid[x, y + 1]);
            } catch { }
            try {
                o.Add(grid[x, y - 1]);
            } catch { }

            return o;
        }

        public void makePath(ref int doneCells,ref float prosent, int seed = -99) {
            
            if(Thread.CurrentThread.Name == "MAIN" && mode == Mode.Preping) {
                throw new Exception("Maze not ready");
            } else if(mode == Mode.Preping) {
                while(mode == Mode.Preping) {
                    Thread.Sleep(500);
                }
            }
            
            DateTime startTime = DateTime.Now;

            Debug.WriteLine("Star making maze");

            mode = Mode.Making;
            prosentDone = 0;
            doneCells = 0;

            Random r;
            if(seed != -99) {
                r = new Random(seed);
            } else {
                r = new Random();
            }

            List<Cell> stack = new List<Cell> { grid[0,0] };

            bool flag = false;
            while(!flag) {

                if(mode == Mode.Stoped) {
                    return;
                } else if(mode == Mode.Paused && Thread.CurrentThread.Name != "MAIN") {
                    ThreadPriority prePause = Thread.CurrentThread.Priority;
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    do {
                        Thread.Sleep(500);
                    } while (mode == Mode.Paused);
                    Thread.CurrentThread.Priority = prePause;
                }

                prosent = ((float)doneCells / (float)cellCount)*100;

                Cell curr = stack[stack.Count-1];
                if(!curr.isDone) {
                    curr.isDone = true;
                    doneCells++;
                }
                
                List<Cell> frends = getFrends(curr.x,curr.y);

                for (int i = frends.Count; i > 0; i--) {
                    if(frends[i-1].isDone) {
                        frends.Remove(frends[i-1]);
                    }
                }

                if (frends.Count == 0) {
                    
                    stack.RemoveAt(stack.Count-1);
                    if (stack.Count == 0) {
                        flag = true;
                    }
                } else {


                    int index = r.Next(frends.Count);

                    Cell next = frends[index];
                    if(curr.x < next.x) {
                        curr.setWall(wall.Rigth,false);
                        next.setWall(wall.Left,false);
                    } else if(curr.y < next.y) {
                        curr.setWall(wall.Down, false);
                        next.setWall(wall.Top, false);
                    } else if(curr.y > next.y) {
                        curr.setWall(wall.Top, false);
                        next.setWall(wall.Down, false);
                    } else if(curr.x > next.x) {
                        curr.setWall(wall.Left, false);
                        next.setWall(wall.Rigth, false);
                    }

                    if (frends.Count == 1) {
                        stack.Remove(curr);
                    }

                    stack.Add(next);
                }
            }

            mazeDoneData data = new mazeDoneData();
            data.seed = seed;
            data.timeUsed = DateTime.Now - startTime;

            mazeDoneEvent.Invoke(this,data);

            mode = Mode.Done;
        }

        private void showArea(GridArea area) {
            grid[area.home.X, area.home.Y].DBG = true;
            for (int i = area.home.Y; i < area.size.Height; i++) {
                grid[area.home.X, 10 + i].setWall(wall.Rigth, true);
            }
        }

        private void makePathArea(GridArea area, Random rng) {
            showArea(area);
        }

        public void makePathThread(int seed = -99) {
            DateTime startTime = DateTime.Now;
            
            Thread.Sleep(100);
            
            Random r;
            if (seed != -99) {
                r = new Random(seed);
            } else {
                r = new Random();
            }

            GridArea g = new GridArea(0, 0, this.width, this.height);
            makePathArea(g,r);

            /*
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    grid[i, j].setWall(wall.Down,false);
                    grid[i, j].setWall(wall.Left, false);
                    grid[i, j].setWall(wall.Rigth, false);
                    grid[i, j].setWall(wall.Top, false);
                }
            }
            */


            mazeDoneData data = new mazeDoneData();
            data.seed = seed;
            data.timeUsed = DateTime.Now - startTime;

            mazeDoneEvent.Invoke(this, data);


            mode = Mode.Done;
        }

        public void stop() {
            if(mode == Mode.Making) {
                mode = Mode.Stoped;
            }
        }

        [Obsolete]
        public Bitmap makeImage() {
            bool wasPaused = mode == Mode.Making;
            if(wasPaused) {
                mode = Mode.Paused;
                Thread.Sleep(500); // give it time to get into the pause loop
            }

            Bitmap o = new Bitmap(width*2,height*2);
            for(int i = 0; i < o.Width; i++) {
                for(int j = 0; j < o.Height; j++) {
                    if(i%2 == 0 && j%2 == 0) {

                        Color cellColor = (grid[i / 2, j / 2].DBG) ? Color.FromArgb(0,255,0,255) : Color.White;
                        o.SetPixel(i,j, cellColor);
                        
                        bool wall_rigth = grid[i / 2, j / 2].getWall(wall.Rigth);
                        bool wall_down = grid[i / 2, j / 2].getWall(wall.Down);
                        if(i < o.Width) {
                            o.SetPixel(i+1,j,(wall_rigth) ? Color.Black : Color.White);
                        }
                        if (j < o.Height) {
                            o.SetPixel(i, j+1, (wall_down) ? Color.Black : Color.White);
                        }
                    }
                }
            }
            if(wasPaused) {
                mode = Mode.Making;
            }
            return o;
        }
        
        public Bitmap getImage(Point pos, Size size) {
            Bitmap o = new Bitmap(
                (int)Math.Ceiling((decimal)size.Width * 2),
                (int)Math.Ceiling((decimal)size.Height * 2)
            );

            if (pos.X < 0 || pos.X >= width || pos.Y < 0 || pos.Y >= height) {
                throw new Exception("");
            }

            
            pos.X -= Math.Max(0, pos.X - size.Width / 2);
            pos.Y -= Math.Max(0, pos.Y - size.Height / 2);
            /*
            if(pos.X != 0) {
                size.Width /= 2;
            }

            if(pos.Y != 0) {
                size.Height /= 2;
            }
            */

            for (int i = 0; i < o.Width; i++) {
                for (int j = 0; j < o.Height; j++) {
                    if (i % 2 == 0 && j % 2 == 0) {

                        int x = pos.X + i/2;
                        int y = pos.Y + j/2;

                        Color cellColor = (grid[x,y].DBG) ? Color.Pink : Color.White;
                        o.SetPixel(i, j, cellColor);

                        bool wall_rigth = grid[x,y].getWall(wall.Rigth);
                        bool wall_down = grid[x,y].getWall(wall.Down);
                        if (i < o.Width) {
                            o.SetPixel(i + 1, j, (wall_rigth) ? Color.Black : Color.White);
                        }
                        if (j < o.Height) {
                            o.SetPixel(i, j + 1, (wall_down) ? Color.Black : Color.White);
                        }
                    }
                }
            }
            return o;
        }

        public void Dispose() {

            for(int i = 0; i < width; i++) {
                for(int j = 0; j < height; j++) {
                    grid[i, j].Dispose();
                }
            }
        }
    }
}

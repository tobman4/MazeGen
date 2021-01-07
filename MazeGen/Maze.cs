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
        public int width { get; private set; }
        public int height { get; private set; }

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

        private Cell getCell(int x, int y, GridArea g) {
            if (x < g.home.X || x > g.home.X + g.size.Width ||
                y < g.home.Y || y > g.home.Y + g.size.Height) {
                throw new Exception("Cell not in gridarea");
            } else if (x < 0 || x >= width || y < 0 || y >= height) {
                throw new Exception("cell not in grid");
            } else {
                return grid[x,y];
            }
        }

        private List<Cell> getFrends(int x, int y, GridArea g = null) {
            List<Cell> o = new List<Cell>();

            if(g == null) {
                g = new GridArea(25,25,width-25,height-25);
            }

            // TODO: remove try catch

            try {
                o.Add(getCell(x+1,y,g));
            } catch { }
            try {
                o.Add(getCell(x-1,y,g));
            } catch { }
            try {
                o.Add(getCell(x,y+1,g));
            } catch { }
            try {
                o.Add(getCell(x,y-1,g));
            } catch { }

            return o;
        }

        private void openGrids(GridArea A, GridArea B,Random r) {
            if(A.home.Y == B.home.Y) {
                int start = A.home.Y;
                int X;
                if(A.home.X < B.home.X) {
                    X = B.home.X-1;
                    
                } else {
                    X = A.home.X-1;
                }
                for(int i = start; i < start + A.size.Height; i++) {
                    if(grid[X,i].openings() == 1) {
                        grid[X, i].setWall(wall.Rigth,false);
                    }
                }
            } else {
                int start = A.home.X;
                int Y;
                if (A.home.Y < B.home.Y) {
                    Y = B.home.Y - 1;

                } else {
                    Y = A.home.Y - 1;
                }

                for (int i = start; i < start + A.size.Width; i++) {
                    if(grid[i, Y].openings() == 1) {
                        grid[i, Y].setWall(wall.Down, false);
                    } else if(grid[i, Y].openings() == 2 && r.Next(0,100) < 10) {

                    }
                }
            }
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
                if (!curr.isDone) {
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

        private void makePathSplit(GridArea g, Random r) {

            List<Cell> stack = new List<Cell> { grid[g.home.X,g.home.Y] };

            bool flag = false;
            while (!flag) {

                if (mode == Mode.Stoped) {
                    return;
                } else if (mode == Mode.Paused && Thread.CurrentThread.Name != "MAIN") {
                    ThreadPriority prePause = Thread.CurrentThread.Priority;
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    do {
                        Thread.Sleep(500);
                    } while (mode == Mode.Paused);
                    Thread.CurrentThread.Priority = prePause;
                }

                Cell curr = stack[stack.Count - 1];
                if (!curr.isDone) {
                    curr.isDone = true;
                    g.doneCells++;
                }

                List<Cell> frends = getFrends(curr.x, curr.y,g);

                for (int i = frends.Count; i > 0; i--) {
                    if (frends[i - 1].isDone) {
                        frends.Remove(frends[i - 1]);
                    }
                }

                if (frends.Count == 0) {

                    stack.RemoveAt(stack.Count - 1);
                    if (stack.Count == 0) {
                        flag = true;
                    }
                } else {


                    int index = r.Next(frends.Count);

                    Cell next = frends[index];
                    if (curr.x < next.x) {
                        curr.setWall(wall.Rigth, false);
                        next.setWall(wall.Left, false);
                    } else if (curr.y < next.y) {
                        curr.setWall(wall.Down, false);
                        next.setWall(wall.Top, false);
                    } else if (curr.y > next.y) {
                        curr.setWall(wall.Top, false);
                        next.setWall(wall.Down, false);
                    } else if (curr.x > next.x) {
                        curr.setWall(wall.Left, false);
                        next.setWall(wall.Rigth, false);
                    }

                    if (frends.Count == 1) {
                        stack.Remove(curr);
                    }

                    stack.Add(next);
                }
            }

        }

        public void makePathThread(ref int doneCells, ref float prosent, int seed = -99) {

            if (Thread.CurrentThread.Name == "MAIN" && mode == Mode.Preping) {
                throw new Exception("Maze not ready");
            } else if (mode == Mode.Preping) {
                while (mode == Mode.Preping) {
                    Thread.Sleep(500);
                }
            }

            DateTime startTime = DateTime.Now;
            
            Random r;
            if (seed != -99) {
                r = new Random(seed);
            } else {
                r = new Random();
            }

            int w = width / 2;
            int h = height / 2;

            GridArea TL = new GridArea(0,0,w,h);
            GridArea TR = new GridArea(w+1,0,w-2,h);

            GridArea BR = new GridArea(w + 1, h+1, w - 2, h-2);
            GridArea BL = new GridArea(0, h+1, w, h-2);

            Thread[] threads = {
                makeThread(TL,new Random(r.Next())),
                makeThread(TR,new Random(r.Next())),
                makeThread(BL,new Random(r.Next())),
                makeThread(BR,new Random(r.Next())),
            };

            foreach(Thread t in threads) {
                t.Start(); // TODO high prio
            }

            bool isdone = false;
            while(!isdone) {
                doneCells = TL.doneCells + TR.doneCells + BL.doneCells + BR.doneCells;

                prosent = ((float)doneCells / (float)cellCount) * 100;

                isdone = true;
                foreach(Thread t in threads) {
                    if(t.IsAlive) {
                        isdone = false;
                    }
                }
            }

            openGrids(TL, TR, r);
            openGrids(TR, BR, r);
            openGrids(BR, BL, r);
            openGrids(BL, TL, r);

            mazeDoneData data = new mazeDoneData();
            data.seed = seed;
            data.timeUsed = DateTime.Now - startTime;
            mazeDoneEvent.Invoke(this, data);


            mode = Mode.Done;
        }

        private Thread makeThread(GridArea g, Random r) {
            return new Thread(() => {
                makePathSplit(g, r);
            });

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
                (int)Math.Ceiling((decimal)size.Width * 2)+2,
                (int)Math.Ceiling((decimal)size.Height * 2)+2
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

            for (int i = 1; i < o.Width; i++) {
                for (int j = 1; j < o.Height; j++) {
                    if (i % 2 == 0 && j % 2 == 0) {

                        int x = pos.X + (i-1)/2;
                        int y = pos.Y + (j-1)/2;

                        Color cellColor = (grid[x,y].DBG) ? Color.FromArgb(0, 255, 0, 255) : Color.White;
                        o.SetPixel(i, j, cellColor);

                        if(grid[x, y].DBG) {
                            Debug.WriteLine($"X: {i} Y: {j} IS DBG");
                        }

                        bool wall_rigth = grid[x,y].getWall(wall.Rigth);
                        bool wall_down = grid[x,y].getWall(wall.Down);
                        if (i < o.Width-1) {
                            o.SetPixel(i + 1, j, (wall_rigth) ? Color.Black : Color.White);
                        }
                        if (j < o.Height-1) {
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

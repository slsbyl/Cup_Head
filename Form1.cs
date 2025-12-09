using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cup_Head
{
    class Hero
    {
        public Rectangle src;
        public Rectangle dst;
        public List<Bitmap> Idle = new List<Bitmap>();
        public List<Bitmap> Run = new List<Bitmap>();
        public int Frame = 0;
        public int state = 0;
    }
    class Circle
    {
        public int x;
        public int y;
        public Bitmap img;
        public int dy=1;
        public int dx = 1;

    }

    public partial class Form1 : Form
    {
        Bitmap bg;
        int bgX = 0;  
        int ctcircle = 0;
        int fcircle = 0;


        Timer t = new Timer();
        Bitmap off;
        List<Hero> Lhero = new List<Hero>();
        List<Circle> Circles = new List<Circle>();

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

        }
        void creatCercles()
        {
            Circle c = new Circle();
            c.x = 200;
            c.y = 200;
            c.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\Light Orange\\8ball_bg_ball_light-orange-solid.png");
            Circles.Add(c);

            Circle c1 = new Circle();
            c1.x = this.Width-200;
            c1.y = 250;
            c1.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\Orange\\8ball_bg_ball_orange-solid.png");
            Circles.Add(c1);

            Circle c3 = new Circle();
            c3.x = this.Width - 200;
            c3.y = 150;
            c3.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\Purple\\8ball_bg_ball_purple-solid.png");
            Circles.Add(c3);

            Circle c5 = new Circle();
            c5.x = this.Width - 200;
            c5.y = this.Height - 250;
            c5.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\White and Orange Stripes\\8ball_bg_ball_white-orange-stripe.png");
            Circles.Add(c5);

            Circle c4 = new Circle();
            c4.x = this.Width - 300;
            c4.y = this.Height-200;
            c4.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\Red\\8ball_bg_ball_red-solid.png");
            Circles.Add(c4);

            Circle c6 = new Circle();
            c6.x = 100;
            c6.y = this.Height /4+100;
            c6.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\White and Blue Stripes\\8ball_bg_ball_white-blue-stripe.png");
            Circles.Add(c6);

            Circle cue = new Circle();
            cue.x = 150;
            cue.y = this.Height - 300;
            cue.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\Cue\\8ball_bg_cue_back.png");
            Circles.Add(cue);

            Circle Rack = new Circle();
           Rack.x = this.Width/2-300;
           Rack.y = this.Height/2-350;
            Rack.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\Rack\\8ball_bg_triangle_rack.png");
            Circles.Add(Rack);


            Circle main = new Circle();
            main.x = 0;
            main.y = this.Height + this.Location.Y - 200;
            main.img = new Bitmap("Anmies&Background\\BG and FG\\FG\\Main\\8ball_main.png");
            Circles.Add(main);







        }
        void Createhero()
        { 
            Hero h = new Hero();

            for (int i = 1; i < 34; i++)
            {
                Bitmap img = new Bitmap("Hero\\Idle\\cuphead_idle_000" + i + ".png");
                img.MakeTransparent();
                h.Idle.Add(img);
            }
            for (int i = 1; i < 17; i++)
            {
                Bitmap img = new Bitmap("Hero\\Run\\Shooting\\Straight\\cuphead_run_shoot_000" + i + ".png");
                img.MakeTransparent();
                h.Run.Add(img);
            }


            h.src = new Rectangle(0, 0, h.Idle[0].Width+50,h.Idle[0].Height);
            h.dst = new Rectangle(100, 500, h.Idle[0].Width , h.Idle[0].Height );

            Lhero.Add(h);
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Hero h = Lhero[0];

            if (e.KeyCode == Keys.Right)
            {
                h.state = 0;
                h.Frame = 0;
            }

            DrawDubb(this.CreateGraphics());
        }

        void DrawHero(Graphics g2)
        {
            Hero h = Lhero[0];

            if (h.state == 0)
                g2.DrawImage(h.Idle[h.Frame], h.dst, h.src, GraphicsUnit.Pixel);

            if (h.state == 1)
                g2.DrawImage(h.Run[h.Frame], h.dst, h.src, GraphicsUnit.Pixel);

        }
        void DrawCircles(Graphics g2)
        {

            Circle main = Circles[8];
            g2.DrawImage(main.img, main.x, main.y, main.img.Width, main.img.Height);

            Circle c1 = Circles[2];
            g2.DrawImage(c1.img, c1.x, c1.y, 150, 150);
            if (fcircle == 0)
                c1.dx = -1;
            else
                c1.dx = 1;

            c1.x += 1 * c1.dx;

            for (int i = 0; i < 2; i++)
            {
                Circle c = Circles[i];
                g2.DrawImage(c.img, c.x, c.y, 100, 100);
                if (fcircle == 0)
                    c.dy = -1;  
                else
                    c.dy = 1;     

                c.y += 1 * c.dy;

            }
            Circle c3 = Circles[4];
            g2.DrawImage(c3.img, c3.x, c3.y, 200, 200);

            Circle c2 = Circles[3];
            g2.DrawImage(c2.img, c2.x, c2.y, 250, 250);

            Circle c4 = Circles[5];
            g2.DrawImage(c4.img, c4.x, c4.y, 150, 150);
            if (fcircle == 0)
                c4.dx = -1;
            else
                c4.dx = 1;

            c4.x += 1 * c4.dx;

           

            Circle rack = Circles[7];
            g2.DrawImage(rack.img, rack.x, rack.y, rack.img.Width, rack.img.Height);
            if (fcircle == 0)
                rack.dy = -1;
            else
                rack.dy = 1;

            rack.y += 1 * rack.dy;



        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Hero h = Lhero[0];

            if (e.KeyCode == Keys.Right)
            {
                h.state = 1;         
                h.Frame++;        
                if (h.Frame >= h.Run.Count)
                    h.Frame = 0;

                h.dst.X += 50;      
            }

            DrawDubb(this.CreateGraphics());
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }
        void handleAnimation()
        {
            Hero h = Lhero[0];

            if (h.state == 0) 
            {
                h.Frame++;
                if (h.Frame >= h.Idle.Count)
                    h.Frame = 0;
            }
        }


        void T_Tick(object sender, EventArgs e)
        {

            ctcircle++;

            if (ctcircle <= 5)
            {
                fcircle = 0;  
            }
            else if (ctcircle <= 10)
            {
                fcircle = 1;  
            }
            else
            {
                ctcircle = 0;  
            }



            handleAnimation();
            DrawDubb(CreateGraphics());
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width,this.ClientSize.Height);
            bg = new Bitmap("background.png");
           
            creatCercles();
            Createhero();
            t.Interval = 100;       
            t.Tick += T_Tick;
            t.Start();
        }

        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.Black);
            g2.DrawImage(bg, 0, 0, this.ClientSize.Width *5, this.ClientSize.Height);
            //   g2.DrawImage(circle, 200, 200, 100, 100);

            Circle cue = Circles[6];
            g2.DrawImage(cue.img, cue.x, cue.y, cue.img.Width, cue.img.Height);
            DrawHero(g2);

            DrawCircles(g2);

        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}

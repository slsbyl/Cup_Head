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
        public List<Bitmap> idle = new List<Bitmap>();
        public List<Bitmap> run = new List<Bitmap>();
        public List<Bitmap> runback = new List<Bitmap>();
        public List<Bitmap> jump = new List<Bitmap>();
        public List<Bitmap> jumpback = new List<Bitmap>();
        public List<Bitmap> shootingup = new List<Bitmap>();
        public List<Bitmap> shootingupback = new List<Bitmap>();
        public List<Bitmap> death = new List<Bitmap>();

        public int frame = 0;
        public int state = 0;
        public int worldx = 0;
        public List<Bitmap> idleback = new List<Bitmap>();
        

        public bool isJumping = false;
        public int jumpspeedy = -25;
        public int velocity_y = 0;
        public int jumpspeedx = 0;
        public bool facingleft = false;



    }
    public class Boss_laser
    {
        public int x, y;
        public int width = 3;
        public int height = 400;
        public int speedy = 30;
        public bool active = true;
    }

    public class Boss_bulletb
    {
        public int x, y;
        public int speedy = 20;
        public List<Bitmap> frames;
        public int frame = 0;
    }

    class Boss8_ball
    {

        public Rectangle dst;
        public int frame2 = 0;
        public int state2 = 0; 
     
        private int framect = 0;  
        private int frameDelay = 1;  

        public int health = 5; 
        public bool isdead = false; 
        public List<Bitmap> bossintro = new List<Bitmap>();
        public List<Bitmap> bossidleleft = new List<Bitmap>();
        public List<Bitmap> bossidleright = new List<Bitmap>();
        public List<Bitmap> bossshootintro = new List<Bitmap>();
        public List<Bitmap> portaleffect = new List<Bitmap>();
        public List<Bitmap> projectile = new List<Bitmap>();
        public List<Bitmap> death = new List<Bitmap>();



        public List<Boss_bulletb> bossbullets = new List<Boss_bulletb>();
        public List<Boss_laser> lasers = new List<Boss_laser>();

        public void Create()
        {
            for (int i = 0; i < 14; i++)
            {
                Bitmap img = new Bitmap("Anmies&Background\\Intro\\8ball_intro_000" + i + ".png");
                img.MakeTransparent();
                bossintro.Add(img);
            }

            for (int i = 1; i < 8; i++)
            {
                Bitmap img = new Bitmap("Anmies&Background\\idle\\Middle\\Trans To Left\\8ball_idle_trans_m_to_l_000" + i + ".png");
                img.MakeTransparent();
                bossidleleft.Add(img);
            }

            for (int i = 1; i < 8; i++)
            {
                Bitmap img = new Bitmap("Anmies&Background\\idle\\Right\\Trans To Middle\\8ball_idle_trans_r_to_m_000" + i + ".png");
                img.MakeTransparent();
                bossidleright.Add(img);
            }

            for (int i = 1; i < 9; i++)
            {
                Bitmap img = new Bitmap("Anmies&Background\\Shoot\\Intro\\8ball_shoot_000" + i + ".png");
                img.MakeTransparent();
                bossshootintro.Add(img);
            }
            for (int i = 1; i < 21; i++) {
                Bitmap img = new Bitmap("Anmies&Background\\Shoot\\Projectile\\8ball_projectile_000" + i + ".png");
                img.MakeTransparent();
                projectile.Add(img);
            }

            for (int i = 1; i < 9; i++)
            {
                Bitmap img = new Bitmap("Anmies&Background\\Death\\8ball_death_000" + i + ".png");
                img.MakeTransparent();
                death.Add(img);
            }


        }


        public void Draw(Graphics g)
        {
            if (isdead)
            {
                if (frame2 < death.Count)
                {
                    g.DrawImage(death[frame2], dst);

                    frame2++;
                    if (frame2 >= death.Count)
                    {
                        bossintro.Clear();
                        bossidleleft.Clear();
                        bossidleright.Clear();
                        bossshootintro.Clear();
                        bossbullets.Clear();
                        lasers.Clear();
                        death.Clear(); 
                    }
                }

                return; 
            }

            if (bossintro.Count == 0 || bossidleleft.Count == 0 || bossidleright.Count == 0 || bossshootintro.Count == 0)
                return;

            if (frame2 < 0) frame2 = 0;

            if (state2 == 0)
            {
                if (frame2 >= bossintro.Count)
                    frame2 = bossintro.Count - 1;

                g.DrawImage(bossintro[frame2], dst);
            }

            else if (state2 == 1)
            {
                if (frame2 >= bossidleleft.Count)
                    frame2 = bossidleleft.Count - 1;

                g.DrawImage(bossidleleft[frame2], dst);
            }

            else if (state2 == 2)
            {
                if (frame2 >= bossidleright.Count)
                    frame2 = bossidleright.Count - 1;

                g.DrawImage(bossidleright[frame2], dst);
            }

           else if (state2 == 3) 
            {
                if (frame2 >= bossshootintro.Count)
                    frame2 = bossshootintro.Count - 1;

                g.DrawImage(bossshootintro[frame2], dst);
                if (frame2 == 0)
                {
                    Boss_bulletb b = new Boss_bulletb();
                    b.frames = projectile;
                    b.frame = 0;
                    b.x = dst.X + dst.Width / 2 - 25;
                    b.y = dst.Y + dst.Height;
                    bossbullets.Add(b);
                }
                for (int i = 0; i < bossbullets.Count; i++)
                {
                    Boss_bulletb b = bossbullets[i];

                    if (b.frames != null && b.frames.Count > 0 && b.frame < b.frames.Count)
                    {
                        g.DrawImage(b.frames[b.frame], b.x, b.y, 150, 150);
                    }
                }


            }

            if (state2 == 3) 
            {
                for (int i = 0; i < lasers.Count; i++)
                {
                    Boss_laser l = lasers[i];
                    g.FillRectangle(Brushes.Red, l.x, l.y, l.width, l.height);
                }

            }





        }





        public void update()
        {
            framect++;
            if (framect >= frameDelay)
            {
                frame2++;            
                framect = 0;    
            }
            if (state2 == 0 && frame2 >= bossintro.Count)
            {
                frame2 = 0;
                state2 = 1; 
            }

            else if (state2 == 1 && frame2 >= bossidleleft.Count)
            {
                frame2 = 0;
                state2 = 2; 
            }

            else if (state2 == 2 && frame2 >= bossidleright.Count)
            {
                frame2 = 0;
                state2 = 3; 
            }

            else if (state2 == 3 && frame2 >= bossshootintro.Count)
            {
                frame2 = 0;
                state2 = 1; 
            }
            else if (state2 == 3 && frame2 == 0) 
            {
                Boss_bulletb b = new Boss_bulletb();
                b.frames = projectile;
                b.frame = 0;
                b.x = dst.X + dst.Width / 2 - 25; 
                b.y = dst.Y + dst.Height;

                bossbullets.Add(b);
            }
            for (int i = bossbullets.Count - 1; i >= 0; i--)
            {
                Boss_bulletb b = bossbullets[i];
                b.y += b.speedy;
                if (b.frame < b.frames.Count - 1)
                    b.frame++;
                if (b.y > 3000)
                    bossbullets.RemoveAt(i);
            }
            if (state2 == 3 && frame2 == 3) 
            {
                Boss_laser laser = new Boss_laser();
                laser.x = dst.X + dst.Width -80; 
                laser.y = dst.Y +200; 
                lasers.Add(laser);

                Boss_laser laser2 = new Boss_laser();
                laser2.x = dst.X + dst.Width -260; 
                laser2.y = dst.Y + 200;
               
                lasers.Add(laser2);
            }
            for (int i = lasers.Count - 1; i >= 0; i--)
            {
                Boss_laser l = lasers[i];
                if (l.active)
                {
                    l.y += l.speedy;
                    if (l.y > 2000)
                        l.active = false;
                }
                else
                {
                    lasers.RemoveAt(i);
                }
            }
        }
        public void checkhitting(List<Bullet> heroBullets)
        {
            if (isdead) return; 

            Rectangle bossrect = dst;
            for (int i = heroBullets.Count - 1; i >= 0; i--)
            {
                Bullet b = heroBullets[i];
                Rectangle bullet_rect = new Rectangle(b.x, b.y, 50, 50);

              
                int bLeft = bullet_rect.X;
                int bright = bullet_rect.X + bullet_rect.Width;
                int btop = bullet_rect.Y;
                int b_bottom = bullet_rect.Y + bullet_rect.Height;

                int bossleft = bossrect.X;
                int bossright = bossrect.X + bossrect.Width;
                int bosstop = bossrect.Y;
                int bossbottom = bossrect.Y + bossrect.Height;



                bool hit = bright > bossleft && bLeft < bossright && b_bottom > bosstop && btop < bossbottom;

                if (hit)
                {
                    health = health - 1;
                    heroBullets.RemoveAt(i);
                }

                if (health <= 0)
                {
                    isdead = true;
                    frame2 = 0;
                }


            }
        }


    }

    class Bullet
    {
        public int x, y;
        public int speed;
        public bool dirLeft;

        public List<Bitmap> frames;
        public int frame = 0;
    }


    class Circle
    {
        public int x;
        public int y;
        public Bitmap img;
        public int dy = 1;
        public int dx = 1;

    }
   

    public partial class Form1 : Form
    {

        Boss8_ball boss = new Boss8_ball();
    
        List<Bitmap> herohealthframes = new List<Bitmap>();
        int currhealthframe = 0;  
        int maxhealth = 5;      
        bool lastchargeup = false;
        List<Bitmap> Bulletright = new List<Bitmap>();
        List<Bitmap> Bulletleft = new List<Bitmap>();
        List<Bitmap> Bulletup = new List<Bitmap>();
        List<Bullet> bulletss = new List<Bullet>();
        bool uppress = false;

        bool rightpress = false;
        bool leftpress = false;
        int cam_x = 0;
        Bitmap bg;
      

        Timer t = new Timer();
        Bitmap off;
        List<Hero> Lhero = new List<Hero>();
        List<Circle> Circles = new List<Circle>();
        bool downpress = false;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;

        }

        void Drawhealth(Graphics g)
        {
            int x = 20;
            int y = 20; 
            g.DrawImage(herohealthframes[currhealthframe], x, y, herohealthframes[0].Width-80, herohealthframes[0].Height-80);
        }

        void Updatecam()
        {
            Hero h = Lhero[0];

            cam_x = h.worldx - this.ClientSize.Width / 2;

            if (cam_x < 0) cam_x = 0;

            if (cam_x > bg.Width - this.ClientSize.Width)
                cam_x = bg.Width - this.ClientSize.Width;
        }
        void creatCercles()
        {
            Circle c = new Circle();
            c.x = 200;
            c.y = 200;
            c.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\Light Orange\\8ball_bg_ball_light-orange-solid.png");
            Circles.Add(c);

            Circle c1 = new Circle();
            c1.x = this.Width - 200;
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
            c4.y = this.Height - 200;
            c4.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\Red\\8ball_bg_ball_red-solid.png");
            Circles.Add(c4);

            Circle c6 = new Circle();
            c6.x = 100;
            c6.y = this.Height / 4 + 100;
            c6.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\8-Ball\\White and Blue Stripes\\8ball_bg_ball_white-blue-stripe.png");
            Circles.Add(c6);

            Circle cue = new Circle();
            cue.x = 150;
            cue.y = this.Height - 300;
            cue.img = new Bitmap("Anmies&Background\\BG and FG\\BG\\Cue\\8ball_bg_cue_back.png");
            Circles.Add(cue);

            Circle Rack = new Circle();
            Rack.x = this.Width / 2 - 300;
            Rack.y = this.Height / 2 - 350;
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

            for (int i = 1; i < 7; i++)
            {
                Bitmap img = new Bitmap("Hero\\idle\\cuphead_idle_000" + i + ".png");
                img.MakeTransparent();
                h.idle.Add(img);
            }
            for (int i = 1; i < 7; i++)
            {
                Bitmap img = new Bitmap("Hero\\Idle_back\\cuphead_idle_000" + i + ".png");
                img.MakeTransparent();
                h.idleback.Add(img);
            }


            for (int i = 1; i < 17; i++)
            {
                Bitmap img = new Bitmap("Hero\\run\\Shooting\\Straight\\cuphead_run_shoot_000" + i + ".png");
                img.MakeTransparent();
                h.run.Add(img);
            }

            for (int i = 1; i < 17; i++)
            {
                Bitmap img = new Bitmap("Hero\\run\\Shooting\\Straight_back\\cuphead_run_shoot_000" + i + ".png");
                img.MakeTransparent();
                h.runback.Add(img);
            }


            for (int i = 1; i < 9; i++)
            {
                Bitmap img = new Bitmap("Hero\\Jump\\Cuphead\\cuphead_jump_000" + i + ".png");
                img.MakeTransparent();
                h.jump.Add(img);
            }

            for (int i = 1; i < 9; i++)
            {
                Bitmap img = new Bitmap("Hero\\Jump\\Cuphead_back\\cuphead_jump_000" + i + ".png");
                img.MakeTransparent();
                h.jumpback.Add(img);
            }
            for (int i = 1; i <7; i++)
            {
                Bitmap img = new Bitmap("Hero\\Shoot\\Up\\cuphead_shoot_up_000" + i + ".png");
                img.MakeTransparent();
                h.shootingup.Add(img);
            }
            for (int i = 1; i < 7; i++)
            {
                Bitmap img = new Bitmap("Hero\\Shoot\\Up_back\\cuphead_shoot_up_000" + i + ".png");
                img.MakeTransparent();
                h.shootingupback.Add(img);
            }

            for (int i = 1; i < 7; i++)
            {
                Bitmap img = new Bitmap("Hero\\Death\\cuphead_death_body_000" + i + ".png");
                img.MakeTransparent();
                h.death.Add(img);
            }



            h.src = new Rectangle(0, 0, h.idle[0].Width + 50, h.idle[0].Height);
            h.dst = new Rectangle(100, 500, h.idle[0].Width, h.idle[0].Height);

            Lhero.Add(h);
        }

       

        void Createbullet()
        {
            for (int i = 1; i < 13; i++)
            {
                Bitmap img = new Bitmap("Charge\\Charge\\Charge Loop large\\weapon_charge_large_loop_000" + i + ".png");
                img.MakeTransparent();
                Bulletright.Add(img);
            }

            for (int i = 1; i < 13; i++)
            {
                Bitmap img = new Bitmap("Charge\\Charge\\Charge Loop large_back\\weapon_charge_large_loop_000" + i + ".png");
                img.MakeTransparent();
                Bulletleft.Add(img);
            }
            for (int i = 1; i < 13; i++)
            {
                Bitmap img = new Bitmap("Charge\\Charge\\Charge Loop large_up\\weapon_charge_large_loop_000" + i + ".png");
                img.MakeTransparent();
                Bulletup.Add(img);
            }
        }
      
       



        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Hero h = Lhero[0];

            if (e.KeyCode == Keys.Right)
            {
                rightpress = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                leftpress = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                uppress = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                uppress = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                downpress = true;
            }
            DrawDubb(this.CreateGraphics());
        }

        void DrawHero(Graphics g)
        {
            Hero h = Lhero[0];
          
            if (currhealthframe == herohealthframes.Count - 1) 
            {
                g.DrawImage(h.death[h.frame], h.dst, h.src, GraphicsUnit.Pixel);
                h.frame++;
                if (h.frame >= h.death.Count) h.frame = h.death.Count - 1;
                return; 
            }
            h.dst.X = h.worldx - cam_x;

            if (h.state == 0)
                g.DrawImage(h.idle[h.frame], h.dst, h.src, GraphicsUnit.Pixel);

            else if (h.state == 1)
                g.DrawImage(h.run[h.frame], h.dst, h.src, GraphicsUnit.Pixel);

            else if (h.state == 2)
                g.DrawImage(h.idleback[h.frame], h.dst, h.src, GraphicsUnit.Pixel);

            else if (h.state == 3)
                g.DrawImage(h.runback[h.frame], h.dst, h.src, GraphicsUnit.Pixel);
            else if (h.state == 4) 
                g.DrawImage(h.jump[h.frame], h.dst, h.src, GraphicsUnit.Pixel);
            else if (h.state == 5)
                g.DrawImage(h.jumpback[h.frame], h.dst, h.src, GraphicsUnit.Pixel);
            else if (h.state == 6)
                g.DrawImage(h.shootingupback[h.frame], h.dst, h.src, GraphicsUnit.Pixel);

            else if (h.state == 7)
                g.DrawImage(h.shootingup[h.frame], h.dst, h.src, GraphicsUnit.Pixel);


           


        }

        void DrawCircles(Graphics g2)
        {

            Circle main = Circles[8];
            g2.DrawImage(main.img, main.x, main.y, main.img.Width, main.img.Height);
            Circle c3 = Circles[4];
            g2.DrawImage(c3.img, c3.x, c3.y, 200, 200);

            Circle c2 = Circles[3];
            g2.DrawImage(c2.img, c2.x, c2.y, 250, 250);
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Hero h = Lhero[0];

            if (e.KeyCode == Keys.Right)

                rightpress = true;

            if (e.KeyCode == Keys.Left)
                leftpress = true;

            if (e.KeyCode == Keys.Space && !h.isJumping)
            {
                h.isJumping = true;

                h.velocity_y = -25;
                if (h.facingleft)
                {
                    h.jumpspeedx = -10;

                    h.state = 5; 
                }
                else
                {
                    h.jumpspeedx = 10;
                    h.state = 4;
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                uppress = true; lastchargeup = true;

                if (h.facingleft)
                {
                    h.state = 6;
                }
                else
                {
                    h.state = 7;
                }

                h.frame = 0;
            }

            if (e.KeyCode == Keys.X)
            {
                Firebullet();
            }
        }






        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
          

        }

        void Firebullet()
        {
            Hero h = Lhero[0];
            Bullet b = new Bullet();

            if (uppress)
            {
                b.x = h.worldx + h.dst.Width / 2-20;
                b.y = h.dst.Y - 50;
                b.speed = -50;
                b.frames = Bulletup;
            }
            else if (h.facingleft)
            {
                b.x = h.worldx - 100;
                b.y = h.dst.Y + h.dst.Height / 2 - 35;
                b.speed = -50;
                b.frames = Bulletleft;
            }
            else
            {
                b.x = h.worldx + 100;
                b.y = h.dst.Y + h.dst.Height / 2-35 ;
                b.speed = 50;
                b.frames = Bulletright;
            }

            bulletss.Add(b);
        }

        void T_Tick(object sender, EventArgs e)
        {
            Hero h = Lhero[0]; int speed = 20;

            if (rightpress)
            {
                h.facingleft = false;
                h.state = 1;
                h.worldx += speed;
                h.frame++;
                if (h.frame >= h.run.Count) h.frame = 0;
                for (int i = 0; i < bulletss.Count; i++)
                {
                    Bullet b = bulletss[i];
                    if (b.frame == 0) 
                    {
                        b.x = h.worldx + h.dst.Width;  
                    }
                }

            }
            else if (leftpress)
            {
                h.facingleft = true;
                h.state = 3;
                h.worldx -= speed;
                h.frame++;
                if (h.frame >= h.runback.Count) h.frame = 0;
                for (int i = 0; i < bulletss.Count; i++)
                {
                    Bullet b = bulletss[i];
                    if (b.frame == 0) 
                    {
                        b.x = h.worldx;  
                    }
                }

            }

            else if (!h.isJumping && !uppress)
            {
                if (h.facingleft)
                {
                    h.state = 2;
                    h.frame++;
                    if (h.frame >= h.idleback.Count)
                        h.frame = 0;
                }
                else
                {
                    h.state = 0;
                    h.frame++;
                    if (h.frame >= h.idle.Count)
                        h.frame = 0;
                }
            }
            if (uppress)
            {
                h.frame++;
                if (h.facingleft)
                {
                    if (h.frame >= h.shootingupback.Count)
                        h.frame = 0;
                }
                else
                {
                    if (h.frame >= h.shootingup.Count)
                        h.frame = 0;
                }
            }

            if (h.isJumping)
            {
                h.dst.Y += h.velocity_y;
                h.velocity_y += 2;
                h.worldx += h.jumpspeedx;
                if (h.dst.Y >= 500)
                {
                    h.dst.Y = 500;
                    h.isJumping = false;
                    h.jumpspeedx = 0;
                    if (h.facingleft)
                    {
                        h.state = 2;
                    }
                    else
                    {
                        h.state = 0;
                    }

                    h.frame = 0;
                }
                h.frame++;
                if (h.state == 4 && h.frame >= h.jump.Count) h.frame = 0;
                if (h.state == 5 && h.frame >= h.jumpback.Count) h.frame = 0;
            }
            for (int i = bulletss.Count - 1; i >= 0; i--)
            {
                Bullet b = bulletss[i];
                if (b.frame > 0)
                {
                    if (b.frames == Bulletup)
                    {
                        b.y += b.speed;  
                    }
                    else
                    {
                        b.x += b.speed;
                    }

                }

                b.frame++;
                if (b.frame >= b.frames.Count || b.x < cam_x - 100 || b.x > cam_x + this.ClientSize.Width + 100 || b.y < 0)

                    bulletss.RemoveAt(i);
            }
          

            h = Lhero[0];

            boss.checkhitting(bulletss);

            boss.update();

            for (int i = boss.bossbullets.Count - 1; i >= 0; i--)
            {
                Boss_bulletb b = boss.bossbullets[i];


                h = Lhero[0];
                Rectangle herorect = h.dst;

                Rectangle bulletrect = new Rectangle(b.x, b.y, 50, 50);
                int b_left = bulletrect.X;
                int b_right = bulletrect.X + bulletrect.Width;
                int b_top = bulletrect.Y;
                int b_bottom = bulletrect.Y + bulletrect.Height;


                int h_left = herorect.X;
                int h_right = herorect.X + herorect.Width;
                int h_top = herorect.Y;
                int h_bottom = herorect.Y + herorect.Height;

                if (b_right > h_left && b_left < h_right && b_bottom > h_top && b_top < h_bottom)
                {
                    currhealthframe++;
                    if (currhealthframe >= herohealthframes.Count)
                    {
                        currhealthframe = herohealthframes.Count - 1;
                        h.state = 4;
                    }
                    boss.bossbullets.RemoveAt(i);
                }

            }
            for (int i = boss.lasers.Count - 1; i >= 0; i--)
            {
                var l = boss.lasers[i];
                 h = Lhero[0];
                Rectangle herorect = h.dst;
                Rectangle laserrect = new Rectangle(l.x, l.y, l.width, l.height);

                int l_left = laserrect.X;
                int l_right = laserrect.X + laserrect.Width;
                int l_top = laserrect.Y;
                int l_bottom = laserrect.Y + laserrect.Height;
///////////////////////////
                int h_left = herorect.X;
                int h_right = herorect.X + herorect.Width;
                int h_top = herorect.Y;
                int h_bottom = herorect.Y + herorect.Height;

                if (l_right > h_left && l_left < h_right && l_bottom > h_top && l_top < h_bottom)
                {
                    currhealthframe++;
                    if (currhealthframe >= herohealthframes.Count)
                    {
                        currhealthframe = herohealthframes.Count - 1;
                        h.state = 4;
                    }
                    l.active = false;
                }

            }
            Updatecam();
            DrawDubb(CreateGraphics());
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            bg = new Bitmap("background.png");
            Createbullet();
            creatCercles();
            Createhero();
            boss.Create();
            for (int i = 1; i <= 6; i++)
            {
                Bitmap img = new Bitmap("health\\" + i + ".png");  
                img.MakeTransparent();
                herohealthframes.Add(img);
            }


            boss.dst = new Rectangle(this.Width/2-200, -15, 350, 350);
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Start();
        }
        void Drawbullets(Graphics g)
        {

            for (int i = 0; i < bulletss.Count; i++)
            {
                Bullet b = bulletss[i];

                int drawX = b.x - cam_x-30;
                g.DrawImage(b.frames[b.frame], drawX, b.y, 80, 80);
            }

        }
      
        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.Black);
            g2.DrawImage(bg,
                new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height),
                new Rectangle(cam_x, 0, this.ClientSize.Width, this.ClientSize.Height),
                GraphicsUnit.Pixel);
            Circle cue = Circles[6];
            g2.DrawImage(cue.img, cue.x, cue.y, cue.img.Width, cue.img.Height);
            Drawhealth(g2);
            DrawHero(g2);
            Drawbullets(g2);
            DrawCircles(g2);
            boss.Draw(g2);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}





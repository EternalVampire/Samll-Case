using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft_War
{
    class BulletFather:GameObject
    {
        private Image imgBullet;//存储子弹图片

        public int Power
        {
            get;
            set;
        }//存储子弹威力

        public BulletFather(PlaneFather pf, Image img, int speed,int power)
            : base(pf.X + pf.Width/2-25, pf.Y + pf.Height/2-60, img.Width, img.Height, 0, speed, pf.Dir)
        {
            this.imgBullet = img;
            this.Power = power;
        }

        //重写GameObject的抽象成员
        public override void Draw(Graphics g)
        {
            this.Move();
            g.DrawImage(imgBullet, this.X, this.Y);
        }

        public override void Move()
        {
            switch (this.Dir)
            {
                case Direction.Up:
                    this.Y -= this.Speed;
                    break;
                case Direction .Down :
                    this.Y += this.Speed;
                    break;
            }
            //子弹发出后，控制子弹的坐标
            if (this.Y <= 0)
            {
                this.Y = -300;
                //在游戏中移除子弹对象
            }
            if (this.Y >= 700)
            {
                this.Y = 1000;
                //在游戏中移除子弹对象
            }

        }
    }
}

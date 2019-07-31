using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Aircraft_War
{
    class PlaneHero:PlaneFather
    {
        //导入玩家飞机的图片
        private static Image imgPlane = Resources.me1;

        //调用父类构造函数
        public PlaneHero(int x, int y, int speed, int life, Direction dir)
            :base(x,y,imgPlane,speed,life,dir)
        {

        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(imgPlane, this.X, this.Y, this.Width / 2, this.Height / 2);
        }

        public void MonseMove(MouseEventArgs e)
        {
            this.X = e.X;
            this.Y = e.Y;
        }

        public void Fire()
        {
            SingleObject.GetSingle().AddGameObject(new HeroBullet(this, 10, 1));    
        }

        public override void IsOver()
        {
            SingleObject.GetSingle().AddGameObject(new HeroBoom(this.X, this.Y));
        }
    }
}

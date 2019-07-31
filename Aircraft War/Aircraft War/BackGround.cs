using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;
using System.Windows.Forms;

namespace Aircraft_War
{
     class BackGround:GameObject
    {
        //导入背景图片
        private static Image imgBG = Resources.background;

        //调用父类的构造函数
        public BackGround(int x, int y, int speed)
            : base(x, y, imgBG.Width, imgBG.Height, 0, speed, Direction.Down)
        {

        }

        public override void Draw(Graphics g)
        {
            this.Y += this.Speed;
            if (this.Y == 0)
            {
                this.Y = -700;
            }

            g.DrawImage(imgBG, this.X, this.Y);
        }
    }
}

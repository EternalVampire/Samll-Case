using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;
using System.Drawing;

namespace Aircraft_War
{
    class EnemyBoom:BoomFather
    {
        //最小飞机爆炸
        private Image[] imgs1 = { Resources.enemy1_down1, Resources.enemy1_down2, Resources.enemy1_down3, Resources.enemy1_down4 };

        //中等飞机爆炸
        private Image[] imgs2 = { Resources.enemy2_down1, Resources.enemy2_down2, Resources.enemy2_down3, Resources.enemy2_down4 };

        //最大飞机爆炸
        private Image[] imgs3 = { Resources.enemy3_down1, Resources.enemy3_down2, Resources.enemy3_down3, Resources.enemy3_down4, Resources.enemy3_down5, Resources.enemy3_down6 };

        //声明标示来标记当前属于哪架飞机
        public int Type
        {
            get;
            set;
        }

        //根据地方类型播放对应的爆炸图片
        public EnemyBoom(int x, int y,int type)
            : base(x, y)
        {
            this.Type = type;
        }

        public override void Draw(Graphics g)
        {
            switch (this.Type)
            {
                case 0:
                    for (int i = 0; i < imgs1.Length; i++)
                    {
                        g.DrawImage(imgs1[i], this.X, this.Y);
                    }
                    break;
                case 1:
                    for (int i = 0; i < imgs2.Length; i++)
                    {
                        g.DrawImage(imgs2[i], this.X, this.Y);
                    }
                    break;
                case 2:
                    for (int i = 0; i < imgs3.Length; i++)
                    {
                        g.DrawImage(imgs3[i], this.X, this.Y);
                    }
                    break;
            }
            //爆炸图片播放完毕后，进行销毁
            SingleObject.GetSingle().RemoveGameObject(this);
        }
    }
}

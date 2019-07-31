using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;
using System.Drawing;

namespace Aircraft_War
{
    class HeroBoom:BoomFather
    {
        private Image[] imgs = { Resources.me_destroy_1, Resources.me_destroy_2, Resources.me_destroy_3, Resources.me_destroy_4 };

        public HeroBoom(int x, int y)
            : base(x, y)
        {

        }

        public override void Draw(Graphics g)
        {
            for (int i = 0; i < imgs.Length; i++)
            {
                g.DrawImage(imgs[i], this.X, this.Y);
            }

            //绘制完成后，将爆照的图片移除
            SingleObject.GetSingle().RemoveGameObject(this);
        }
    }
}

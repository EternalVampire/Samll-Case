using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Aircraft_War
{
    abstract class PlaneFather:GameObject
    {
        private Image imgPlane;//声明一个字段存储飞机的图片。  

        public PlaneFather(int x,int y, Image img,int speed , int life, Direction dir )
            :base(x,y,img.Width ,img.Height,life,speed ,dir)
        {
            this.imgPlane = img;
        }

        //判断是否死亡的抽象函数
        public abstract void IsOver();
    }
}

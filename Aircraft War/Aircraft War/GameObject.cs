using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft_War
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    abstract class GameObject
    {
        #region 存储游戏对象共有的横纵坐标 宽度 高度 生命值 速度 方向
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Life
        {
            get;
            set;
        }

        public int Speed
        {   
            get;
            set;
        }

        public Direction  Dir
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="heigth"></param>
        /// <param name="life"></param>
        /// <param name="speed"></param>
        /// <param name="dir"></param>
        public GameObject(int x, int y, int width, int heigth, int life, int speed, Direction dir)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = heigth;
            this.Life = life;
            this.Speed = speed;
            this.Dir = dir;
        }

        /// <summary>
        /// 父类中提供绘制对象的抽象函数
        /// </summary>
        /// <param name="g"></param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// 提供一个用于碰撞检测的函数，返回当前游戏对面的矩形
        /// </summary>
        /// <returns></returns>
        public Rectangle Getrectangle()
        {
            return new Rectangle(this.X, this.Y, this.Width, this.Height);
        }

        //横纵坐标的重载函数
        public GameObject(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 移动的虚方法，如果每个类中又不一样的地方，需重写
        /// </summary>
        public virtual void Move()
        {
            //根据游戏对象的方向进行移动
            switch (this.Dir)
            {
                case  Direction.Up:
                    this.Y -= this.Speed;
                    break;
                case Direction.Down :
                    this.Y += this.Speed;
                    break;
                case Direction.Left:
                    this.X -= this.Speed;
                    break;
                case Direction.Right:
                    this.X += this.Speed;
                    break;
            }

            //移动完成后，判断一下游戏对象是否超出窗体
            if(this .X <= 0)
            {
                this.X = 0;
            }
            if (this.X >=  378)
            {
                this.X = 378;
            }
            if (this.Y <= 0)
            {
                this.Y = 0;
            }
            if (this.Y >= 700)
            {
                this.Y = 700;
            }

        }
    }
}

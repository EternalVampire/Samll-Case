using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aircraft_War.Properties;
using System.Drawing;

namespace Aircraft_War
{
    class PlaneEnemy:PlaneFather
    {
        public static Image Enemy1 = Resources.enemy1;//最小的敌机。
        public static Image Enemy2 = Resources.enemy2;//中等的敌机。
        public static Image Enemy3 = Resources.enemy3_n1;//最大的敌机。

        static Random r = new Random();

        public PlaneEnemy(int x, int y, int type)
            : base(x, y, GetImage(type), GetSpeed(type), GetLife(type), Direction.Down)
        {
            this.EnemyType = type;
        }

        //声明标示来标记当前属于哪架飞机。
        public int EnemyType
        {
            get;
            set;
        }

        //根据飞机的类型，返回对应的图片。
        public static Image GetImage(int type)
        {
            switch (type)
            {
                case 0 :
                    return Enemy1;
                case 1:
                    return Enemy2;
                case 2:
                    return Enemy3;
            }
            return null;
        }

        //根据飞机的类型，返回对应的生命值。
        public static int GetLife(int type)
        {
            switch (type)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 4;
            }
            return 0;
        }

        //根据飞机的类型，返回对应的速度。
        public static int GetSpeed(int type)
        {
            switch (type)
            {
                case 0:
                    return 6;
                case 1:
                    return 5;
                case 2 :
                    return 7;
            }
            return 0;
        }

        //将自己绘制到From窗体上。
        public override void Draw(Graphics g)
        {
            this.Move();

            switch (EnemyType)
            {
                case 0:
                    g.DrawImage(Enemy1, this.X, this.Y);
                    break;
                case 1:
                    g.DrawImage(Enemy2, this.X, this.Y);
                    break;
                case 2:
                    g.DrawImage(Enemy3, this.X, this.Y);
                    break;
            }
            
        }

        public override void Move()
        {
            switch (this.Dir)
            {
                case Direction.Up:
                    this.Y -= this.Speed;
                    break;
                case Direction.Down:
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
            if (this.X <= 0)
            {
                this.X = 0;
            }
            if (this.X >= 400)
            {
                this.X = 400;
            }
            if (this.Y <= 0)
            {
                this.Y = 0;
            }
            if (this.Y >= 700)
            {
                this.Y = 1400;

                //敌方飞机超出窗体后移除游戏对象
                SingleObject.GetSingle().RemoveGameObject(this);
            }

            if (this.EnemyType == 0 && this.Y >= 200)
            {
                if (this.X >= 0 && this.X <= 200)
                {
                    this.X += r.Next(0, 50);
                }
                else
                {
                    this.X -= r.Next(0,100);
                }
            }
            else
            {
                this.Speed += 1;
            }

            //百分之五的概率发射子弹
            if (r.Next(0, 100) > 98)
            {
                Fire(); 
            }
        }

        public override void IsOver()
        {
            //敌方飞机死亡后，将从游戏中移除对象
            SingleObject.GetSingle().RemoveGameObject(this);

            //播放敌人爆炸图片
            SingleObject.GetSingle().AddGameObject(new EnemyBoom(this.X,this.Y,this.EnemyType));

            switch (this.EnemyType)
            {
                case 0:
                    SingleObject.GetSingle().Score   += 50;
                    break;
                case 1:
                    SingleObject.GetSingle().Score += 200;
                    break;
                case 2:
                    SingleObject.GetSingle().Score += 400;
                    break;
            }
        }

        public void Fire()
        {
            SingleObject.GetSingle().AddGameObject(new EnemyBullet(this, 20, 1));
        }
    }

}

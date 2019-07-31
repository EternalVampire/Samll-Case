using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft_War.Properties
{
    class SingleObject
    {
        //构造函数私有化
        private SingleObject()
        {

        }

        //声明全局唯一的对象
        private static SingleObject _single = null;

        //提供一个静态函数用于返回一个唯一的对象
        public static SingleObject GetSingle()
        {
            if (_single == null)
            {
                _single = new SingleObject();
            }

            return _single; 
        }

        //存储的背景在游戏中的唯一对象
        public BackGround BG
        {
            get;
            set;
        }

        //存储玩家飞机在游戏中的唯一对象
        public PlaneHero PH
        {
            get;
            set;
        }

        //记录玩家分数
        public int Score
        {
            get;
            set;
        }

        //存储玩家子弹的集合对象
        List<HeroBullet> listHeroBullet = new List<HeroBullet>();

        //声明一个集合存储敌方飞机爆炸的对象
        List<EnemyBoom> listEnemyBoom = new List<EnemyBoom>();

        //声明一个集合存储敌方的子弹
        List<EnemyBullet> listEnemyBullet = new List<EnemyBullet>();

        //声明一个集合存储玩家爆炸的对象
        List<HeroBoom> listHeroBoom = new List<HeroBoom>();

        //储存敌方飞机的集合对象
        public List<PlaneEnemy> listPlaneEnemy = new List<PlaneEnemy>();

        //将游戏对象添加到窗体中
        public void AddGameObject(GameObject go)
        {
            if(go is BackGround)
            {
                this.BG = go as BackGround;
            } 
            else if (go is PlaneHero)
            {
                this.PH = go as PlaneHero;
            }
            else if (go is HeroBullet)
            {
                listHeroBullet.Add(go as HeroBullet);
            }
            else if(go is PlaneEnemy) 
            {
                listPlaneEnemy.Add(go as PlaneEnemy);
            }
            else if (go is EnemyBoom)
            {
                listEnemyBoom.Add(go as EnemyBoom);
            }
            else if (go is EnemyBullet)
            {
                listEnemyBullet.Add(go as EnemyBullet);
            }
            else if(go is HeroBoom)
            {
                listHeroBoom.Add(go as HeroBoom);
            }

        }

        //将游戏对象从游戏中移除
        public void RemoveGameObject(GameObject go)
        {
            if (go is PlaneEnemy)
            {
                listPlaneEnemy.Remove(go as PlaneEnemy);
            }
            else if (go is HeroBullet)
            {
                listHeroBullet.Remove(go as HeroBullet);
            }
            else if (go is EnemyBoom)
            {
                listEnemyBoom.Remove(go as EnemyBoom);
            }
            else if (go is EnemyBullet)
            {
                listEnemyBullet.Remove(go as EnemyBullet);
            }
            else if (go is HeroBoom)
            {
                listHeroBoom.Remove(go as HeroBoom);
            }
        }

        public void Draw(Graphics g)
        {
            this.BG.Draw(g);//向窗体绘制背景
            this.PH.Draw(g);//向窗体绘制玩家飞机
            for (int i = 0; i < listHeroBullet.Count; i++)
            {
                listHeroBullet[i].Draw(g);
            }

            for (int i = 0; i < listPlaneEnemy.Count; i++)
            {
                listPlaneEnemy[i].Draw(g);
            }
            for (int i = 0; i < listEnemyBoom.Count; i++)
            {
                listEnemyBoom[i].Draw(g);
            }
            for (int i = 0; i < listEnemyBullet.Count; i++)
            {
                listEnemyBullet[i].Draw(g);
            }
            for(int i = 0; i < listHeroBoom.Count; i++)
            {
                listHeroBoom[i].Draw(g);
            }
        }

        //碰撞检测函数
        public void collision()
        {
            #region 判断玩家的子弹是否与敌人飞机进行碰撞
            for (int i = 0; i < listHeroBullet.Count; i++)
            {
                for (int j = 0; j < listPlaneEnemy.Count; j++)
                {
                    if (listHeroBullet[i].Getrectangle().IntersectsWith(listPlaneEnemy[j].Getrectangle()))
                    {
                        //如果条件成立，则说明发生了碰撞
                        listPlaneEnemy[j].Life -= listHeroBullet[i].Power;

                        //判断敌人是否死亡
                        listPlaneEnemy[j].IsOver();

                        //将玩家子弹销毁
                        listHeroBullet.Remove(listHeroBullet[i]);
                        break;
                    }
                }
            }
            #endregion

            #region 判断敌人子弹是否玩家进行碰撞
            for (int i = 0; i < listEnemyBullet.Count; i++)
            {
                if (listEnemyBullet[i].Getrectangle().IntersectsWith(this.PH.Getrectangle()))
                {
                    //玩家爆炸，但不死亡
                    this.PH.IsOver(); 
                        break;
                }
            }
            #endregion

            #region 判断敌人飞机是否与玩家飞机进行碰撞
            for (int i = 0; i < listPlaneEnemy.Count; i++)
            {
                if (listPlaneEnemy[i].Getrectangle().IntersectsWith(this.PH.Getrectangle()))
                {
                    listPlaneEnemy[i].Life = 0;
                    listPlaneEnemy[i].IsOver();
                    break;
                }
            }
            #endregion
        }
    }
}

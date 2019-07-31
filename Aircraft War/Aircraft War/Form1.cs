using Aircraft_War.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aircraft_War
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitialGame();
        }       
       
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //当窗体被重新绘制的时候，会执行当前事件
            SingleObject.GetSingle().Draw(e.Graphics);

            //创建玩家分数对象
            string socre = SingleObject.GetSingle().Score.ToString();

            //绘制玩家分数
            e.Graphics.DrawString(socre, new Font("宋体", 20, FontStyle.Bold), Brushes.Black, new Point(0, 0));
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            //将图像绘制于缓冲区，减少图片闪烁问题
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer|ControlStyles.ResizeRedraw|ControlStyles.AllPaintingInWmPaint, true);
                
        }

        //定义一个随机数
        static Random r = new Random();

        /// <summary>
        /// 初始化游戏对象
        /// </summary>
        public void InitialGame()
        {
            //初始化背景
            SingleObject.GetSingle().AddGameObject(new BackGround(0, -700, 5));

            //初始化玩家飞机
            SingleObject.GetSingle().AddGameObject(new PlaneHero(100, 100, 5, 3, Direction.Up));

            InitiaPlaneEnemy();
        }            

        //初始化敌方飞机
        public void InitiaPlaneEnemy()
        {
            for (int i = 0; i <= 5; i++)
            {
                SingleObject.GetSingle().AddGameObject(new PlaneEnemy(r.Next(0, this.Width), -400, r.Next(0, 2)));
            }

            if (r.Next(0, 100) > 80)
            {
                SingleObject.GetSingle().AddGameObject(new PlaneEnemy(r.Next(0, this.Width), -400, 2));
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            SingleObject.GetSingle().PH.MonseMove(e);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            SingleObject.GetSingle().PH.Fire();
        }

        private void timerBG_Tick(object sender, EventArgs e)
        {
            this.Invalidate();

            //获取敌方飞机的数量
            int count = SingleObject.GetSingle().listPlaneEnemy.Count;

            if (count <= 3)
            {
                InitiaPlaneEnemy();
            }

            //碰撞检测
            SingleObject.GetSingle().collision();
        }
    }
}

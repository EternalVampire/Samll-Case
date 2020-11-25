using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManage
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 新增学生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subAddStudent_Click(object sender, EventArgs e)
        {
            frmAddStudent fAddStudent = new frmAddStudent();
            fAddStudent.MdiParent = this;
            fAddStudent.Show();
        }

        /// <summary>
        /// 学生列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subStudentList_Click(object sender, EventArgs e)
        {
            bool bl = CheckForm(typeof(frmStudentList).Name);
            if (!bl)
            {
                frmStudentList fStudentList = frmStudentList.CreateInstance();
                fStudentList.MdiParent = this;
                fStudentList.Show();
            }
        }

        /// <summary>
        /// 检查窗体是否打开
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool CheckForm(string formName)
        {
            bool bl = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == formName)
                {
                    bl = true;
                    f.Activate();
                    break; 
                }
            }
            return bl;
        }

        /// <summary>
        /// 新增班级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subAddClass_Click(object sender, EventArgs e)
        {
            frmAddClass fAddClass = new frmAddClass();
            fAddClass.MdiParent = this;
            fAddClass.Show();
        }

        /// <summary>
        /// 班级列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void subClassList_Click(object sender, EventArgs e)
        {
            bool bl = CheckForm(typeof(frmClassList).Name);
            if(!bl)
            {
                frmClassList fClassList = new frmClassList();
                fClassList.MdiParent = this;
                fClassList.Show();
            }
        }

        private void subGradeList_Click(object sender, EventArgs e)
        {
            bool bl = CheckForm(typeof(frmGradeList).Name);
            if (!bl)
            {
                frmGradeList fGradeList = new frmGradeList();
                fGradeList.MdiParent = this;
                fGradeList.Show();
            }
        }

        /// <summary>
        /// 窗体关闭，退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定退出系统吗？", "退出提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
            
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定退出系统吗？", "退出提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.ExitThread();
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }
    }
}

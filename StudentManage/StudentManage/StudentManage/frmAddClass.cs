using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManage
{
    public partial class frmAddClass : Form
    {
        public frmAddClass()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //初始化年级列表
        private void frmAddClass_Load(object sender, EventArgs e)
        {
            InitGradeList();//加载年级列表
        }

        private void InitGradeList()
        {
            string sql = "select GreadeID,GreadeName from GreadeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);

            cboGrade.DataSource = dtGradeList;
            cboGrade.DisplayMember = "GreadeName";
            cboGrade.ValueMember = "GreadeID";

            cboGrade.SelectedIndex = 0;
        }

        /// <summary>
        /// 添加班级
        /// </summary>
        /// <param name="sender"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //信息获取
            string className = txtClassName.Text.Trim();
            int gradeID = (int)cboGrade.SelectedValue;
            string remark = txtRemark.Text.Trim();

            //判断是否为空
            if(string.IsNullOrEmpty(className))
            {
                MessageBox.Show("班级名称不能为空！","添加提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //判断是否已存在
            {
                string sqlExists = "Select count(1) from ClassInfo where ClassName=@ClassName and GreadeID=@GreadeID";
                SqlParameter[] paras =
                {
                    new SqlParameter("@ClassName",className),
                    new SqlParameter ("@GreadeID",gradeID)
                };
                object oCount = SqlHelper.ExecuteScalar(sqlExists, paras);

                if (oCount == null || oCount == DBNull.Value || ((int)oCount) == 0)
                {
                    //添加操作
                    string sqlAdd = "insert into ClassInfo(ClassName,GreadeID,Remark) values(@ClassName,@GreadeID,@Remark)";
                    SqlParameter[] parasAdd =
                    {
                        new SqlParameter("@ClassName",className),
                        new SqlParameter("@GreadeID",gradeID),
                        new SqlParameter("@Remark",remark)
                    };
                    //执行并返回值
                    int count = SqlHelper.ExecuteNonQuery(sqlAdd, parasAdd);
                    if(count>0)
                    {
                        MessageBox.Show($"班级：{className} 添加成功", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"班级：{className} 添加失败", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("班级名称已存在！", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

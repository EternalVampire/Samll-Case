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
    public partial class frmAddStudent : Form
    {
        public frmAddStudent()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 加载班级列表\性别默认为男
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAddStudent_Load(object sender, EventArgs e)
        {
            InitClasses();//加载班级列表
            rbtMale.Checked = true;
        }

        private void InitClasses()
        {
            //获取数据
            string sql = "select ClassID,ClassName,GreadeName from ClassInfo c,GreadeInfo g where c.GreadeID = g.GreadeID";
            DataTable dtClasses = SqlHelper.GetDataTable(sql);

            //组合班级列表显示项的过程
            if (dtClasses.Rows.Count > 0)
            {
                foreach (DataRow dr in dtClasses.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GreadeName"].ToString();
                    dr["ClassName"] = className + "--" + gradeName;
                }
            }

            //指定数据源
            cboClasses.DataSource = dtClasses;
            cboClasses.DisplayMember = "ClassName";
            cboClasses.ValueMember = "ClassID";
            cboClasses.SelectedIndex = 0;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //获取输入信息
            string stuName = txtStuName.Text.Trim();
            int classId = (int)cboClasses.SelectedValue;
            string sex = rbtMale.Checked ? rbtMale.Text : rbtFemale.Text;
            string phone = txtPhone.Text.Trim();

            //判断姓名，电话是否为空
            if (string.IsNullOrEmpty(stuName))
            {
                MessageBox.Show("姓名不能为空！", "添加学生提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("电话不能为空！", "添加学生提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //判断数据库中是否存在
            string sql = "select count(1) from StudentInfo where StuName=@StuName and Phone=@phone";
            SqlParameter[] paras =
            {
                new SqlParameter ("@StuName",stuName),
                new SqlParameter("@phone",phone)
            };
            object o = SqlHelper.ExecuteScalar(sql, paras);
            if (o != null && o != DBNull.Value && ((int)o) > 0)
            {
                MessageBox.Show("该学生已存在！", "添加学生提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string sqlAdd = "insert into StudentInfo(StuName,ClassID,Sex,Phone) values(@StuName,@ClassID,@Sex,@phone)";
            SqlParameter[] parasAdd =
            {
                new SqlParameter("@StuName",stuName),
                new SqlParameter("@ClassID",classId),
                new SqlParameter("@Sex",sex),
                new SqlParameter("@phone",phone)
            };
            int count = SqlHelper.ExecuteNonQuery(sqlAdd, parasAdd);
            if (count > 0)
            {
                MessageBox.Show($"学生：{stuName} 添加成功！", "添加学生提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("该学生添加失败，请检查！", "添加学生提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

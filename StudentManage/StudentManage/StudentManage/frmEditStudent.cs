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
    public partial class frmEditStudent : Form
    {
        public frmEditStudent()
        {
            InitializeComponent();
        }
        private int stuID = 0;
        private Action reLoad = null;

        private void frmEditStudent_Load(object sender, EventArgs e)
        {
            InitClasses();
            InitStuInfo();
        }

        private void InitStuInfo()
        {
            //获取StuID
            if(this.Tag!=null)
            {
                TagObject tagObject = (TagObject)this.Tag;
                stuID = tagObject.EditID;
                reLoad = tagObject.ReLoad;
            }

            string sql = "select StuName,Sex,ClassID,Phone from StudentInfo where StuID=@StuID";
            SqlParameter paraId = new SqlParameter("@StuID", stuID);
            SqlDataReader dr = SqlHelper.ExecuteReader(sql, paraId);

            if(dr.Read())
            {
                txtStuName.Text = dr["StuName"].ToString();
                txtPhone.Text = dr["Phone"].ToString();
                string sex = dr["Sex"].ToString();
                if (sex == "男")
                    rbtMale.Checked = true;
                else
                    rbtFemale.Checked = true;
                int classID = (int)dr["ClassID"];
                cboClasses.SelectedValue = classID;
            }
            dr.Close();
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
            

        }

        /// <summary>
        /// 修改学生信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //获取输入信息
            string stuName = txtStuName.Text.Trim();
            int classID = (int)cboClasses.SelectedValue;
            string sex = rbtMale.Checked ? rbtMale.Text.Trim() : rbtFemale.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if(string.IsNullOrEmpty(stuName))
            {
                MessageBox.Show("姓名不能为空！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("电话不能为空！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //判断是否存在
            string sql = "select count(1) from StudentInfo where StuName=@StuName and Phone=@Phone and StuID<>@StuID";
            SqlParameter[] paras =
            {
                new SqlParameter("@StuName",stuName),
                new SqlParameter("@Phone",phone),
                new SqlParameter("@StuID",stuID)
            };
            object o = SqlHelper.ExecuteScalar(sql, paras);
            if(o != null && o != DBNull.Value && ((int)o) > 0)
            {
                MessageBox.Show("该学生已存在！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //修改
            string sqlUpdate = "update StudentInfo set StuName=@StuName,Sex=@Sex,ClassID=@ClassID,Phone=@Phone where StuID=@StuID";
            SqlParameter[] parasUpdate =
            {
                new SqlParameter("@StuName",stuName),
                new SqlParameter("@ClassID",classID),
                new SqlParameter("@Sex",sex),
                new SqlParameter("@phone",phone),
                new SqlParameter("@StuID",stuID)
            };
            int count = SqlHelper.ExecuteNonQuery(sqlUpdate, parasUpdate);
            if (count > 0)
            {
                MessageBox.Show($"学生：{stuName} 修改成功！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                reLoad.Invoke();
            }
            else
            {
                MessageBox.Show("该学生修改失败，请检查！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

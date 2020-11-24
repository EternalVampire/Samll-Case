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
    public partial class FrmEditClass : Form
    {
        public FrmEditClass()
        {
            InitializeComponent();
        }

        private int ClassID = 0;
        private string oldName = "";
        private int oldGradeID = 0;
        private Action reLoad = null;

        /// <summary>
        /// 打开页面，加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEditClass_Load(object sender, EventArgs e)
        {
            InitGradeList();
            InitClassInfo();
        }

        private void InitGradeList()
        {
            string sql = "select GreadeID,GreadeName from GreadeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);

            cboGrade.DataSource = dtGradeList;
            cboGrade.DisplayMember = "GreadeName";
            cboGrade.ValueMember = "GreadeID";
        }

        private void InitClassInfo()
        {
            if (this.Tag != null)
            {
                TagObject tagObject = (TagObject)this.Tag;
                ClassID = tagObject.EditID;
                reLoad = tagObject.ReLoad;
            }

            string sql = "select ClassName,GreadeID,Remark from ClassInfo where ClassID=@ClassID";
            SqlParameter paraId = new SqlParameter("@ClassID", ClassID);
            SqlDataReader dr = SqlHelper.ExecuteReader(sql, paraId);

            if (dr.Read())
            {
                txtClassName.Text = dr["ClassName"].ToString();
                oldName = txtClassName.Text.Trim();
                txtRemark.Text = dr["Remark"].ToString();
                int gradeID = (int)dr["GreadeID"];
                oldGradeID = gradeID;
                cboGrade.SelectedValue = gradeID;
            }
            dr.Close();
        }

        /// <summary>
        /// 提交修改信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            //获取输入
            string className = txtClassName.Text.Trim();
            int gradeID = (int)cboGrade.SelectedValue;
            string remark = txtRemark.Text.Trim();

            //判断空值
            if(string.IsNullOrEmpty(className))
            {
                MessageBox.Show("名称不能为空", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //判断是否存在
            string sqlExists = "select count(1) from ClassInfo where ClassName=@ClassName and GreadeID=@GreadeID ";
            if(className == oldName && gradeID==oldGradeID)
            {
                sqlExists += " and ClassID<>@ClassID";
            }
            SqlParameter[] paras =
            {
                new SqlParameter("@ClassName",className),
                new SqlParameter("@GreadeID",gradeID),
                new SqlParameter("@ClassID",ClassID)
            };
            object oCount = SqlHelper.ExecuteScalar(sqlExists, paras);
            if(oCount != null && ((int)oCount) > 0)
            {
                MessageBox.Show("名称已存在！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //提交修改
            string sqlEdit = "update ClassInfo set ClassName=@ClassName,GreadeID=@GreadeID,Remark=@Remark where ClassID=@ClassID";
            SqlParameter[] parasEdit =
            {
                new SqlParameter("@ClassName",className),
                new SqlParameter("@GreadeID",gradeID),
                new SqlParameter("@Remark",remark),
                new SqlParameter("@ClassID",ClassID)
            };
            int count = SqlHelper.ExecuteNonQuery(sqlEdit, parasEdit);
            if(count > 0)
            {
                MessageBox.Show($"班级{className} 修改成功！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //刷新页面
                reLoad();
            }
            else
            {
                MessageBox.Show("修改失败", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

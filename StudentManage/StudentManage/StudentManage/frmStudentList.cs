using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManage
{
    public partial class frmStudentList : Form
    {
        public frmStudentList()
        {
            InitializeComponent();
        }

        private Action reLoad = null;//内置委托

        private static frmStudentList fStudentList = null;
        public static frmStudentList CreateInstance()
        {
            if(fStudentList==null || fStudentList.IsDisposed)
            fStudentList = new frmStudentList();
            return fStudentList;
        }

        //加载班级列表  加载所有的学生信息
        private void frmStudentList_Load(object sender, EventArgs e)
        {
            LoadClasses();//加载班级列表
            LoadAllStudentList();//加载所有学生信息
        }

        private void LoadAllStudentList()
        {
            string sql = "select StuID,StuName,ClassName,GreadeName,Sex,Phone from StudentInfo s " +
                "inner join ClassInfo c on c.ClassID=s.ClassID " +
                "inner join GreadeInfo g on g.GreadeID=c.GreadeID ";

            //加载数据
            DataTable dtStudents = SqlHelper.GetDataTable(sql);

            //组装
            if(dtStudents.Rows.Count > 0)
            {
                foreach(DataRow dr in dtStudents.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GreadeName"].ToString();
                    dr["ClassName"] = className + "--" + gradeName;
                }
            }

            //只显示数据
            dtStudents.Columns.Remove(dtStudents.Columns[3]);

            //绑定数据
            dgvStudents.DataSource = dtStudents;
        }

        private void LoadClasses()
        {
            //获取数据
            string sql = "select ClassID,ClassName,GreadeName from ClassInfo c,GreadeInfo g where c.GreadeID = g.GreadeID";
            DataTable dtClasses = SqlHelper.GetDataTable(sql);

            //组合班级列表显示项的过程
            if(dtClasses.Rows.Count>0)
            {
                foreach(DataRow dr in dtClasses.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GreadeName"].ToString();
                    dr["ClassName"] = className + "--" + gradeName;
                }
            }

            //添加默认选项
            DataRow drNew = dtClasses.NewRow();
            drNew["ClassID"] = 0;
            drNew["ClassName"] = "请选择";
            dtClasses.Rows.InsertAt(drNew, 0);

            //指定数据源
            cboClassName.DataSource = dtClasses;
            cboClassName.DisplayMember = "ClassName";
            cboClassName.ValueMember = "ClassID";
        }

        /// <summary>
        /// 查询学生信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            //接受条件设置信息
            int classId = (int)cboClassName.SelectedValue;
            string stuName = txtStuName.Text.Trim();

            //查询sql
            string sql = "select StuID,StuName,c.ClassName,GreadeName,Sex,Phone from StudentInfo s " +
                "inner join ClassInfo c on c.ClassID=s.ClassID " +
                "inner join GreadeInfo g on g.GreadeID=c.GreadeID ";
            sql += " where 1=1 ";
            if(classId > 0)
            {
                sql += " and c.ClassID=@ClassID ";
            }
            if(string.IsNullOrEmpty(stuName))
            {
                sql += " and StuName like @StuName ";
            }
            //sql += " where s.IsDeleted=0 ";
            sql += " order by StuID ";

            SqlParameter[] paras =
            {
                new SqlParameter("@ClassID",classId),
                new SqlParameter("@StuName","%"+ stuName +"%")
            };

            //加载数据
            DataTable dtStudents = SqlHelper.GetDataTable(sql,paras);

            if(dtStudents.Rows.Count > 0)
            {
                foreach(DataRow dr in dtStudents.Rows)
                {
                    string className = dr["ClassName"].ToString();
                    string gradeName = dr["GreadeName"].ToString();
                    dr["ClassName"] = className + "--" + gradeName;
                }
            }

            //只显示固定列
            dtStudents.Columns.Remove(dtStudents.Columns[3]);

            //绑定数据
            dgvStudents.DataSource = dtStudents;
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataRow dr = (dgvStudents.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                int stuId = int.Parse(dr["StuID"].ToString());
                //当前点击的单元格
                DataGridViewCell cell = dgvStudents.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "修改")
                {
                    //修改操作 
                    reLoad = LoadAllStudentList;
                    int stuID = (int)dr["StuID"];
                    frmEditStudent frmEdit = new frmEditStudent();
                    frmEdit.Tag = new TagObject()
                    {
                        EditID = stuID,
                        ReLoad = reLoad
                    };
                    frmEdit.MdiParent = this.MdiParent;
                    frmEdit.Show();
                }
                else if (cell is DataGridViewCell && cell.FormattedValue.ToString() == "删除")
                {
                    //删除操作
                    if (MessageBox.Show("你确定要该条信息吗","删除提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        //获取数据
                        

                        string sqlIsDel = "Delete StudentInfo  where StuID=@StuID ";
                        SqlParameter para = new SqlParameter("@StuID", stuId);
                        int count = SqlHelper.ExecuteNonQuery(sqlIsDel, para);
                        if (count > 0)
                        {
                            MessageBox.Show("该信息删除成功！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            DataTable dtStudents = (DataTable)dgvStudents.DataSource;
                            dgvStudents.DataSource = null;
                            dtStudents.Rows.Remove(dr);
                            dgvStudents.DataSource = dtStudents;
                        }
                        else
                        {
                            MessageBox.Show("该信息删除失败", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> listIds = new List<int>();
            for(int i = 0; i<dgvStudents.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dgvStudents.Rows[i].Cells["colCheck"] as DataGridViewCheckBoxCell;
                bool chk = Convert.ToBoolean(cell.Value);
                if(chk)
                {
                    DataRow dr = (dgvStudents.Rows[i].DataBoundItem as DataRowView).Row;
                    int stuId = (int)dr["StuID"];
                    listIds.Add(stuId);
                }
            }

            if(listIds.Count ==0)
            {
                MessageBox.Show("请选择删除的信息！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("你确定要删除该信息吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int count = 0;
                    //启动事务
                    using (SqlConnection conn = new SqlConnection(SqlHelper.connString))
                    {
                        conn.Open();
                        SqlTransaction trans = conn.BeginTransaction();

                        //执行事务
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = trans;

                        try
                        {
                            foreach (int id in listIds)
                            {
                                cmd.CommandText = "delete from StudentInfo where StuID=@StuID";
                                SqlParameter para = new SqlParameter("@StuID", id);
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(para);
                                count += cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch(SqlException ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("删除信息出现异常！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    if(count == listIds.Count)
                    {
                        MessageBox.Show("这些信息删除成功！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //刷新列表
                        DataTable dtStudents = (DataTable)dgvStudents.DataSource;
                        string idStr = string.Join(",", listIds);
                        DataRow[] rows = dtStudents.Select("StuID in (" + idStr + ")");
                        foreach (DataRow dr in rows)
                        {
                            dtStudents.Rows.Remove(dr);
                        }
                        dgvStudents.DataSource = dtStudents;
                    }
                }
            }
        }
    }
}

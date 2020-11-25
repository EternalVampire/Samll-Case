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
    public partial class frmClassList : Form
    {
        private Action reLoad = null;
        public frmClassList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化年级列表，所有的班级列表信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmClassList_Load(object sender, EventArgs e)
        {
            InitGrades();//加载年级列表
            InitAllClasses();//加载所有的班级信息
        }

        private void InitAllClasses()
        {
            string sql = "select ClassID, ClassName, GreadeName, Remark from ClassInfo c inner join GreadeInfo g on c.GreadeID = g.GreadeID";
            DataTable dtClasses = SqlHelper.GetDataTable(sql);

            dgvClassList.DataSource = dtClasses;
        }

        private void InitGrades()
        {
            string sql = "Select GreadeID ,GreadeName  from GreadeInfo";
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);

            DataRow dr = dtGradeList.NewRow();
            dr["GreadeID"] = 0;
            dr["GreadeName"] = "请选择年级";
            dtGradeList.Rows.InsertAt(dr, 0);

            cboGrades.DataSource = dtGradeList;
            cboGrades.DisplayMember = "GreadeName";
            cboGrades.ValueMember = "GreadeID";

            cboGrades.SelectedIndex = 0;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //获取查询条件
            int gradeId = (int)cboGrades.SelectedValue;
            string className = txtClassName.Text.Trim();

            string sql = "Select ClassID,ClassName,GreadeName,Remark from ClassInfo c inner join GreadeInfo g on c.GreadeID = g.GreadeID";
            sql += " where 1=1";
            if (gradeId > 0)
            {
                sql += " and c.GreadeID = @GreadeID";
            }
            if (!string.IsNullOrEmpty(className))
            {
                sql += " and ClassName like @ClassName";
            }

            SqlParameter[] paras =
            {
                new SqlParameter("@GreadeID",gradeId),
                new SqlParameter("@ClassName","%"+className+"%")
            };

            DataTable dtClasses = SqlHelper.GetDataTable(sql, paras);

            dgvClassList.DataSource = dtClasses;
        }

        private void dgvClassList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                //获取选择单元格
                DataGridViewCell cell = dgvClassList.Rows[e.RowIndex].Cells[e.ColumnIndex];

                DataRow dr = (dgvClassList.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                int classID = (int)dr["ClassID"];

                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "修改")
                {
                    reLoad = InitAllClasses;
                    FrmEditClass frmEdit = new FrmEditClass();
                    frmEdit.Tag = new TagObject()
                    {
                        EditID = classID,
                        ReLoad = reLoad
                    };
                    frmEdit.MdiParent = this.MdiParent;
                    frmEdit.Show();
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "删除")
                {
                    if (MessageBox.Show("你确定要删除该信息及其相关的信息吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string delStudent = "delete from StudentInfo where ClassID=@ClassID ";

                        string delClass = "delete from ClassInfo where ClassID=@ClassID";//班级信息
                        SqlParameter[] para =
                         {
                        new SqlParameter("@ClassID",classID)
                    };
                        List<CommandInfo> listComs = new List<CommandInfo>();

                        CommandInfo comStudent = new CommandInfo()
                        {
                            CommandText = delStudent,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comStudent);
                        CommandInfo comClass = new CommandInfo()
                        {
                            CommandText = delClass,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comClass);
                        //执行 事务
                        bool bl = SqlHelper.ExecuteTrans(listComs);
                        if (bl)
                        {
                            MessageBox.Show("该信息删除成功！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //刷新列表
                            DataTable dtClass = (DataTable)dgvClassList.DataSource;
                            dtClass.Rows.Remove(dr);
                            dgvClassList.DataSource = dtClass;
                        }
                        else
                        {
                            MessageBox.Show("该信息删除失败！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }



                }
            }
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelet_Click(object sender, EventArgs e)
        {
            List<int> listIds = new List<int>();
            for (int i = 0; i < dgvClassList.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dgvClassList.Rows[i].Cells["colCheck"] as DataGridViewCheckBoxCell;
                bool chk = Convert.ToBoolean(cell.Value);
                if (chk)
                {
                    DataRow dr = (dgvClassList.Rows[i].DataBoundItem as DataRowView).Row;
                    int classID = (int)dr["ClassID"];
                    listIds.Add(classID);
                }
            }

            if (listIds.Count == 0)
            {
                MessageBox.Show("请选择删除的信息！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("你确定要删除该信息及其相关信息吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string delStudent = "delete from StudentInfo where ClassID=@ClassID ";

                    string delClass = "delete from ClassInfo where ClassID=@ClassID";//班级信息
                    List<CommandInfo> listComs = new List<CommandInfo>();
                    foreach (int id in listIds)
                    {
                        SqlParameter[] para =
                        {
                            new SqlParameter("@ClassID",id)
                        };
                        CommandInfo comStudent = new CommandInfo()
                        {
                            CommandText = delStudent,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comStudent);
                        CommandInfo comClass = new CommandInfo()
                        {
                            CommandText = delClass,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comClass);
                    }

                    bool bl = SqlHelper.ExecuteTrans(listComs);
                    if(bl)
                    {
                        MessageBox.Show("这信息及其相关信息删除成功！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //刷新列表
                    DataTable dtClass = (DataTable)dgvClassList.DataSource;
                    string idStr = string.Join(",", listIds);
                    DataRow[] rows = dtClass.Select("ClassID in (" + idStr + ")");
                    foreach (DataRow dr in rows)
                    {
                        dtClass.Rows.Remove(dr);
                    }
                    dgvClassList.DataSource = dtClass;
                }
                else
                {
                    MessageBox.Show("这些信息删除失败！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

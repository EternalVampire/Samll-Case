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
    public partial class frmGradeList : Form
    {
        private int flag = 0;// 0 添加，1 修改
        private int editGradeId = 0;
        private string oldName = "";
        public frmGradeList()
        {
            InitializeComponent();
        }

        private void frmGradeList_Load(object sender, EventArgs e)
        {
            txtGradeName.Text = "";
            flag = 0;
            btnSubmit.Text = "添加";
            LoadGradeList();//加载信息
        }

        private void LoadGradeList()
        {
            string sql = "Select GreadeID ,GreadeName  from GreadeInfo";//数据库单词有误，应为Grade
            DataTable dtGradeList = SqlHelper.GetDataTable(sql);

            dgvGradeList.DataSource = dtGradeList;
        }

        /// <summary>
        /// 添加状态设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (flag != 0)
            {
                flag = 0;
                btnSubmit.Text = "添加";
                txtGradeName.Text = "";
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string gradeName = txtGradeName.Text.Trim();
            if (string.IsNullOrEmpty(gradeName))
            {
                MessageBox.Show("名称不能为空！", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (flag == 0)//添加
            {
                string sqlExist = "select count(1) from GreadeInfo where GreadeName=@GreadeName ";
                SqlParameter paraName = new SqlParameter("@GreadeName", gradeName);
                object oCount = SqlHelper.ExecuteScalar(sqlExist, paraName);
                if (oCount != null && ((int)oCount > 0))
                {
                    MessageBox.Show("名称已存在！", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string sqlAdd = "insert into GreadeInfo(GreadeName) values (@GreadeName);select @@Identity ";
                object oGradeId = SqlHelper.ExecuteScalar(sqlAdd, paraName);
                if (oGradeId != null && int.Parse(oGradeId.ToString()) > 0)
                {
                    MessageBox.Show($"年级：{gradeName}添加成功！", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DataTable dtGradeList = (DataTable)dgvGradeList.DataSource;
                    DataRow dr = dtGradeList.NewRow();
                    dr["GreadeID"] = int.Parse(oGradeId.ToString());
                    dr["GreadeName"] = gradeName;
                    dtGradeList.Rows.Add(dr);
                    dgvGradeList.DataSource = dtGradeList;
                }
                else
                {
                    MessageBox.Show($"年级：{gradeName}添加失败！", "添加提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (flag == 1)//修改
            {
                if (gradeName == oldName)
                {
                    MessageBox.Show($"名称未发生修改！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string sqlExist = "select count(1) from GreadeInfo where GreadeName=@GreadeName and GreadeID<>@GreadeID ";
                SqlParameter[] paras =
                {
                    new SqlParameter("@GreadeName",gradeName),
                    new SqlParameter("@GreadeID",editGradeId)
                };
                object oCount = SqlHelper.ExecuteScalar(sqlExist, paras);
                if (oCount != null && ((int)oCount) > 0)
                {
                    MessageBox.Show("名称已存在！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //修改入库
                string sqlUpdate = "update GreadeInfo set GreadeName=@GreadeName where GreadeID=@GreadeID";
                int count = SqlHelper.ExecuteNonQuery(sqlUpdate, paras);
                if (count > 0)
                {
                    MessageBox.Show($"年级{gradeName}修改成功！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DataTable dtGradeList = (DataTable)dgvGradeList.DataSource;
                    DataRow[] rows = dtGradeList.Select("GreadeID=" + editGradeId);
                    DataRow dr = rows[0];
                    dr["GreadeName"] = gradeName;
                    dgvGradeList.DataSource = dtGradeList;
                }
                else
                {
                    MessageBox.Show($"年级{gradeName}修改失败！", "修改提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        /// <summary>
        /// 修改或删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvGradeList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                //获取选择单元格
                DataGridViewCell cell = dgvGradeList.Rows[e.RowIndex].Cells[e.ColumnIndex];

                DataRow dr = (dgvGradeList.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                editGradeId = (int)dr["GreadeID"];

                if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "修改")
                {
                    txtGradeName.Text = dr["GreadeName"].ToString();
                    oldName = dr["GreadeName"].ToString();
                    flag = 1;
                    btnSubmit.Text = "修改";
                }
                else if (cell is DataGridViewLinkCell && cell.FormattedValue.ToString() == "删除")
                {
                    if (MessageBox.Show("你确定要删除该信息及其相关的信息吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string delStudent = "delete from StudentInfo where ClassID in (select ClassID form ClassInfo where GreadeID=@GreadeID) ";

                        string delClass = "delete from ClassInfo where ClassID=@ClassID ";//班级信息

                        string delGrade = "delect from GreadeInfo where GreadeID=@GreadeID ";//年级信息
                        SqlParameter[] para =
                        {
                            new SqlParameter("@GreadeID",editGradeId)
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

                        CommandInfo comGrade = new CommandInfo()
                        {
                            CommandText = delGrade,
                            IsProc = false,
                            Parameters = para
                        };
                        listComs.Add(comGrade);

                        //执行 事务
                        bool bl = SqlHelper.ExecuteTrans(listComs);
                        if (bl)
                        {
                            MessageBox.Show("该信息删除成功！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //刷新列表
                            DataTable dtGradeList = (DataTable)dgvGradeList.DataSource;
                            dtGradeList.Rows.Remove(dr);
                            dgvGradeList.DataSource = dtGradeList;
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
            for (int i = 0; i < dgvGradeList.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dgvGradeList.Rows[i].Cells["colCheck"] as DataGridViewCheckBoxCell;
                bool chk = Convert.ToBoolean(cell.Value);
                if (chk)
                {
                    DataRow dr = (dgvGradeList.Rows[i].DataBoundItem as DataRowView).Row;
                    int gradeId = (int)dr["GreadeID"];
                    listIds.Add(gradeId);
                }
            }

            if (listIds.Count == 0)
            {
                MessageBox.Show("请选择删除的信息！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("你确定删除这些及其相关的信息吗！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string delStudent = "delete from StudentInfo where ClassID in (select ClassID form ClassInfo where GreadeID=@GreadeID) ";

                    string delClass = "delete from ClassInfo where ClassID=@ClassID ";//班级信息

                    string delGrade = "delect from GreadeInfo where GreadeID=@GreadeID ";//年级信息
                    SqlParameter[] para =
                    {
                            new SqlParameter("@GreadeID",editGradeId)
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

                    CommandInfo comGrade = new CommandInfo()
                    {
                        CommandText = delGrade,
                        IsProc = false,
                        Parameters = para
                    };
                    listComs.Add(comGrade);

                    //执行 事务
                    bool bl = SqlHelper.ExecuteTrans(listComs);
                    if (bl)
                    {
                        MessageBox.Show("该信息删除成功！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //刷新列表
                        DataTable dtGrade = (DataTable)dgvGradeList.DataSource;

                        string idStr = string.Join(",", listIds);
                        DataRow[] rows = dtGrade.Select("GreadeID in (" + idStr + ")");
                        foreach (DataRow dr in rows)
                        {
                            dtGrade.Rows.Remove(dr);
                        }
                        dgvGradeList.DataSource = dtGrade;
                    }
                    else
                    {
                        MessageBox.Show("该信息删除失败！", "删除提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}


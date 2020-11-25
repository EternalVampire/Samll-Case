using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentManage
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //获取用户输入信息
            string uName = txtUserName.Text.Trim();
            string uPwd = txtUserPwd.Text.Trim();

            //判断是否为空
            if(string.IsNullOrEmpty(uName))
            {
                MessageBox.Show("请输入用户名!", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }
            if(string.IsNullOrEmpty(uPwd))
            {
                MessageBox.Show("请输入密码!", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserPwd.Focus();
                return;
            }

            //与数据库通信，检查输入的数据与数据库是否一致
            {
                //写查询语句
                string sql = "select count(1) from UserInfo where UserName=@UserName and UserPwd=@UserPwd";

                //添加参数
                SqlParameter[] paras =
                {
                    new SqlParameter ("@UserName",uName),
                    new SqlParameter ("@UserPwd",uPwd)
                };
                
                //调用SqlHelper类
                object o = SqlHelper.ExecuteScalar(sql, paras);

                //处理结果
                if(o==null || o==DBNull.Value || ((int)o)==0)
                {
                    MessageBox.Show("登录账号或密码有误，请重新登录", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("登录成功！", "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //转到主页面
                    FrmMain fMain = new FrmMain();
                    fMain.Show();
                    this.Hide();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

namespace Northwind.Store
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Northwind.Store.Admin;
    using Northwind.Store.Data;
    using Northwind.Store.Models;
    using Northwind.Store.Order;

    public partial class LoginScreen : Form
    {
        private readonly ToolStripMenuItem adminMenuItem;
        private readonly ToolStripMenuItem fileMenuItem;

        public LoginScreen(ToolStripMenuItem adminMenuItem, ToolStripMenuItem fileMenuItem)
        {
            this.adminMenuItem = adminMenuItem;
            this.fileMenuItem = fileMenuItem;
            this.InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.ValidateUser();
        }

        private async void ValidateUser()
        {
            var sql = @"SELECT [UserId]
                              ,[CustomerId]
                              ,[EmployeeId]
                              ,[LastName]
                              ,[FirstName]
                              ,[LoginId]
                              ,[Password]
                              ,[Admin]
                              ,[Department]
                          FROM [Northwind].[dbo].[Users] WHERE 
                               [LoginId] = @LoginId
                               AND [Password]= @Password ";
            List<User> result = await DapperService.GetAll<User>(sql,
                new { LoginId = this.txtUserName.Text, Password = this.txtPassword.Text });
            Global.LoggedInUser = result.FirstOrDefault();
            if (Global.LoggedInUser == null)
            {
                this.lblMessage.Text = "Invalid User Name or Password!";
                return;
            }

            this.fileMenuItem.Visible = true;
            this.fileMenuItem.DropDownItems["employeesToolStripMenuItem"].Visible = Global.LoggedInUser.Admin is true;
            this.adminMenuItem.Visible = Global.LoggedInUser.Admin is true;
            var orderDetailsScreen = new OrderDetailsScreen();
            this.Hide();
            orderDetailsScreen.Show();
        }
    }
}
namespace Northwind.Store.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class UserScreen : Form
    {
        private User selectedUser;

        public UserScreen()
        {
            this.InitializeComponent();
            this.BindGrid();
        }

        public async void BindGrid()
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
                          FROM [Northwind].[dbo].[Users]";
            List<User> users = await DapperService.GetAll<User>(sql, null);
            this.dgvUsers.DataSource = users;
            this.dgvUsers.ClearSelection();
        }
        private void FillUser()
        {
            this.selectedUser ??= new User();
            this.selectedUser.UserId = Convert.ToInt32(this.txtUserId.Text);
            this.selectedUser.CustomerId = this.txtCustomerId.Text;
            if (string.IsNullOrEmpty( this.selectedUser.CustomerId ) && !string.IsNullOrEmpty(this.txtEmployeeId.Text))
            {
                this.selectedUser.EmployeeId = Convert.ToInt32(this.txtEmployeeId.Text); 
            }
            this.selectedUser.LastName = this.txtLastName.Text;
            this.selectedUser.FirstName = this.txtFirstName.Text;
            this.selectedUser.LoginId = this.txtUserName.Text;
            this.selectedUser.Password = this.txtPassword.Text;
            this.selectedUser.Admin = this.chkAdmin.Checked;
            this.selectedUser.Department = this.txtDepartment.Text;
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = this.dgvUsers.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = selectedRows[0];
            if (selectedRow == null)
            {
                return;
            }

            this.selectedUser = new User
            {
                UserId = Convert.ToInt32(selectedRow.Cells[0].Value),
                CustomerId = selectedRow.Cells[1].Value?.ToString(),
                EmployeeId = Convert.ToInt32(selectedRow.Cells[2].Value),
                LastName = selectedRow.Cells[3].Value?.ToString(),
                FirstName = selectedRow.Cells[4].Value?.ToString(),
                LoginId = selectedRow.Cells[5].Value?.ToString(),
                Password = selectedRow.Cells[6].Value?.ToString(),
                Admin = Convert.ToBoolean(selectedRow.Cells[7].Value),
                Department = selectedRow.Cells[8].Value?.ToString(),
            };
            if (selectedRow.Cells[2].Value == null)
            {
                this.selectedUser.EmployeeId = null;
            }

            this.txtLastName.Text = this.selectedUser.LastName;
            this.txtFirstName.Text = this.selectedUser.FirstName;
            this.txtUserName.Text = this.selectedUser.LoginId;
            this.txtPassword.Text = this.selectedUser.Password;
            this.chkAdmin.Checked = this.selectedUser.Admin.Value;
            this.txtDepartment.Text = this.selectedUser.Department;
            this.txtUserId.Text = this.selectedUser.UserId?.ToString();
            this.txtCustomerId.Text = this.selectedUser.CustomerId;
            this.txtEmployeeId.Text = this.selectedUser.EmployeeId.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.UpdateUser();
        }

        private void btnInsertNew_Click(object sender, EventArgs e)
        {
            this.AddUser();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.selectedUser = null;
            this.txtUserId.Text = "0";
            this.txtCustomerId.Text = string.Empty;
            this.txtEmployeeId.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtFirstName.Text = string.Empty;
            this.txtUserName.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
            this.chkAdmin.Checked = false;
            this.txtDepartment.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async Task AddUser()
        {
            this.FillUser();
            var sql = @"INSERT INTO [dbo].[Users]
                               ([LastName]
                               ,[FirstName]
                               ,[LoginId]
                               ,[Password]
                               ,[Admin]
                               ,[Department]
                               ,[CustomerId]
                               ,[EmployeeId])
                         VALUES
                               (@LastName
                               ,@FirstName
                               ,@LoginId
                               ,@Password
                               ,@Admin
                               ,@Department
                               ,@CustomerId
                               ,@EmployeeId)";
            try
            {
                var saved = await DapperService.EditData(sql, this.selectedUser);
                if (saved > 1)
                {
                    this.lblMessage.Text = "User saved successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            this.BindGrid();
        }

        private async Task UpdateUser()
        {
            this.FillUser();    
            var sql = @"UPDATE [dbo].[Users]
                        SET    [LastName] = @LastName
                              ,[FirstName] = @FirstName
                              ,[LoginId] = @LoginId
                              ,[Password] = @Password
                              ,[Admin] = @Admin
                              ,[Department] = @Department
                              ,[CustomerId] = @CustomerId
                              ,[EmployeeId] = @EmployeeId
                        WHERE [UserId] = @UserId";
            try
            {
                var saved = await DapperService.EditData(sql, this.selectedUser);
                if (saved > 0)
                {
                    this.lblMessage.Text = "User saved successfully";
                }

                this.BindGrid();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
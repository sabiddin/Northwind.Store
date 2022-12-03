namespace Northwind.Store.Employee
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class EmployeeScreen : Form
    {
        private Employee selectedEmployee;

        public EmployeeScreen()
        {
            this.btnInsertNew.Visible = Global.LoggedInUser.Admin is true;
            this.btnUpdate.Visible = Global.LoggedInUser.Admin is true;
            this.btnClear.Visible = Global.LoggedInUser.Admin is true;
            this.btnCancel.Visible = Global.LoggedInUser.Admin is true;
            this.InitializeComponent();
            BindGrid();
        }

        private async Task AddEmployee()
        {
            this.FillEmployee();
            var sql = @"INSERT INTO [dbo].[Employees]
                           ([LastName]
                           ,[FirstName]
                           ,[Title]
                           ,[TitleOfCourtesy]
                           ,[BirthDate]
                           ,[HireDate]
                           ,[Address]
                           ,[City]
                           ,[Region]
                           ,[PostalCode]
                           ,[Country]
                           ,[HomePhone])
                     VALUES
                           (@LastName
                           ,@FirstName
                           ,@Title
                           ,@TitleOfCourtesy
                           ,@BirthDate
                           ,@HireDate
                           ,@Address
                           ,@City
                           ,@Region
                           ,@PostalCode
                           ,@Country
                           ,@HomePhone)";
            try
            {
                var saved = await DapperService.EditData(sql, this.selectedEmployee);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Employee saved successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            BindGrid();
        }

        private void FillEmployee()
        {
            this.selectedEmployee ??= new Employee();
            if (this.txtEmployeeId.Text.HasValue())
            {
                this.selectedEmployee.EmployeeId = Convert.ToInt32(this.txtEmployeeId.Text); 
            }

            this.selectedEmployee.LastName = this.txtLastName.Text;
            this.selectedEmployee.FirstName = this.txtFirstName.Text;
            this.selectedEmployee.Title = this.txtTitle.Text;
            this.selectedEmployee.TitleOfCourtesy = this.txtTitleOfCourtesy.Text;
            this.selectedEmployee.BirthDate = Convert.ToDateTime(this.txtBirthDate.Text);
            this.selectedEmployee.HireDate = Convert.ToDateTime(this.txtHireDate.Text);
            this.selectedEmployee.Address = this.txtAddress.Text;
            this.selectedEmployee.City = this.txtCity.Text;
            this.selectedEmployee.Region = this.txtRegion.Text;
            this.selectedEmployee.PostalCode = this.txtPostalCode.Text;
            this.selectedEmployee.Country = this.txtCountry.Text;
            this.selectedEmployee.HomePhone = this.txtHomePhone.Text;
        }

        private async Task UpdateEmployee()
        {
            this.FillEmployee();
            var sql = @"UPDATE [dbo].[Employees]
                           SET [LastName] = @LastName
                              ,[FirstName] = @FirstName
                              ,[Title] = @Title
                              ,[TitleOfCourtesy] = @TitleOfCourtesy
                              ,[BirthDate] = @BirthDate
                              ,[HireDate] = @HireDate
                              ,[Address] = @Address
                              ,[City] = @City
                              ,[Region] = @Region
                              ,[PostalCode] = @PostalCode
                              ,[Country] = @Country
                              ,[HomePhone] = @HomePhone      
                         WHERE [EmployeeID] = @EmployeeId";
                                    try
            {
                var saved = await DapperService.EditData(sql, this.selectedEmployee);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Employee saved successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            BindGrid();
        }

        public async void BindGrid()
        {
            var sql = @"SELECT [EmployeeID]
                              ,[LastName]
                              ,[FirstName]
                              ,[Title]
                              ,[TitleOfCourtesy]
                              ,[BirthDate]
                              ,[HireDate]
                              ,[Address]
                              ,[City]
                              ,[Region]
                              ,[PostalCode]
                              ,[Country]
                              ,[HomePhone]
                          FROM [Northwind].[dbo].[Employees]";
            List<Employee> employees = await DapperService.GetAll<Employee>(sql, null);
            this.dgvEmployees.DataSource = employees;
            this.dgvEmployees.ClearSelection();
        }

        private void dgvEmployees_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = this.dgvEmployees.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = selectedRows[0];
            if (selectedRow == null)
            {
                return;
            }

            this.selectedEmployee = new Employee()
            {
                EmployeeId = Convert.ToInt32(selectedRow.Cells[0].Value),
                LastName = selectedRow.Cells[1].Value?.ToString(),
                FirstName = selectedRow.Cells[2].Value?.ToString(),
                Title = selectedRow.Cells[3].Value?.ToString(),
                TitleOfCourtesy = selectedRow.Cells[4].Value?.ToString(),
                BirthDate = Convert.ToDateTime(selectedRow.Cells[5].Value),
                HireDate = Convert.ToDateTime(selectedRow.Cells[6].Value),
                Address = selectedRow.Cells[7].Value?.ToString(),
                City = selectedRow.Cells[8].Value?.ToString(),
                Region = selectedRow.Cells[9].Value?.ToString(),
                PostalCode = selectedRow.Cells[10].Value?.ToString(),
                Country = selectedRow.Cells[10].Value?.ToString(),
                HomePhone = selectedRow.Cells[10].Value?.ToString(),
            };

            this.txtEmployeeId.Text = this.selectedEmployee.EmployeeId?.ToString();
            this.txtFirstName.Text = this.selectedEmployee.FirstName;
            this.txtLastName.Text = this.selectedEmployee.LastName;
            this.txtTitle.Text = this.selectedEmployee.Title;
            this.txtTitleOfCourtesy.Text = this.selectedEmployee.TitleOfCourtesy;
            this.txtBirthDate.Text = this.selectedEmployee.BirthDate.Value.ToShortDateString();
            this.txtHireDate.Text = this.selectedEmployee.HireDate.Value.ToShortDateString();
            this.txtAddress.Text = this.selectedEmployee.Address;
            this.txtCity.Text = this.selectedEmployee.City;
            this.txtRegion.Text = this.selectedEmployee.Region;
            this.txtPostalCode.Text = this.selectedEmployee.PostalCode;
            this.txtCountry.Text = this.selectedEmployee.Country;
            this.txtHomePhone.Text = this.selectedEmployee.HomePhone?.ToString();
        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.UpdateEmployee();
        }

        private void btnInsertNew_Click(object sender, EventArgs e)
        {
            this.AddEmployee();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtEmployeeId.Text = string.Empty;
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtTitle.Text = string.Empty;
            this.txtTitleOfCourtesy.Text = string.Empty;
            this.txtBirthDate.Text = string.Empty;
            this.txtHireDate.Text = string.Empty;
            this.txtAddress.Text = string.Empty;
            this.txtCity.Text = string.Empty;
            this.txtRegion.Text = string.Empty;
            this.txtCountry.Text = string.Empty;
            this.txtHomePhone.Text = string.Empty;
            this.txtPostalCode.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
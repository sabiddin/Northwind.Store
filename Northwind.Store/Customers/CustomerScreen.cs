namespace Northwind.Store.Customers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class CustomerScreen : Form
    {
        private Customer selectedCustomer;

        public CustomerScreen()
        {
            this.InitializeComponent();
            BindGrid();
            this.btnInsertNew.Visible = Global.LoggedInUser.Admin is true;
            this.btnUpdate.Visible = Global.LoggedInUser.Admin is true;
            this.btnClear.Visible = Global.LoggedInUser.Admin is true;
            this.btnCancel.Visible = Global.LoggedInUser.Admin is true;
        }

        private async Task AddCustomer()
        {
            this.FillCustomer();
            var sql = @"INSERT INTO [dbo].[Customers]
           ([CustomerID]
           ,[CompanyName]
           ,[ContactName]
           ,[ContactTitle]
           ,[Address]
           ,[City]
           ,[Region]
           ,[PostalCode]
           ,[Country]
           ,[Phone]
           ,[Fax])
     VALUES
           (@CustomerID
           ,@CompanyName
           ,@ContactName
           ,@ContactTitle
           ,@Address
           ,@City
           ,@Region
           ,@PostalCode
           ,@Country
           ,@Phone
           ,@Fax)";
            try
            {
                var saved = await DapperService.EditData(sql, this.selectedCustomer);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Customer saved successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            BindGrid();
        }

        private void FillCustomer()
        {
            this.selectedCustomer ??= new Customer();
            this.selectedCustomer.CustomerId = this.txtCustomerId.Text;
            this.selectedCustomer.ContactName = this.txtContactName.Text;
            this.selectedCustomer.CompanyName = this.txtCompanyName.Text;
            this.selectedCustomer.ContactTitle = this.txtContactTitle.Text;
            this.selectedCustomer.Address = this.txtAddress.Text;
            this.selectedCustomer.City = this.txtCity.Text;
            this.selectedCustomer.Region = this.txtRegion.Text;
            this.selectedCustomer.PostalCode = this.txtPostalCode.Text;
            this.selectedCustomer.Country = this.txtCountry.Text;
            this.selectedCustomer.Phone = this.txtPhone.Text;
            this.selectedCustomer.Fax = this.txtFax.Text;
        }

        private async Task UpdateCustomer()
        {
            this.FillCustomer();
            var sql = @"UPDATE [dbo].[Customers]
                       SET [CustomerID] = @CustomerID
                          ,[CompanyName] = @CompanyName
                          ,[ContactName] = @ContactName
                          ,[ContactTitle] = @ContactTitle
                          ,[Address] = @Address
                          ,[City] = @City
                          ,[Region] = @Region
                          ,[PostalCode] = @PostalCode
                          ,[Country] = @Country
                          ,[Phone] =  @Phone
                          ,[Fax] =  @Fax
                     WHERE [CustomerID] =   @CustomerID";
            try
            {
                var saved = await DapperService.EditData(sql, this.selectedCustomer);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Customer saved successfully";
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
            var sql = @"SELECT [CustomerID]
                                  ,[CompanyName]
                                  ,[ContactName]
                                  ,[ContactTitle]
                                  ,[Address]
                                  ,[City]
                                  ,[Region]
                                  ,[PostalCode]
                                  ,[Country]
                                  ,[Phone]
                                  ,[Fax]
                              FROM [Northwind].[dbo].[Customers]";
            List<Customer> customers = await DapperService.GetAll<Customer>(sql, null);
            this.dgvCustomers.DataSource = customers;
            this.dgvCustomers.ClearSelection();
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = this.dgvCustomers.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = selectedRows[0];
            if (selectedRow == null)
            {
                return;
            }

            this.selectedCustomer = new Customer
            {
                CustomerId = selectedRow.Cells[0].Value?.ToString(),
                CompanyName = selectedRow.Cells[1].Value?.ToString(),
                ContactName = selectedRow.Cells[2].Value?.ToString(),
                ContactTitle = selectedRow.Cells[3].Value?.ToString(),
                Address = selectedRow.Cells[4].Value?.ToString(),
                City = selectedRow.Cells[5].Value?.ToString(),
                Region = selectedRow.Cells[6].Value?.ToString(),
                PostalCode = selectedRow.Cells[7].Value?.ToString(),
                Country = selectedRow.Cells[8].Value?.ToString(),
                Phone = selectedRow.Cells[9].Value?.ToString(),
                Fax = selectedRow.Cells[10].Value?.ToString(),
            };

            this.txtCustomerId.Text = this.selectedCustomer.CustomerId?.ToString();
            this.txtCompanyName.Text = this.selectedCustomer.CompanyName;
            this.txtContactName.Text = this.selectedCustomer.ContactName?.ToString();
            this.txtContactTitle.Text = this.selectedCustomer.ContactTitle?.ToString();
            this.txtAddress.Text = this.selectedCustomer.Address;
            this.txtCity.Text = this.selectedCustomer.City?.ToString();
            this.txtRegion.Text = this.selectedCustomer.Region?.ToString();
            this.txtPostalCode.Text = this.selectedCustomer.PostalCode;
            this.txtCountry.Text = this.selectedCustomer.Country?.ToString();
            this.txtPhone.Text = this.selectedCustomer.Phone;
            this.txtFax.Text = this.selectedCustomer.Fax?.ToString();
        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.UpdateCustomer();
        }

        private void btnInsertNew_Click(object sender, EventArgs e)
        {
            this.AddCustomer();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCustomerId.Text = string.Empty;
            this.txtCompanyName.Text = string.Empty;
            this.txtContactName.Text = string.Empty;
            this.txtContactTitle.Text = string.Empty;
            this.txtAddress.Text = string.Empty;
            this.txtCity.Text = string.Empty;
            this.txtRegion.Text = string.Empty;
            this.txtCountry.Text = string.Empty;
            this.txtFax.Text = string.Empty;
            this.txtPhone.Text = string.Empty;
            this.txtPostalCode.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
namespace Northwind.Store.Customers
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class frmCustomer : Form
    {
        private Customer selectedCustomer;

        public frmCustomer()
        {
            this.InitializeComponent();
            this.BindGrid();
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
                Country = selectedRow.Cells[7].Value?.ToString(),
                Fax = selectedRow.Cells[8].Value?.ToString(),
                Phone = selectedRow.Cells[9].Value?.ToString(),
                PostalCode = selectedRow.Cells[10].Value?.ToString(),
            };
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var customerEdit = new frmCustomerEdit(this.selectedCustomer, ModeEnum.Update);
            customerEdit.ShowDialog();
            var dr = customerEdit.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.BindGrid();
            }

            customerEdit.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var customerEdit = new frmCustomerEdit(null, ModeEnum.Add);
            var dr = customerEdit.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                this.BindGrid();
            }

            customerEdit.Close();
        }

        private void pnlCustomers_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            if (Global.LoggedInUser.Admin is true)
            {
                this.btnAdd.Visible = true;
                this.btnEdit.Visible = true;
                this.btnEdit.Text = "Edit Customer";
            }
            else
            {
                this.btnAdd.Visible = false;
                this.btnEdit.Text = "View Customer";
            }
        }
    }
}
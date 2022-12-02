namespace Northwind.Store.Supplier
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class SupplierScreen : Form
    {
        private Supplier selectedSupplier;

        public SupplierScreen()
        {
            this.InitializeComponent();
            this.BindGrid();
            this.btnInsertNew.Visible = Global.LoggedInUser.Admin is true;
            this.btnUpdate.Visible = Global.LoggedInUser.Admin is true;
            this.btnClear.Visible = Global.LoggedInUser.Admin is true;
            this.btnCancel.Visible = Global.LoggedInUser.Admin is true;
        }

        private async Task AddSupplier()
        {
            this.FillSupplier();
            var sql = @"INSERT INTO [dbo].[Suppliers]
                                   ([CompanyName]
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
                                   (@CompanyName
                                    ,@ContactName
                                    ,@ContactTitle
                                    ,@Address
                                    ,@City
                                    ,@Region
                                    ,@PostalCode
                                    ,@Country
                                    ,@Phone
                                    ,@Fax)";
            var saved = await DapperService.EditData(sql, this.selectedSupplier);
            if (saved > 0)
            {
                this.lblMessage.Text = "Supplier saved successfully";
            }

            this.BindGrid();
        }

        private void FillSupplier()
        {
            this.selectedSupplier ??= new Supplier();
            if (this.txtSupplierId.Text.HasValue())
            {
                this.selectedSupplier.SupplierId = Convert.ToInt32(this.txtSupplierId.Text);
            }

            this.selectedSupplier.ContactName = this.txtContactName.Text;
            this.selectedSupplier.CompanyName = this.txtCompanyName.Text;
            this.selectedSupplier.ContactTitle = this.txtContactTitle.Text;
            this.selectedSupplier.Address = this.txtAddress.Text;
            this.selectedSupplier.City = this.txtCity.Text;
            this.selectedSupplier.Region = this.txtRegion.Text;
            this.selectedSupplier.PostalCode = this.txtPostalCode.Text;
            this.selectedSupplier.Country = this.txtCountry.Text;
            this.selectedSupplier.Phone = this.txtPhone.Text;
            this.selectedSupplier.Fax = this.txtFax.Text;
        }

        private async Task UpdateSupplier()
        {
            this.FillSupplier();
            var sql = @"UPDATE [dbo].[Suppliers]
                           SET [CompanyName] = CompanyName
                              ,[ContactName] = ContactName
                              ,[ContactTitle] = ContactTitle
                              ,[Address] = Address
                              ,[City] = City
                              ,[Region] = Region
                              ,[PostalCode] = PostalCode
                              ,[Country] = Country
                              ,[Phone] = Phone
                              ,[Fax] = Fax                              
                         WHERE [SupplierID] = @SupplierId";
            var saved = await DapperService.EditData(sql, this.selectedSupplier);
            if (saved > 0)
            {
                this.lblMessage.Text = "Supplier saved successfully";
            }

            this.BindGrid();
        }

        public async void BindGrid()
        {
            var sql = @"SELECT [SupplierID]
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
                              ,[HomePage]
                          FROM [Northwind].[dbo].[Suppliers]";
            List<Supplier> suppliers = await DapperService.GetAll<Supplier>(sql, null);
            this.dgvSuppliers.DataSource = suppliers;
            this.dgvSuppliers.ClearSelection();
        }

        private void dgvSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = this.dgvSuppliers.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = selectedRows[0];
            if (selectedRow == null)
            {
                return;
            }

            this.selectedSupplier = new Supplier
            {
                SupplierId = Convert.ToInt32(selectedRow.Cells[0].Value),
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

            this.txtSupplierId.Text = this.selectedSupplier.SupplierId?.ToString();
            this.txtCompanyName.Text = this.selectedSupplier.CompanyName;
            this.txtContactName.Text = this.selectedSupplier.ContactName;
            this.txtContactTitle.Text = this.selectedSupplier.ContactTitle;
            this.txtAddress.Text = this.selectedSupplier.Address;
            this.txtCity.Text = this.selectedSupplier.City;
            this.txtRegion.Text = this.selectedSupplier.Region;
            this.txtPostalCode.Text = this.selectedSupplier.PostalCode;
            this.txtCountry.Text = this.selectedSupplier.Country;
            this.txtPhone.Text = this.selectedSupplier.Phone;
            this.txtFax.Text = this.selectedSupplier.Fax;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.UpdateSupplier();
        }

        private void btnInsertNew_Click(object sender, EventArgs e)
        {
            this.AddSupplier();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtSupplierId.Text = string.Empty;
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
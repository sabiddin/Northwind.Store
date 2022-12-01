namespace Northwind.Store.Product
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class ProductScreen : Form
    {
        private Product selectedProduct;

        public ProductScreen()
        {
            this.InitializeComponent();
            this.BindGrid();
            this.btnInsertNew.Visible = Global.LoggedInUser.Admin is true;
            this.btnUpdate.Visible = Global.LoggedInUser.Admin is true;
            this.btnClear.Visible = Global.LoggedInUser.Admin is true;
            this.btnCancel.Visible = Global.LoggedInUser.Admin is true;
        }

        public async void BindGrid()
        {
            var sql = @"SELECT [ProductID]
                              ,[ProductName]
                              ,[SupplierID]
                              ,[CategoryID]
                              ,[QuantityPerUnit]
                              ,[UnitPrice]
                              ,[UnitsInStock]
                              ,[UnitsOnOrder]
                              ,[ReorderLevel]
                              ,[Discontinued]
                          FROM [Northwind].[dbo].[Products]";
            List<Product> products = await DapperService.GetAll<Product>(sql, null);
            this.dgvProducts.DataSource = products;
            this.dgvProducts.ClearSelection();
        }

        private void FillProduct()
        {
            this.selectedProduct ??= new Product();
            if (this.txtProductId.Text.HasValue())
            {
                this.selectedProduct.ProductId = Convert.ToInt32(this.txtProductId.Text);
            }

            this.selectedProduct.ProductName = this.txtProductName.Text;
            if (this.txtSupplierId.Text.HasValue())
            {
                this.selectedProduct.SupplierId = Convert.ToInt32(this.txtSupplierId.Text);
            }

            if (this.txtCategoryId.Text.HasValue())
            {
                this.selectedProduct.CategoryId = Convert.ToInt32(this.txtCategoryId.Text);
            }

            this.selectedProduct.QuantityPerUnit = this.txtQuantityPerUnit.Text;
            if (this.txtUnitPrice.Text.HasValue())
            {
                this.selectedProduct.UnitPrice = Convert.ToDecimal(this.txtUnitPrice.Text);
            }

            if (this.txtUnitsInStock.Text.HasValue())
            {
                this.selectedProduct.UnitsInStock = Convert.ToInt32(this.txtUnitsInStock.Text);
            }

            if (this.txtUnitsOnOrder.Text.HasValue())
            {
                this.selectedProduct.UnitsOnOrder = Convert.ToInt32(this.txtUnitsOnOrder.Text);
            }

            if (this.txtReorderLevel.Text.HasValue())
            {
                this.selectedProduct.ReorderLevel = Convert.ToInt32(this.txtReorderLevel.Text);
            }

            this.selectedProduct.Discontinued = this.chkDiscontinued.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.selectedProduct ??= new Product();
            txtProductId.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtSupplierId.Text = string.Empty;
            txtCategoryId.Text = string.Empty;
            txtQuantityPerUnit.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtUnitsInStock.Text = string.Empty;
            txtUnitsOnOrder.Text = string.Empty;
            txtReorderLevel.Text = string.Empty;
            chkDiscontinued.Checked = false;
        }

        private void btnInsertNew_Click(object sender, EventArgs e)
        {
            this.AddProduct();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.UpdateProduct();
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = this.dgvProducts.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = selectedRows[0];
            if (selectedRow == null)
            {
                return;
            }

            this.selectedProduct = new Product
            {
                ProductId = Convert.ToInt32(selectedRow.Cells[0].Value),
                ProductName = selectedRow.Cells[1].Value?.ToString(),
                SupplierId = Convert.ToInt32(selectedRow.Cells[2].Value),
                CategoryId = Convert.ToInt32(selectedRow.Cells[3].Value),
                QuantityPerUnit = selectedRow.Cells[4].Value?.ToString(),
                UnitPrice = Convert.ToDecimal(selectedRow.Cells[5].Value),
                UnitsInStock = Convert.ToInt32(selectedRow.Cells[6].Value),
                UnitsOnOrder = Convert.ToInt32(selectedRow.Cells[7].Value),
                ReorderLevel = Convert.ToInt32(selectedRow.Cells[8].Value),
                Discontinued = Convert.ToBoolean(selectedRow.Cells[9].Value),
            };
            if (selectedRow.Cells[2].Value == null)
            {
                this.selectedProduct.SupplierId = null;
            }

            if (selectedRow.Cells[3].Value == null)
            {
                this.selectedProduct.CategoryId = null;
            }

            txtProductId.Text = this.selectedProduct.ProductId?.ToString();
            txtProductName.Text = this.selectedProduct.ProductName;
            txtSupplierId.Text = this.selectedProduct.SupplierId?.ToString();
            txtCategoryId.Text = this.selectedProduct.CategoryId?.ToString();
            txtQuantityPerUnit.Text = this.selectedProduct.QuantityPerUnit;
            txtUnitPrice.Text = this.selectedProduct.UnitPrice?.ToString();
            txtUnitsInStock.Text = this.selectedProduct.UnitsInStock?.ToString();
            txtUnitsOnOrder.Text = this.selectedProduct.UnitsOnOrder?.ToString();
            txtReorderLevel.Text = this.selectedProduct.ReorderLevel?.ToString();
            chkDiscontinued.Checked = this.selectedProduct.Discontinued.Value;
        }

        private async Task AddProduct()
        {
            this.FillProduct();
            var sql = @"INSERT INTO [dbo].[Products]
                               ([ProductName]
                               ,[SupplierID]
                               ,[CategoryID]
                               ,[QuantityPerUnit]
                               ,[UnitPrice]
                               ,[UnitsInStock]
                               ,[UnitsOnOrder]
                               ,[ReorderLevel]
                               ,[Discontinued])
                         VALUES
                               (@ProductName
                               ,@SupplierID
                               ,@CategoryID
                               ,@QuantityPerUnit
                               ,@UnitPrice
                               ,@UnitsInStock
                               ,@UnitsOnOrder
                               ,@ReorderLevel
                               ,@Discontinued)";
                                try
            {
                var saved = await DapperService.EditData(sql, this.selectedProduct);
                if (saved > 1)
                {
                    this.lblMessage.Text = "Product added successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            this.BindGrid();
        }

        private async Task UpdateProduct()
        {
            this.FillProduct();
            var sql = @"UPDATE [dbo].[Products]
                           SET [ProductName] = @ProductName
                              ,[SupplierID] = @SupplierID
                              ,[CategoryID] = @CategoryID
                              ,[QuantityPerUnit] = @QuantityPerUnit
                              ,[UnitPrice] = @UnitPrice
                              ,[UnitsInStock] = @UnitsInStock
                              ,[UnitsOnOrder] = @UnitsOnOrder
                              ,[ReorderLevel] = @ReorderLevel
                              ,[Discontinued] = @Discontinued
                         WHERE [ProductID] = @ProductID";
            try
            {
                var saved = await DapperService.EditData(sql, this.selectedProduct);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Product updated successfully";
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
namespace Northwind.Store.Order
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class OrderDetailsScreen : Form
    {
        private const string employeeSql =
            @" SELECT O.OrderID, o.CustomerId, o.EmployeeId, o.OrderDate, od.ProductID, p.ProductName, od.Discount, od.Quantity, od.UnitPrice FROM Orders o INNER JOIN [Order Details] od  ON o.OrderID = od.OrderID INNER JOIN [Products] p ON od.ProductID = p.ProductID WHERE o.EmployeeId = @EmployeeId";

        private const string customerSql =
            @" SELECT O.OrderID, o.CustomerID, o.EmployeeID, o.OrderDate, od.ProductID, p.ProductName, od.Discount, od.Quantity, od.UnitPrice FROM Orders o INNER JOIN [Order Details] od  ON o.OrderID = od.OrderID INNER JOIN [Products] p ON od.ProductID = p.ProductID WHERE o.CustomerId = @CustomerId";

        private OrderDetails orderDetails;

        public OrderDetailsScreen()
        {
            this.InitializeComponent();
            this.BindGrid();
        }

        public async void BindGrid()
        {
            var sql = "";
            sql = Global.LoggedInUser.EmployeeId.HasValue ? employeeSql : customerSql;
            List<OrderDetails> orderDetailsList = await DapperService.GetAll<OrderDetails>(sql,
                new { Global.LoggedInUser.CustomerId, Global.LoggedInUser.EmployeeId });
            this.dgvOrderDetails.DataSource = orderDetailsList;
            this.dgvOrderDetails.ClearSelection();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void dgvOrderDetails_SelectionChanged(object sender, EventArgs e)
        {
            var selectedRows = this.dgvOrderDetails.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = selectedRows[0];
            if (selectedRow == null)
            {
                return;
            }

            this.orderDetails = new OrderDetails
            {
                OrderId = Convert.ToInt32(selectedRow.Cells[0].Value),
                CustomerId = selectedRow.Cells[1].Value?.ToString(),
                EmployeeId = Convert.ToInt32(selectedRow.Cells[2].Value),
                OrderDate = Convert.ToDateTime(selectedRow.Cells[3].Value),
                ProductId = Convert.ToInt32(selectedRow.Cells[4].Value),
                ProductName = selectedRow.Cells[5].Value?.ToString(),
                Discount = Convert.ToDecimal(selectedRow.Cells[6].Value),
                Quantity = Convert.ToInt32(selectedRow.Cells[7].Value),
                UnitPrice = Convert.ToDecimal(selectedRow.Cells[8].Value),
            };

            this.txtOrderDate.Text = this.orderDetails.OrderDate.ToShortDateString();
            this.txtProductName.Text = this.orderDetails.ProductName;
            this.txtDiscount.Text = this.orderDetails.Discount.ToString();
            this.txtQuantity.Text = this.orderDetails.Quantity.ToString();
            this.txtUnitPrice.Text = this.orderDetails.UnitPrice.ToString();
        }
    }
}
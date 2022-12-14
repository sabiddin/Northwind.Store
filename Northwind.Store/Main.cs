namespace Northwind.Store
{
    using System;
    using System.Windows.Forms;
    using Northwind.Store.Admin;
    using Northwind.Store.Customers;
    using Northwind.Store.Employee;
    using Northwind.Store.Order;
    using Northwind.Store.Product;
    using Northwind.Store.Supplier;

    public partial class Main : Form
    {
        private int childFormNumber;

        public Main()
        {
            this.InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            var childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + this.childFormNumber++;
            childForm.Show();
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var customerScreen = new CustomerScreen();
            customerScreen.MdiParent = this;
            customerScreen.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.adminToolStripMenuItem.Visible = false;
            this.fileMenu.Visible = false;
            var loginScreen = new LoginScreen(this.adminToolStripMenuItem, this.fileMenu);
            loginScreen.MdiParent = this;
            loginScreen.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var userScreen = new UserScreen();
            userScreen.MdiParent = this;
            userScreen.Show();
        }

        private void orderDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var orderDetailsScreen = new OrderDetailsScreen();
            orderDetailsScreen.MdiParent = this;
            orderDetailsScreen.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var productScreen = new ProductScreen();
            productScreen.MdiParent = this;
            productScreen.Show();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var employeeScreen = new EmployeeScreen();
            employeeScreen.MdiParent = this;
            employeeScreen.Show();
        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var screen = new SupplierScreen();
            screen.MdiParent = this;
            screen.Show();
        }
    }
}
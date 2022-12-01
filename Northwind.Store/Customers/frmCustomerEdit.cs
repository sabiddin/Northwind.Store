namespace Northwind.Store.Customers
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Northwind.Store.Data;
    using Northwind.Store.Models;

    public partial class frmCustomerEdit : Form
    {
        private Customer customer;
        private ModeEnum mode;

        public frmCustomerEdit(Customer customer, ModeEnum mode)
        {
            this.customer = customer;
            this.mode = mode;
            this.InitializeComponent();
            this.LoadScreen();
        }

        private async void LoadScreen()
        {
            if (this.customer != null)
            {
                this.InitializeEdit();
            }
        }

        private void InitializeEdit()
        {
            this.txtCustomerId.Text = this.customer.CustomerId;
            this.txtContactName.Text = this.customer.ContactName;
            this.txtCompanyName.Text = this.customer.CompanyName;
            this.txtContactTitle.Text = this.customer.ContactTitle;
            this.txtAddress.Text = this.customer.Address;
            this.txtCity.Text = this.customer.City;
            this.txtRegion.Text = this.customer.Region;
            this.txtPostalCode.Text = this.customer.PostalCode;
            this.txtCountry.Text = this.customer.Country;
            this.txtPhone.Text = this.customer.Phone;
            this.txtFax.Text = this.customer.Fax;
        }

        private void FillCustomer()
        {
            this.customer ??= new Customer();
            this.customer.CustomerId = this.txtCustomerId.Text;
            this.customer.ContactName = this.txtContactName.Text;
            this.customer.CompanyName = this.txtCompanyName.Text;
            this.customer.ContactTitle = this.txtContactTitle.Text;
            this.customer.Address = this.txtAddress.Text;
            this.customer.City = this.txtCity.Text;
            this.customer.Region = this.txtRegion.Text;
            this.customer.PostalCode = this.txtPostalCode.Text;
            this.customer.Country = this.txtCountry.Text;
            this.customer.Phone = this.txtPhone.Text;
            this.customer.Fax = this.txtFax.Text;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            switch (this.mode)
            {
                case ModeEnum.Add:
                    await this.AddCustomer();
                    break;
                case ModeEnum.Update:
                    await this.UpdateCustomer();
                    break;
            }
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
                var saved = await DapperService.EditData(sql, this.customer);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Customer saved successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
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
                var saved = await DapperService.EditData(sql, this.customer);
                if (saved > 0)
                {
                    this.lblMessage.Text = "Customer saved successfully";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void frmCustomerEdit_Load(object sender, EventArgs e)
        {
            if (Global.LoggedInUser.Admin is true)
            {
                this.btnSave.Visible = true;
                this.btnCancel.Visible = true; 
            }
            else
            {
                this.btnSave.Visible = false;
                this.btnCancel.Visible = false;
            }

        }
    }
}
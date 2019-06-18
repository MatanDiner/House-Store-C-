using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class payment : System.Web.UI.Page
{
    int i = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        TotalPriceLBL.Text = Convert.ToString("<br/><h3>Total Price: " + Session["totalPrice"] + "</h3>");


        if (!IsPostBack)
        {
            if (Session["logedInUser"] == null)//unsigned user
            {
                Response.Redirect("login.aspx");
            }
            else //signed user
            {
                User logedInUser = (User)Session["logedInUser"];

                if (logedInUser.Type == "administrator")//administrator user
                    Response.Redirect("inventoryManagement.aspx");
            }

        }

    }

    protected void checkCalender(object sender, EventArgs e)
    {
        ShipDateValidator.Text = ShipDate_CLD.SelectedDate.ToString();
        if (ShipDate_CLD.SelectedDate.ToString() == "01/01/0001 00:00:00")
        {
            ShipDateValidator.Visible = true;
            ShipDateValidator.Text = "Please Enter a Shippment Date";
            return;
        }
        else
            if (ShipDate_CLD.SelectedDate <= DateTime.Today)
            {
                ShipDateValidator.Text = "This date was in past";
                ShipDateValidator.Visible = true;
                return;
            }
            else
                if (ShipDate_CLD.SelectedDate <= DateTime.Today.AddMonths(1))
                {
                    ShipDateValidator.Visible = true;
                    ShipDateValidator.Text = "Please choose a date that is at least 1 month forward";
                    return;
                }
                else
                {
                    ShipDateValidator.Visible = false;
                    ShipDateValidator.Text = "";
                }
    }

    protected void PhonePayment_RB_CheckedChanged(object sender, EventArgs e)
    {
        PhonePaymentPlaceHolder.Visible = true;
        creditPaymentPlaceHolder.Visible = false;
        Session["howPayment"] = "cash";
    }

    protected void creditPayment_RB_CheckedChanged(object sender, EventArgs e)
    {
        PhonePaymentPlaceHolder.Visible = false;
        creditPaymentPlaceHolder.Visible = true;
        Session["howPayment"] = "credit";
    }





    protected void Payment_Submit_Click(object sender, EventArgs e)
    {
        checkCalender(sender, e);
        string signature = FileUpload.FileName;
        string SavedFileName = Server.MapPath(".") + "/assets/UploadedFiles/uploadNo" + i + signature;
        FileUpload.SaveAs(SavedFileName);

        signatureImage.ImageUrl = "assets/UploadedFiles/uploadNo" + i + signature;
        signatureImage.ToolTip = "your signature";
        signatureLable.Text = "<br/>form uploaded succesfully";
        i++;


        if (Session["CartProducts"] != null)  // this session came from cart  !!
        {
            int SuccessfulSignUpInSalesTable = 0;
            int SuccessfulInventoryUpdate = 0;
            int countSales = 0;
            Dictionary<Product, int> SalesProducts = (Dictionary<Product, int>)Session["CartProducts"]; // this session came from cart  !!

            foreach (KeyValuePair<Product, int> kvp in SalesProducts)
            {
                countSales++;
                DBservices dbs = new DBservices();
                Sale sale = new Sale();
                sale.Product = kvp.Key;
                sale.ProductTotalPrice = (float)kvp.Key.Price * kvp.Value;
                sale.Amount = kvp.Value;
                User logedInUser = (User)Session["logedInUser"];//get the  user name
                sale.UserName = logedInUser.Name;
                sale.Date = DateTime.Now;//get the  date
                sale.Payment = (string)Session["howPayment"];
                SuccessfulSignUpInSalesTable += dbs.insertSale(sale);  //insert row to sale table

                int newInvetoryValue = kvp.Key.Inventory - kvp.Value;
                SuccessfulInventoryUpdate += dbs.updateInventory(newInvetoryValue, kvp.Key.Id);//update inventory
            }

            if (countSales == SuccessfulSignUpInSalesTable && countSales == SuccessfulInventoryUpdate)
            //option 1 //check if we finish to save and update our data . and give massage to user
            //option 2   if (SalesProducts.Keys.Last() == kvp.Key) 
            {
                priceSuccsesMassege.Controls.Add(new LiteralControl("<h2>Sale successfully saved</h2>"));
                priceSuccsesMassege.Visible = true;
            }
        }
    }
}
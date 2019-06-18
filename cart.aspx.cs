using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cart : System.Web.UI.Page
{
    double totalPrice = 0;
    bool fromAmount = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["logedInUser"] == null)
            {//unsigned user
                Response.Redirect("login.aspx");
            }
            if (Session["logedInUser"] != null)
            {//signed user
                User logedInUser = (User)Session["logedInUser"];
                if (logedInUser.Type == "administrator")//administrator user
                    Response.Redirect("inventoryManagement.aspx");
                else
                {
                    Session["ProductsInDiscount"] = null;
                }
            }
        }

        initialPage();
    }

    protected void initialPage()
    {
        if (Session["checkedProductsList"] != null)
        {
            List<Product> checkedProductsList = (List<Product>)Session["checkedProductsList"];
            if (checkedProductsList == null || checkedProductsList.Count < 1)
            {
                cartPh.Controls.Add(new LiteralControl("<h2>No products chose</h2>"));
            }
            else
            {
                cartPh.Controls.Add(new LiteralControl("<div>"));
                Label totalPriceLBL = new Label();
                totalPriceLBL.ID = "totalPriceLBL";
                cartPh.Controls.Add(totalPriceLBL);
                totalPriceLBL.CssClass = "total";
                cartPh.Controls.Add(new LiteralControl("</div>"));

                foreach (var product in checkedProductsList)
                {
                    //create controllers
                    Image ProductImage = new Image();
                    Label ProductTitle = new Label();
                    Label ProductPrice = new Label();
                    CheckBox ProductCheckBox = new CheckBox();
                    TextBox amountTB = new TextBox();
                    Label amountAlertLBL = new Label();
                    RegularExpressionValidator regex = new RegularExpressionValidator();
                    //set controllers
                    ProductCheckBox.ID = "CartProductCheckBox" + Convert.ToString(product.Id);
                    ProductCheckBox.Checked = true;
                    ProductCheckBox.Text = "Remove from cart";
                    ProductTitle.Text = "<h3 class='content'>" + Convert.ToString(product.ProductName) + "</h3>";
                    ProductImage.ImageUrl = product.ImagePath;
                    ProductCheckBox.AutoPostBack = true;
                    ProductCheckBox.CheckedChanged += CheckedChangedFunc;
                    ProductPrice.Text = Convert.ToString(product.Price);
                    ProductPrice.ID = "ProductPrice" + Convert.ToString(product.Id);
                    amountTB.Text = "1";
                    amountTB.Columns = 1;
                    amountTB.ID = "CartProductAmount" + Convert.ToString(product.Id);
                    amountTB.AutoPostBack = true;
                    amountTB.TextChanged += amountTB_TextChanged;
                    amountAlertLBL.ID = "ProductAmountError" + Convert.ToString(product.Id);
                    regex.ControlToValidate = amountTB.ID;
                    regex.ValidationExpression = "^[0-9]*$";
                    regex.ErrorMessage = "numbers only!";

                    //insert all info to page
                    cartPh.Controls.Add(new LiteralControl("<div class='col-lg-3 col-md-6 col-xs-12'>"));
                    cartPh.Controls.Add(new LiteralControl("<div class='jumbotron'>"));
                    cartPh.Controls.Add(ProductImage);
                    cartPh.Controls.Add(new LiteralControl("<br />"));
                    cartPh.Controls.Add(ProductTitle);
                    cartPh.Controls.Add(new LiteralControl("Price per product:"));
                    cartPh.Controls.Add(ProductPrice);
                    cartPh.Controls.Add(new LiteralControl("<br />"));
                    cartPh.Controls.Add(ProductCheckBox);
                    cartPh.Controls.Add(new LiteralControl("<br /> Amount: "));
                    cartPh.Controls.Add(amountTB);
                    cartPh.Controls.Add(new LiteralControl("<br />"));
                    cartPh.Controls.Add(regex);
                    cartPh.Controls.Add(amountAlertLBL);
                    cartPh.Controls.Add(new LiteralControl("</div> </div>"));
                    totalPrice += product.Price;
                }
                totalPriceLBL.Text = Convert.ToString("Total Price:" + totalPrice);
            }
            if (Request.Cookies["CartChanged"] != null)
            {
                if (Request.Cookies["CartChanged"].Value == "1")
                {
                    CartMessageArea.Text = "<h2>The changes have been saved</h2>";
                    Response.Cookies["CartChanged"].Value = "0";
                }
            }
        }
        else cartPh.Controls.Add(new LiteralControl("<h2>You didn't chose product yet!</h2>"));


    }


    //check inventory, update the inventory message and update total
    void amountTB_TextChanged(object sender, EventArgs e)
    {
        Dictionary<int, int> ProductsInDiscount = new Dictionary<int, int>();
        if (Session["ProductsInDiscount"] != null)                                       //bring back the original price if needed
        {
            Dictionary<int, int> OldProductsInDiscount = (Dictionary<int, int>)Session["ProductsInDiscount"];
            ProductsInDiscount = OldProductsInDiscount;
            foreach (var item in OldProductsInDiscount)
            {
                TextBox amount = (TextBox)cartPh.FindControl("CartProductAmount" + item.Key);
                if (Convert.ToInt32(amount.Text) < 6)                                    //bring back the original price
                {
                    Label price = (Label)cartPh.FindControl("ProductPrice" + Convert.ToString(item.Key));
                    Label alert = (Label)cartPh.FindControl("ProductAmountError" + Convert.ToString(item.Key));
                    price.Text = Convert.ToString(item.Value);
                    price.CssClass = "";
                    alert.Text = "";
                }
            }
        }

        if (Session["lastErrorLableId"] != null)                                        //clear last error on next press
        {
            string lastErrorLableId = (string)Session["lastErrorLableId"];
            Label OldAlertLBL = (Label)cartPh.FindControl(lastErrorLableId);
            OldAlertLBL.Text = "";
            Session["lastErrorLableId"] = null;
        }

        DBservices dbs = new DBservices();
        TextBox TB = (TextBox)sender;
        string name = TB.ID;
        int productId = Convert.ToInt32(name.Replace("CartProductAmount", ""));

        try                                                              //get the inventory of the product that customer pressed on.
        {
            string selectquery = "select productN_inventory from productN where productN_id=" + productId;
            dbs = dbs.searchItemsInDataBase("ProductsDBConnectionString", selectquery);
        }
        catch (Exception ex)
        {
            Response.Write("Unable to read from the database " + ex.Message);
            return;
        }

        foreach (DataRow dr in dbs.dt.Rows)
        {
            int productInventory = (int)dr["productN_inventory"];
            Label AlertLBL = (Label)cartPh.FindControl("ProductAmountError" + Convert.ToString(productId));

            if (Convert.ToInt32(TB.Text) > productInventory || Convert.ToInt32(TB.Text) < 1)   //not enaught products in inventory
            {
                AlertLBL.Text = "<h6 class='alert'>Inventory is " + productInventory + "<h6>";
                TB.Text = "1";
                if (ProductsInDiscount.ContainsKey(productId) == true)
                { // if product already got discount and then search for amount that is highr then inventory
                    double originalPrice = ProductsInDiscount[productId];
                    Label ProductPricetLBL = (Label)cartPh.FindControl("ProductPrice" + Convert.ToString(productId));
                    ProductPricetLBL.CssClass = "";
                    ProductPricetLBL.Text = Convert.ToString(originalPrice);
                }
                string lastErrorLableId = "ProductAmountError" + Convert.ToString(productId);
                Session["lastErrorLableId"] = lastErrorLableId;
            }
            else if (Convert.ToInt32(TB.Text) > 5)                                               //need to get a disscount
            {
                AlertLBL.Text = "<h6 class='alert'>if you'll buy up to 5 products you'll get discount of 10% off<h6>";
                Label ProductPricetLBL = (Label)cartPh.FindControl("ProductPrice" + Convert.ToString(productId));
                ProductPricetLBL.CssClass = "alert";

                if (ProductsInDiscount.ContainsKey(productId) == true)
                {
                    ProductPricetLBL.Text = Convert.ToString(ProductsInDiscount[productId] * 0.9);
                }
                else if (ProductsInDiscount.ContainsKey(productId) == false)
                {
                    double newPrice = Convert.ToInt32(ProductPricetLBL.Text) * 0.9;
                    int originalPrice = Convert.ToInt32(ProductPricetLBL.Text);
                    ProductPricetLBL.Text = Convert.ToString(newPrice);
                    ProductsInDiscount.Add(productId, originalPrice);
                }
            }
            else if (Convert.ToInt32(TB.Text) < 6)
            {
                AlertLBL.Text = "";
            }
        }
        Session["ProductsInDiscount"] = ProductsInDiscount;
        Label totalPriceLable = (Label)cartPh.FindControl("totalPriceLBL");
        totalPriceLable.Text = Convert.ToString("Total Price:" + calculateTotal());
        fromAmount = true;
    }


    double calculateTotal()
    {
        double total = 0;
        List<Product> checkedProductsList = (List<Product>)Session["checkedProductsList"];
        foreach (var product in checkedProductsList)
        {
            Label ProductPricetLBL = (Label)cartPh.FindControl("ProductPrice" + Convert.ToString(product.Id));
            TextBox amount = (TextBox)cartPh.FindControl("CartProductAmount" + Convert.ToString(product.Id));
            total += Convert.ToDouble(ProductPricetLBL.Text) * Convert.ToDouble(amount.Text);
        }
        return total;
    }


    void CheckedChangedFunc(object sender, EventArgs e)//update the list everytime uncheck product
    {

        CheckBox cb = sender as CheckBox;
        if (!cb.Checked)
        {
            int id = Convert.ToInt32(cb.ID.Replace("CartProductCheckBox", ""));
            Product currentProduct = null;
            if (Session["checkedProductsList"] != null)
            {
                List<Product> checkedProductsList = (List<Product>)Session["checkedProductsList"];
                if (checkedProductsList.Exists(product => product.Id == id))
                {
                    currentProduct = checkedProductsList.Find(p => p.Id == id);
                    checkedProductsList.Remove(currentProduct);
                    Session["checkedProductsList"] = checkedProductsList;
                    Response.Cookies["CartChanged"].Value = "1";
                    Response.Redirect("Cart.aspx");
                }
            }
        }
    }


    protected void CartSubmitButton_Click(object sender, EventArgs e)
    {
        totalPrice = calculateTotal();
        if (fromAmount == true)
        {
            return;
        }
        Dictionary<Product, int> CartProducts = new Dictionary<Product, int>();
        List<Product> checkedProductsList = (List<Product>)Session["checkedProductsList"];
        foreach (var product in checkedProductsList)
        {
            Product productObj = new Product();
            Label ProductPricetLBL = (Label)cartPh.FindControl("ProductPrice" + Convert.ToString(product.Id));
            TextBox amount = (TextBox)cartPh.FindControl("CartProductAmount" + Convert.ToString(product.Id));
            product.Price = Convert.ToDouble(ProductPricetLBL.Text);
            CartProducts.Add(product, Convert.ToInt32(amount.Text));
        }
        Session["CartProducts"] = CartProducts;
        Session["totalPrice"] = totalPrice;
        Response.Redirect("payment.aspx");
    }

}
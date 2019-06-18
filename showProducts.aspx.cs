using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class showProducts : System.Web.UI.Page
{
    double discount = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["logedInUser"] == null)//unsigned user
            {
                Response.Redirect("login.aspx");
            }
            if (Session["logedInUser"] != null)//signed user
            {
                User logedInUser = (User)Session["logedInUser"];
                if (logedInUser.Type == "user")//customer user
                {
                    if (Request.Cookies["BeenHere"] != null)//old customer
                    {
                        discount = 0.2;
                        Response.Cookies["BeenHere"].Value = "2";
                        WelcomeLBL.Text = "<h2 style='color:red;'>Welcome! we see you are an old customer:)</h2><br/>";
                    }
                    else//new customer
                    {
                        discount = 0.5;
                        Response.Cookies["BeenHere"].Value = "1";
                        Response.Cookies["BeenHere"].Expires = DateTime.Now.AddYears(1);
                        WelcomeLBL.Text = "<h2 style='color:red;'>Welcome! we see you are a new customer:)</h2><br/>";
                    }
                }
                else//administrator user
                    Response.Redirect("inventoryManagement.aspx");
            }
        }
        else
        {
            if (Request.Cookies["BeenHere"].Value == "1")
            {
                discount = 0.5;
            }
            else
                if (Request.Cookies["BeenHere"].Value == "2")
                {
                    discount = 0.2;
                }
        }
        initialPage();
    }


    protected void initialPage()
    {
        List<Product> ProductList = new List<Product>();
        Product prod = new Product();
        ProductList = prod.getProducts();
        foreach (var product in ProductList)
        {

                //create controllers
                Image ProductImage = new Image();
                Label ProductId = new Label();
                Label ProductTitle = new Label();
                Label ProductPrice = new Label();
                Label ProductInventory = new Label();
                Label ProductCategoryId = new Label();
                CheckBox ProductCheckBox = new CheckBox();

                //set controllers
                ProductCheckBox.ID = "ProductCheckBox" + Convert.ToString(product.Id);
                ProductCheckBox.Text = " Add to cart";
                if (Session["checkedProductsList"] != null)
                {
                    foreach (var ProductChecked in (List<Product>)Session["checkedProductsList"])
                    {
                        if (ProductChecked.Id == product.Id)
                        {
                            ProductCheckBox.Checked = true;
                        }
                    }
                }
                ProductId.Text = "  <br />" + "<span class='title'>Product Id: </span><span class='content'>" + Convert.ToString(product.Id) + "</span>  <br />";
                ProductTitle.Text = "<span class='title'>Product Name: </span><span class='content'>" + Convert.ToString(product.ProductName) + "</span>  <br />"; ;
                if (product.Id == 1)
                {
                    ProductPrice.Text = "<span class='title special'>Special Price: </span><span class='content special'>" + Convert.ToString(product.Price * (1 - discount)) + "</span> <br /> <span class='content special'>" + discount * 100 + "% OFF! </span> <br />";
                }
                else
                {
                    ProductPrice.Text = "<span class='title'>Price: </span><span class='content'>" + Convert.ToString(product.Price) + "</span>  <br />";
                }
                ProductInventory.Text = "<span class='title'>Inventory: </span><span class='content'>" + Convert.ToString(product.Inventory) + "</span>  <br />"; ;
                ProductCategoryId.Text = "<span class='title'>Category: </span><span class='content'>" + Convert.ToString(product.CategoryName) + "</span>  <br />";

                //check inventory
                ProductImage.ImageUrl = product.ImagePath;
                if (product.Inventory == 0)
                {
                    ProductCheckBox.Enabled = false;
                }

                //insert all info to page
                ph.Controls.Add(new LiteralControl("<div class='col-lg-3 col-md-6 col-xs-12'>"));
                ph.Controls.Add(new LiteralControl("<div class='jumbotron'>"));
                ph.Controls.Add(ProductImage);
                ph.Controls.Add(new LiteralControl("<br />"));
                ph.Controls.Add(ProductCheckBox);
                ph.Controls.Add(ProductId);
                ph.Controls.Add(ProductTitle);
                ph.Controls.Add(ProductPrice);
                ph.Controls.Add(ProductInventory);
                ph.Controls.Add(ProductCategoryId);
                ph.Controls.Add(new LiteralControl("</div> </div>"));

        }
    }








    /*protected List<Product> pullProductList()
    {
        DBservices dbs = new DBservices();
        try
        {
            dbs = dbs.searchItemsInDataBase("ProductsDBConnectionString", "select * from productN");
        }
        catch (Exception ex)
        {
            Response.Write("Unable to read from the database " + ex.Message);
            return null;
        }
        List<Product> productsList = new List<Product>();
        foreach (DataRow dr in dbs.dt.Rows)
        {
            if ((bool)dr["productN_status"] == false) continue;
            productsList.Add(new Product((string)dr["productN_name"], (double)dr["productN_price"], (string)dr["productN_category"], (int)dr["productN_inventory"], (string)dr["productN_imagePath"], (bool)dr["productN_status"], (int)dr["productN_id"]));
        }
        DataBind(); //must call this method in order to bind the data to the control and render the HTML

        return productsList;
    }*/




    protected void submitBTN_Click(object sender, EventArgs e)
    {
        List<Product> ProductList = new List<Product>();
        Product prod = new Product();
        ProductList = prod.getProducts();
        List<Product> CheckedProductsList = new List<Product>();
        foreach (var product in ProductList)
        {

            CheckBox cb = (CheckBox)ph.FindControl("ProductCheckBox" + Convert.ToString(product.Id));
            if (cb.Checked == true)
            {
                if (product.Id == 1)
                {
                    product.Price = product.Price * (1 - discount);
                }
                CheckedProductsList.Add(product);
            }
        }
        Session["checkedProductsList"] = CheckedProductsList;
        Response.Redirect("cart.aspx");
    }

}
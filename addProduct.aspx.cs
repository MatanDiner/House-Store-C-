using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class addProduct : System.Web.UI.Page
{




    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["logedInUser"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (Session["logedInUser"] != null)
            {
                User logedInUser = (User)Session["logedInUser"];
                if (logedInUser.Type == "administrator")
                {
                    initialCategoryList();
                }
                else
                    Response.Redirect("showProducts.aspx");
            }
        }

        messageLBL.Text = "";

    }








    private void initialCategoryList()
    {
        DBservices dbs = new DBservices();
        try
        {
            dbs = dbs.searchItemsInDataBase("ProductsDBConnectionString", "select Category_name from category");
        }
        catch (Exception ex)
        {
            Response.Write("Unable to read from the database " + ex.Message);
            return;
        }

        List<string> categoryList = new List<string>();
        foreach (DataRow dr in dbs.dt.Rows)
        {
            categoryList.Add((string)dr["Category_name"]);
        }
        productCategoryDDL.DataSource = categoryList;
        DataBind(); //must call this method in order to bind the data to the control and render the HTML
    }






    protected void check_oldProduct(object sender, EventArgs e)
    {
        if (productNameTB.Text == "") return;

        DBservices db = new DBservices();
        DBservices dbS = new DBservices();
        dbS = db.searchItemsInDataBase("ProductsDBConnectionString", "select * from productN");

        string productName = productNameTB.Text;
        bool flag_oldproduct = false;

        foreach (DataRow dr in dbS.dt.Rows)
        {
            if ((string)dr["productN_name"] == productName)
            {
                Labeloldprod.Text = "Editing an existing product";
                productPriceTB.Text = Convert.ToString(dr["productN_price"]);
                productCategoryDDL.SelectedValue = (string)dr["productN_category"];
                productInventoryTB.Text = Convert.ToString(dr["productN_inventory"]);
                productImg.ImageUrl = (string)dr["productN_imagePath"];
                bool productStatus = Convert.ToBoolean(dr["productN_status"]);
                if (productStatus == true)
                {
                    statusCB.Checked = true;
                }
                else
                {
                    statusCB.Checked = false;
                }
                Session["product_id"] = Convert.ToInt32(dr["productN_id"]);
                flag_oldproduct = true;
            }
        }
        if (flag_oldproduct == true)
        {
            Session["oldProduct"] = '1';
            Session["productsDataSet"] = dbS;
            RequiredFileUploadValidator.Enabled = false;
        }
        productImg.Visible = true;

    }





    protected void Submit_Click(object sender, EventArgs e)
    {

        Product product;
        Labeloldprod.Text = "";
        try
        {
            string productName = productNameTB.Text;
            double productPrice = Convert.ToDouble(productPriceTB.Text);
            string CategoryName = productCategoryDDL.SelectedValue;
            int productInventory = Convert.ToInt32(productInventoryTB.Text);
            string name = FU.FileName;
            string ProductFilePath = "";
            string SavedFileName = "";

            if ((name == "" && Session["oldProduct"] != null))
            {
                ProductFilePath = productImg.ImageUrl;
            }
            else
            {
                SavedFileName = Server.MapPath(".") + "/images/"+ name;
                ProductFilePath = "images/" + name;
                FU.SaveAs(SavedFileName);
            }


            bool productStatus;
            if (statusCB.Checked == true)
            {
                productStatus = true;
            }
            else
            {
                productStatus = false;
            }
            if (Session["oldProduct"] != null)
                product = new Product(productName, productPrice, CategoryName, productInventory, ProductFilePath, productStatus, (int)Session["product_id"]);
            else
                product = new Product(productName, productPrice, CategoryName, productInventory, ProductFilePath, productStatus);
        }
        catch (Exception ex)
        {
            messageLBL.Text = "invalid argoments inserted " + ex.Message;
            return;
        }
        try
        {
            if (Session["oldProduct"] != null)
            {
                product.updateTable();
                try
                {
                    product.updateDatabase();
                    Session["oldProduct"] = null;
                    messageLBL.Text = "you changed " + productNameTB.Text + " product successfully";
                }
                catch (Exception ex)
                {
                    Response.Write("Error updating the products database " + ex.Message);
                }
            }
            else
            {
                int numEffected = product.insertProduct();
                messageLBL.Text = "you added " + numEffected.ToString() + " new product";
            }
        }
        catch (Exception ex)
        {
            messageLBL.Text = "An error accured " + ex.Message;
        }

        productNameTB.Text = "";
        productPriceTB.Text = "";
        productInventoryTB.Text = "";
        statusCB.Checked = false;
        productImg.Visible = false;
        RequiredFileUploadValidator.Enabled = true;

    }



    protected void AddProductBTN_Click(object sender, EventArgs e)
    {
        productNameTB.Text = "";
        productPriceTB.Text = "";
        productInventoryTB.Text = "";
        statusCB.Checked = false;
        productImg.Visible = false;
        productImg.ImageUrl = "";
        RequiredFileUploadValidator.Enabled = true;
        Session["oldProduct"] = null;
        Labeloldprod.Text = "";
    }
}


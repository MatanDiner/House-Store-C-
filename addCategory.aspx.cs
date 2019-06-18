using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class _Default : System.Web.UI.Page
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
                if (logedInUser.Type == "user")
                    Response.Redirect("showProducts.aspx");
            }
        }
        
    }
    

          //-------------------------------------------------------------------------
    // Update the DataSet
    //-------------------------------------------------------------------------
    protected void Update_DataSet(){
       Category cg = new Category();
       cg.Name = TextBoxcategory.Text;
       cg.updateTable();
      
    }
    //-------------------------------------------------------------------------
    // update the database
    //-------------------------------------------------------------------------
    protected void update_DB()
    {
        Category cg = new Category();
        try
        {
            cg.updateDatabase();
        }
        catch (Exception ex)
        {
            Response.Write("Error updating the cars database " + ex.Message);
        }
    }





    protected void AddCategoryBTN_Click(object sender, EventArgs e)
    {

        if (TextBoxcategory.Text != "")
        {
            bool new_category_flag = true;
            Category cat = new Category();
            DataTable dt = cat.readCategorysDB(); // read from the DataBase
            foreach (DataRow dr in dt.Rows)
            {
                if ((string)dr["Category_name"] == TextBoxcategory.Text)
                {
                    Labelcategory.Text = "the category is already exist in the data base";
                    new_category_flag = false;
                }
            }
            if (new_category_flag == true)
            {
                Update_DataSet();
                update_DB();
                categoryGridView.DataBind();
                Labelcategory.Text = "new category was added to the data base";
            }
        }
        else
        {
            Labelcategory.Text = "you can't add a blank category name";
        }

    }

 
}
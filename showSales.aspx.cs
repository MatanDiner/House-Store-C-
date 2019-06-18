using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;


public partial class showSales : System.Web.UI.Page
{
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
                if (logedInUser.Type == "user")//customer user
                    Response.Redirect("showProducts.aspx");
                else
                {
                    initialPage();

                }
            }
        }
    }


    protected void initialPage()
    {

        DBservices DBservices = new DBservices();
        SqlConnection con = DBservices.connect("ProductsDBConnectionString");

        SqlCommand cmd = new SqlCommand("select * from category", con);
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);
        CategoryDropDownList.DataSource = dt;
        CategoryDropDownList.DataBind();

    }


    protected void CategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        filterTable(CategoryDropDownList.SelectedValue);  //filter by category option 2 

    }

    protected void filterTable(string category)
    {
        string selectquery = "select [Sale_id],[Sale_productId],[Sale_productTotalPrice],[Sale_amount],[Sale_userName],[Sale_date],[Sale_payment]" +
        "from [dbo].[Sale] inner join [dbo].[productN] on [Sale_productId]= [productN_id] inner join [dbo].[Category] on [Category_name] =[productN_category]" +
        "where [Category_name] = '" + category + "'";

        SaleDB.SelectCommand = selectquery;
        SaleGridView.DataBind();
    }





    protected void subStringDate(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int index = e.Row.Cells[5].Text.IndexOf(" ");
            e.Row.Cells[5].Text = e.Row.Cells[5].Text.Substring(0, index);
        }
    }
}




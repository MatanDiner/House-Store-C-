using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class inventoryManagement : System.Web.UI.Page
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
}
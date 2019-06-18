using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
        {
            UserName.Text = Convert.ToString(Request.Cookies["UserName"].Value);
            UserPassword.Text = Convert.ToString(Request.Cookies["Password"].Value);

        }
    }


    protected void submit_Click(object sender, EventArgs e)
    {
        User logedInUser = new User();
        DBservices Dbs = new DBservices();
        int result=0 ;
        result = Dbs.ChekeLogIn("ProductsDBConnectionString", "Users", UserName.Text, UserPassword.Text);
        switch (result)
        {
            case 0:
                LogIn_Massage.Visible = true; 
                LogIn_Massage.Text = "Error in user name and password";
                Session["logedInUser"] = null;
                break;

            case 1: //admin connect
                LogIn_Massage.Visible = true; 
                //LogIn_Massage.Text = UserName.Text + " connect successfully";
                logedInUser.Type="administrator";
                logedInUser.Name = UserName.Text;
                logedInUser.Password=UserPassword.Text;
                Session["logedInUser"] = logedInUser;
                if (rememberMeCB.Checked == true)
                {
                    Response.Cookies["UserName"].Value = UserName.Text;
                    Response.Cookies["Password"].Value = UserPassword.Text;
                    Response.Cookies["UserName"].Expires= DateTime.Now.AddYears(1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddYears(1);
                }
                Response.Redirect("inventoryManagement.aspx");
                break;

            case 2://customer connect
                LogIn_Massage.Visible = true;
                //LogIn_Massage.Text = UserName.Text + " connect successfully";
                logedInUser.Type="user";
                logedInUser.Name = UserName.Text;
                logedInUser.Password=UserPassword.Text;

                Session["logedInUser"] = logedInUser;
                if (rememberMeCB.Checked == true)
                {
                    Response.Cookies["UserName"].Value = UserName.Text;
                    Response.Cookies["Password"].Value = UserPassword.Text;
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddYears(1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddYears(1);
                }
                Response.Redirect("showProducts.aspx");
                break;

            case 3:
                LogIn_Massage.Visible = true; 
                LogIn_Massage.Text = "Error in password";
                Session["logedInUser"] = null;
                break;

            case 4:
                LogIn_Massage.Visible = true;
                LogIn_Massage.Text = "no such user name";
                Session["logedInUser"] = null;
                break;

            default:
                break;
        }

    }
}
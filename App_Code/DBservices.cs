using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {
        // read the connection string from the configuration file
        string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public int insertProduct(Product product)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("ProductsDBConnectionString"); // create the connection and open it
        }
        catch (Exception ex)
        {
            throw (ex);                               // write to log
        }
        String cStr = BuildInsertCommandProduct(product);    // helper method to build the insert string
        cmd = CreateCommand(cStr, con);               // create the command with all settings(query+ time to wait)
        try
        {
            int numEffected = cmd.ExecuteNonQuery();  // execute the command and bring back the number of effected rows
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();                         // close the db connection MUST!
            }
        }
    }
    //--------------------------------------------------------------------
    // Build the Insert a product command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandProduct(Product product)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}','{1}',{2}, {3},'{4}','{5}')", product.ProductName.ToString(), product.ImagePath.ToString(), product.Price.ToString(), product.Inventory.ToString(), product.Status.ToString(), product.CategoryName.ToString());
        String prefix = "INSERT INTO productN (productN_name  , productN_imagePath  , productN_price , productN_inventory , productN_status, productN_category)";
        command = prefix + sb.ToString();
        return command;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection connection)
    {
        SqlCommand cmd = new SqlCommand();                      // create the command object
        cmd.Connection = connection;                            // assign the connection to the command object
        cmd.CommandText = CommandSTR;                           // can be Select, Insert, Update, Delete 
        cmd.CommandTimeout = 10;                                // Time to wait for the execution' The default is 30 seconds
        cmd.CommandType = System.Data.CommandType.Text;         // the type of the command, can also be stored procedure

        return cmd;
    }

    //---------------------------------------------------------------------------------
    // update the dataset into the database
    //---------------------------------------------------------------------------------
    public void Update()
    {
        // the command build will automatically create insert/update/delete commands according to the select command
        SqlCommandBuilder builder = new SqlCommandBuilder(da);
        da.Update(dt);
    }


    //---------------------------------------------------------------------------------
    // check user name and password
    //---------------------------------------------------------------------------------
    public int ChekeLogIn(string conString, string tableName,string UserName,string Password)
    {
        int flag = 0; //flag 0 no such user name or password, flag 1 admin, flag 2 user, flag 3 wrong password, flag 4 no such user name
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file
            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end


            while (dr.Read())
            {   // Read till the end of the data into a row

                if (UserName == (string)dr["Users_name"] && Password == (string)dr["Users_Password"])
                {
                    if ((string)dr["Users_Type"] == "administrator") { 
                        return 1;
                    }
                    else return 2; 
                }
                else if (UserName == (string)dr["Users_name"] && Password != (string)dr["Users_Password"])
                {
                    return 3; 
                }

                else if (UserName != (string)dr["Users_name"] && Password == (string)dr["Users_Password"])
                {
                    return 4;

                }
            }
            return flag;

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // search specific item
    //---------------------------------------------------------------------------------
    public DBservices searchItemsInDataBase(string conString, string selectQuery)
    {
        DBservices dbS = new DBservices(); // create a helper class
        SqlConnection con = null;
        try
        {
            con = dbS.connect(conString); // open the connection to the database/
            SqlDataAdapter da = new SqlDataAdapter(selectQuery, con); // create the data adapter
            DataSet ds = new DataSet(); // create a DataSet and give it a name (not mandatory) as defualt it will be the same name as the DB
            da.Fill(ds);                        // Fill the datatable (in the dataset), using the Select command
            DataTable dt = ds.Tables[0];
            // add the datatable and the data adapter to the dbS helper class in order to be able to save it to a Session Object
            dbS.dt = dt;
            dbS.da = da;
            return dbS;
        }
        catch (Exception ex)
        {
            // write to log
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //----------------------------------------------------------------------------
    //CCEC-the first method that we have learned to insert into the data base
    //----------------------------------------------------------------------------
    public int insert(Category cat)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("ProductsDBConnectionString"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String cStr = BuildInsertCommand(cat);      // helper method to build the insert string
        cmd = CreateCommand(cStr, con);             // create the command
        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private String BuildInsertCommand(Category cat)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}')", cat.Name);
        String prefix = "INSERT INTO category " + "(Category_name) ";
        command = prefix + sb.ToString();
        return command;
    }


    //----------------------------------------------------------------------------
    // This method is adding data to a table by passing the parameters
    //----------------------------------------------------------------------------
    private void AddRow(DataTable dt, params String[] par)
    {
        DataRow dr = dt.NewRow(); // create a new row
        for (int i = 0; i < par.Length; i++)
        {
            dr[i] = par[i];      //fill the row with the data
        }
        dt.Rows.Add(dr);          // add the row to the table
    }

    public int insertSale(Sale sale)  //INSERT to Sales table 
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("ProductsDBConnectionString"); // create the connection and open it
        }
        catch (Exception ex)
        {
            throw (ex);                               // write to log
        }
        String cStr = BuildInsertCommandSales(sale);    // helper method to build the insert string
        cmd = CreateCommand(cStr, con);               // create the command with all settings(query+ time to wait)
        try
        {
            int numEffected = cmd.ExecuteNonQuery();  // execute the command and bring back the number of effected rows
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();                         // close the db connection MUST!
            }
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert a Sale command String
    //--------------------------------------------------------------------
    private String BuildInsertCommandSales(Sale sale)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        sb.AppendFormat(" Values ('{0}','{1}','{2}', '{3}','{4}','{5}')", sale.Product.Id.ToString(), sale.ProductTotalPrice.ToString(), sale.Amount.ToString(), sale.UserName.ToString(), sale.Date.ToString("yyyy-MM-dd"), sale.Payment.ToString());
        String prefix = "INSERT INTO Sale (Sale_productId  , Sale_productTotalPrice , Sale_amount, Sale_userName, Sale_date, Sale_payment )";
        command = prefix + sb.ToString();
        return command;

    }




    //--------------------------------------------------------------------
    // Execute the UPDATE Command ( inventory after sale )
    //--------------------------------------------------------------------

    public int updateInventory(int newInvetoryValue, int productID)  //INSERT to Sales table 
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("ProductsDBConnectionString"); // create the connection and open it
        }
        catch (Exception ex)
        {
            throw (ex);                               // write to log
        }
        String cStr = BuildUpdateInventoryCommand(newInvetoryValue, productID);    // helper method to build the insert string
        cmd = CreateCommand(cStr, con);               // create the command with all settings(query+ time to wait)
        try
        {
            int numEffected = cmd.ExecuteNonQuery();  // execute the command and bring back the number of effected rows
            return numEffected;
        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();                         // close the db connection MUST!
            }
        }
    }

    //--------------------------------------------------------------------
    // Build the UPDATE command  ( inventory after sale )
    //--------------------------------------------------------------------
    private String BuildUpdateInventoryCommand(int newInvetoryValue, int productID)
    {
        String command;

        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string

        sb.AppendFormat(" SET productN_inventory=('{0}') WHERE productN_id=('{1}')", newInvetoryValue.ToString(), productID.ToString());
        String prefix = "UPDATE productN";
        command = prefix + sb.ToString();
        return command;

    }
    public List<string> List_constractor(string conString, string tableName, string FieldName_1)
    {
        //int flag = 0; //flag 0 no such user name or password, flag 1 admin, flag 2 user, flag 3 wrong password, flag 4 no such user name
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            List<string> FieldName_1_list = new List<string>();
            while (dr.Read())
            {   // Read till the end of the data into a row
                FieldName_1_list.Add((string)dr[FieldName_1]);
            }
            return FieldName_1_list;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    public List<Product> List_constractor(string conString, string tableName)
    {
        //int flag = 0; //flag 0 no such user name or password, flag 1 admin, flag 2 user, flag 3 wrong password, flag 4 no such user name
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            List<Product> list = new List<Product>();
            while (dr.Read())
            {   // Read till the end of the data into a row
                if((bool)dr["productN_status"]==true){
                    Product prod = new Product((string)dr["productN_name"], Convert.ToDouble(dr["productN_price"]), (string)dr["productN_category"], Convert.ToInt32(dr["productN_inventory"]), (string)dr["productN_imagePath"], Convert.ToInt32(dr["productN_id"]));
                    list.Add(prod);
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }





    public List<Category> List_Category_constractor(string conString, string tableName, List<Category> cat)
    {
        //int flag = 0; //flag 0 no such user name or password, flag 1 admin, flag 2 user, flag 3 wrong password, flag 4 no such user name
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            List<Category> categoryList = new List<Category>();
            bool found_in_cat = false;
                while (dr.Read())
                {   // Read till the end of the data into a row
                    foreach (Category category in cat)
                    {
                        if (found_in_cat == true)
                        {
                            found_in_cat = false;
                            break;
                        }
                    if ((string)(dr["Category_name"]) == category.Name)
                    {
                        Category cg = new Category((string)(dr["Category_name"]), Convert.ToInt32(dr["Category_id"]));
                        categoryList.Add(cg);
                        found_in_cat = true;
                    }
                 }
            }
                return categoryList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
 
    }


    public string getcategoryName(string conString, string tableName,int categoryId)
    {
        //int flag = 0; //flag 0 no such user name or password, flag 1 admin, flag 2 user, flag 3 wrong password, flag 4 no such user name
        SqlConnection con = null;
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
            string name=null;
            while (dr.Read())
            {   // Read till the end of the data into a row
                if (Convert.ToInt32(dr["Category_id"]) == categoryId)
                {
                  name = (string)dr["Category_name"];
                }
            }
            return name;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }
}

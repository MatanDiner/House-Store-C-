using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Category
{
	public Category()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int id;

    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public Category(string _name)
    {
        name = _name;
    }
    public Category(string _name,int _id)
    {
        name = _name;
        id = _id;
    }

    public List<Category> getCategory()
    {
        Product prod=new Product();
        List<Product> productList = prod.getProducts();
        List<Category> CategoryList = new List<Category>();
        foreach (Product product in productList)
        {
            bool exist_in_CategoryList = false;
            foreach (Category category in CategoryList)
            {
                if (product.CategoryName == category.Name)
                {
                    exist_in_CategoryList=true;
                }
            }
            if (exist_in_CategoryList == false)
            {
                Category cat = new Category(product.CategoryName);
                CategoryList.Add(cat);
            }
        }
        DBservices bd=new DBservices();
        return bd.List_Category_constractor("ProductsDBConnectionString", "Category", CategoryList);
    }


    public List<Product> getProductsByCat(int categoryId)
    {

        DBservices db=new DBservices();
        string categoryName=db.getcategoryName("ProductsDBConnectionString","Category",categoryId);
        Product product = new Product();
        List<Product> productlist = product.getProducts();
        List<Product> products_of_category = new List<Product>();
        foreach (Product prod in productlist)
        {
            if (categoryName == prod.CategoryName)
            {
                products_of_category.Add(prod);
            }
        }
        return products_of_category;

    }
    //----------------------------------------------------
    //second method:dataAdapter
    //-----------------------------------------------------
    public DataTable readCategorysDB()
    {
        DBservices dbs = new DBservices();
        dbs = dbs.searchItemsInDataBase("ProductsDBConnectionString", "SELECT * FROM category");
        // save the dataset in a session object
        HttpContext.Current.Session["categoryDataSet"] = dbs;
        return dbs.dt;
    }


    //------------------------------------------------------------------------
    // update the dataset with a new car record
    //------------------------------------------------------------------------
    public void updateTable()
    {
        if (HttpContext.Current.Session["categoryDataSet"] == null) return;
        DBservices dbs = (DBservices)HttpContext.Current.Session["categoryDataSet"];
        DataRow dr = dbs.dt.NewRow();
        dr["Category_name"] = name;
        dbs.dt.Rows.Add(dr);

    }
    //---------------------------------------------------------------------------------
    // update the database
    //---------------------------------------------------------------------------------
    public void updateDatabase()
    {
        if (HttpContext.Current.Session["categoryDataSet"] == null) return;
        DBservices dbs = (DBservices)HttpContext.Current.Session["categoryDataSet"];
        dbs.Update();
    }
}
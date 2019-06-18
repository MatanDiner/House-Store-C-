using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
/// <summary>
/// Summary description for Product
/// </summary>
public class Product
{
	public Product()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    int id;
    string categoryName;
    string productName;
    string imagePath;
    double price;
    int inventory;
    bool status;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string ProductName
    {
        get { return productName; }
        set { productName = value; }
    }
    public bool Status
    {
        get { return status; }
        set { status = value; }
    }

    public string CategoryName
    {
        get { return categoryName; }
        set { categoryName = value; }
    }

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }

    public double Price
    {
        get { return price; }
        set { price = value; }
    }

    public int Inventory
    {
        get { return inventory; }
        set { inventory = value; }
    }


    public Product getProduct(int productId){

        Product prod=new Product();
        List<Product> productsList=prod.getProducts();
        Product product=null;
        foreach (Product pro in productsList)
        {
            if(pro.Id==productId){

               product = new Product(pro.ProductName, pro.Price, pro.CategoryName,pro.Inventory, pro.ImagePath, pro.Id);
            }
        }
        return product;
    }


    public Product(string _productName, double _price, string _categoryName, int _inventory, string _imagePath, bool _status)
    {
        ProductName = _productName;
        Price = _price;
        CategoryName = _categoryName;
        Inventory = _inventory;
        ImagePath = _imagePath;
        Status=_status;
    }
    public Product(string _productName, double _price, string _categoryName, int _inventory, string _imagePath, bool _status, int _id)
    {
        ProductName = _productName;
        Price = _price;
        CategoryName = _categoryName;
        Inventory = _inventory;
        ImagePath = _imagePath;
        Status = _status;
        Id = _id;
    }
    public Product(string _productName, double _price, string _categoryName, int _inventory, string _imagePath, int _id)
    {
        ProductName = _productName;
        Price = _price;
        CategoryName = _categoryName;
        Inventory = _inventory;
        ImagePath = _imagePath;
        Id = _id;
    }
    public int insertProduct()
    {
        DBservices db = new DBservices();
        int numAffected = db.insertProduct(this);
        return numAffected;

    }
    //----------------------------------------
    //adapter-bonus
    //-----------------------------------------
    public void updateTable()
    {

        if (HttpContext.Current.Session["productsDataSet"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["productsDataSet"];

        foreach (DataRow dr in dbs.dt.Rows)
        {
            if (Convert.ToInt32(dr["productN_id"]) == id)
            {
                dr["productN_name"] = productName;
                dr["productN_imagePath"] = imagePath;
                dr["productN_price"] = price;
                dr["productN_inventory"] = inventory;
                dr["productN_status"] = status;
                dr["productN_category"] = categoryName;
            }
        }

    }
    //---------------------------------------------------------------------------------
    // update the database-bonus
    //---------------------------------------------------------------------------------
    public void updateDatabase()
    {

        if (HttpContext.Current.Session["productsDataSet"] == null) return;

        DBservices dbs = (DBservices)HttpContext.Current.Session["productsDataSet"];

        dbs.Update();

    }
    public List<Product> getProducts()
    {
        DBservices db = new DBservices();
        List<Product> productList = db.List_constractor("ProductsDBConnectionString", "productN");
        ESproxy.WebService ES = new ESproxy.WebService();
        ESproxy.Product[] ESarr = ES.getProducts();
        foreach (ESproxy.Product ESprod in ESarr)
        {
            Product product = new Product(ESprod.ProductName, ESprod.Price, ESprod.CategoryName, ESprod.Inventory, ESprod.ImagePath, ESprod.Id);
            productList.Add(product);
        }
        return productList;
    }
}
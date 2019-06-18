using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Sale
/// </summary>
public class Sale
{
	public Sale()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    Product product;
    float productTotalPrice;
    int amount;
    string userName;
    DateTime date;
    string payment;


    public Product Product
    {
        get
        {
            return product;
        }

        set
        {
            product = value;
        }
    }


    public float ProductTotalPrice
    {
        get
        {
            return productTotalPrice;
        }

        set
        {
            productTotalPrice = value;
        }
    }


    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public string UserName
    {
        get
        {
            return userName;
        }

        set
        {
            userName = value;
        }
    }


    public DateTime Date
    {
        get
        {
            return date;
        }

        set
        {
            date = value;
        }
    }


    public string Payment
    {
        get
        {
            return payment;
        }

        set
        {
            payment = value;
        }
    }

}
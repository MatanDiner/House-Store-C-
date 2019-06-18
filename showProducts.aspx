<%@ Page Title="" Language="C#" MasterPageFile="~/UserMasterPage.master" AutoEventWireup="true" CodeFile="showProducts.aspx.cs" Inherits="showProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #showProducts {
            background-color: black;
        }

            #showProducts a {
                color: white;
            }

        .special {
            color: red;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="WelcomeLBL" runat="server" Text=""></asp:Label>
    <asp:PlaceHolder ID="ph" runat="server"></asp:PlaceHolder>
    <br />
    <asp:Button ID="submitBTN" runat="server" Text="Submit and add to cart" OnClick="submitBTN_Click" />
</asp:Content>


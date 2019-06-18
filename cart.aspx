<%@ Page Title="" Language="C#" MasterPageFile="~/UserMasterPage.master" AutoEventWireup="true" CodeFile="cart.aspx.cs" Inherits="cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #cart {
            background-color: black;
        }

        #cart a {
            color: white;
        }

        h2 {
            color: white;
        }
        .alert {
        color:red;
        }
        img {
        max-height:130px;
        }
        .total {
        font-size:x-large;
        color:white;
        }
        input#ContentPlaceHolder1_CartSubmitButton {
        width:100%;
        margin-bottom:30px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="CartMessageArea" runat="server" Text=""></asp:Label>
    <asp:Label ID="tatalCartPrice" runat="server" Text=""></asp:Label>
    <asp:PlaceHolder ID="cartPh" runat="server"></asp:PlaceHolder>
    <div class="col-md-12">
    <asp:Button ID="CartSubmitButton" runat="server" Text="Submit & pay" onkeydown = "return (event.keyCode!=13);" OnClick="CartSubmitButton_Click" />
    </div>
<%--     --%>
</asp:Content>


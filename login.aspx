<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.3/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/css/LoginStyleSheet.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server" class="login-form">
        <div class="Logo-Title-Holder">
            <img src="assets/images/LogoBlack.PNG" />
        </div>
        <p class="login-text">
            <span class="fa-stack fa-lg">
                <i class="fa fa-circle fa-stack-2x"></i>
                <i class="fa fa-lock fa-stack-1x"></i>
            </span>
        </p>
        <div>

            <h1 class="font-white">Login</h1>

            <asp:TextBox ID="UserName" runat="server" class="login-username" autofocus="true" required="true" placeholder="User Name"></asp:TextBox>
            <asp:TextBox ID="UserPassword" runat="server" class="login-password" required="true" placeholder="Password" type="password"> </asp:TextBox>

            <p class="font-white">
                Remember me 
             <asp:CheckBox ID="rememberMeCB" runat="server" />
            </p>
            <asp:Button ID="submit" runat="server" Text="Log-in" OnClick="submit_Click" class="login-submit" />

            <asp:Label ID="LogIn_Massage" runat="server" Text="" Visible="false" Style="color: #FF0000"></asp:Label>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>

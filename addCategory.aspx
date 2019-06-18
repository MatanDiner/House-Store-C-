<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="addCategory.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #addCategory {
            background-color: black;
        }

         #addCategory a {
            color: white;
        }
        .center {
        width:100%;
        text-align:center;
        }
   
          .tableHolder {
        width:100%;
            text-align:center;
            margin-bottom:50px;
        }

        td, th {
            width: 150px;
            text-align: center;
        }

          table#ContentPlaceHolder1_categoryGridView{
              margin:0 auto;
              margin-top: 15px;

          }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="center">
    <div >
    <asp:TextBox ID="TextBoxcategory" runat="server"  ></asp:TextBox>
        <asp:Button ID="AddCategoryBTN" runat="server" Text="Add category" OnClick="AddCategoryBTN_Click" />
   <br />
         <asp:Label ID="Labelcategory" runat="server" Text="" style="color:red"></asp:Label>
</div>
    <div class="tableHolder col-md-12">

            <asp:SqlDataSource ID="categoryDB" runat="server" ConnectionString="<%$ ConnectionStrings:ProductsDBConnectionString %>" SelectCommand="SELECT * FROM [Category]"></asp:SqlDataSource>

            <asp:GridView ID="categoryGridView" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="Category_name" DataSourceID="categoryDB" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Category_id" HeaderText="Category ID" InsertVisible="False" ReadOnly="True" SortExpression="Category_id" />
                    <asp:BoundField DataField="Category_name" HeaderText="Category Name" ReadOnly="True" SortExpression="Category_name" />
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>

    </div>
        </div>
</asp:Content>


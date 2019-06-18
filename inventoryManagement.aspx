<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMasterPage.master" AutoEventWireup="true" CodeFile="inventoryManagement.aspx.cs" Inherits="inventoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #inventoryManagement {
            background-color: black;
        }

            #inventoryManagement a {
                color: white;
            }

        .icons {
            height: 40px;
        }
        td,th {
        width:100px;
        text-align:center;
        }
        .tableHolder {
        width:100%;
            text-align:center;
            margin-bottom:50px;
        }
        table#ContentPlaceHolder1_GridView1 {
    margin: 0 auto;
}
        .tableHolder h2 {
        color:white;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="tableHolder col-md-12">
        <h2> Inventory managment</h2>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:ProductsDBConnectionString %>" DeleteCommand="DELETE FROM [productN] WHERE [productN_id] = @original_productN_id AND [productN_name] = @original_productN_name AND [productN_imagePath] = @original_productN_imagePath AND [productN_price] = @original_productN_price AND [productN_inventory] = @original_productN_inventory AND [productN_status] = @original_productN_status AND [productN_category] = @original_productN_category" InsertCommand="INSERT INTO [productN] ([productN_name], [productN_imagePath], [productN_price], [productN_inventory], [productN_status], [productN_category]) VALUES (@productN_name, @productN_imagePath, @productN_price, @productN_inventory, @productN_status, @productN_category)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [productN]" UpdateCommand="UPDATE [productN] SET [productN_inventory] = @productN_inventory, [productN_status] = @productN_status WHERE [productN_id] = @original_productN_id AND [productN_name] = @original_productN_name AND [productN_imagePath] = @original_productN_imagePath AND [productN_price] = @original_productN_price AND [productN_inventory] = @original_productN_inventory AND [productN_status] = @original_productN_status AND [productN_category] = @original_productN_category">
        <DeleteParameters>
            <asp:Parameter Name="original_productN_id" Type="Int32" />
            <asp:Parameter Name="original_productN_name" Type="String" />
            <asp:Parameter Name="original_productN_imagePath" Type="String" />
            <asp:Parameter Name="original_productN_price" Type="Double" />
            <asp:Parameter Name="original_productN_inventory" Type="Int32" />
            <asp:Parameter Name="original_productN_status" Type="Boolean" />
            <asp:Parameter Name="original_productN_category" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="productN_name" Type="String" />
            <asp:Parameter Name="productN_imagePath" Type="String" />
            <asp:Parameter Name="productN_price" Type="Double" />
            <asp:Parameter Name="productN_inventory" Type="Int32" />
            <asp:Parameter Name="productN_status" Type="Boolean" />
            <asp:Parameter Name="productN_category" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="productN_name" Type="String" />
            <asp:Parameter Name="productN_imagePath" Type="String" />
            <asp:Parameter Name="productN_price" Type="Double" />
            <asp:Parameter Name="productN_inventory" Type="Int32" />
            <asp:Parameter Name="productN_status" Type="Boolean" />
            <asp:Parameter Name="productN_category" Type="String" />
            <asp:Parameter Name="original_productN_id" Type="Int32" />
            <asp:Parameter Name="original_productN_name" Type="String" />
            <asp:Parameter Name="original_productN_imagePath" Type="String" />
            <asp:Parameter Name="original_productN_price" Type="Double" />
            <asp:Parameter Name="original_productN_inventory" Type="Int32" />
            <asp:Parameter Name="original_productN_status" Type="Boolean" />
            <asp:Parameter Name="original_productN_category" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="productN_id" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Save" OnClientClick="return confirm('Are you sure edit?');" CausesValidation="True" CommandName="Update" Text="Update"><img class="icons" src="assets/images/save.png" /></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Cancel" CausesValidation="False" CommandName="Cancel" Text="Cancel"><img class="icons" src="assets/images/cancel.png" /></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit"> <img class="icons" src="assets/images/edit.png" /></asp:LinkButton>
                </ItemTemplate>
                <ControlStyle Width="70px" />
                <HeaderStyle Width="70px" />
            </asp:TemplateField>
            <asp:BoundField DataField="productN_id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="productN_id">
                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="productN_name" HeaderText="Name" ReadOnly="True" SortExpression="productN_name">
                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="productN_price" HeaderText="Price" ReadOnly="True" SortExpression="productN_price">
                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Inventory" SortExpression="productN_inventory">
                <EditItemTemplate>
                    <asp:TextBox ID="inventoryTB" runat="server" Text='<%# Bind("productN_inventory") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valInventory" runat="server" ControlToValidate="inventoryTB"
                        ErrorMessage="Required!" Style="color: #FF0000"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Numbers Only!"
                        ControlToValidate="inventoryTB" ValidationExpression="^[0-9]*$" Style="color: #FF0000"></asp:RegularExpressionValidator>
                                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("productN_inventory") %>'></asp:Label>
                </ItemTemplate>
                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:TemplateField>
            <asp:CheckBoxField DataField="productN_status" HeaderText="Status" SortExpression="productN_status">
                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:CheckBoxField>
            <asp:BoundField DataField="productN_category" HeaderText="Category" SortExpression="productN_category" ReadOnly="True">
                <ControlStyle Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:BoundField>
            <asp:ImageField DataImageUrlField="productN_imagePath" HeaderText="Image" ReadOnly="True">
                <ControlStyle Height="100px" Width="100px" />
                <HeaderStyle Width="100px" />
            </asp:ImageField>
        </Columns>
        <EditRowStyle BackColor="#1d1f29" ForeColor="White"/>
        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"  />
        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#E3EAEB" />
        <SelectedRowStyle BackColor="#96adb5" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F8FAFA" />
        <SortedAscendingHeaderStyle BackColor="#246B61" />
        <SortedDescendingCellStyle BackColor="#D4DFE1" />
        <SortedDescendingHeaderStyle BackColor="#15524A" />
    </asp:GridView>

</div>
</asp:Content>
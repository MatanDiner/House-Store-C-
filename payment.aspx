<%@ Page Title="" Language="C#" MasterPageFile="~/UserMasterPage.master" AutoEventWireup="true" CodeFile="payment.aspx.cs" Inherits="payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style>
        #Payment a {
            color: white;
            background-color: black;
        }

        #imagePlace {
            text-align: center;
        }

            #imagePlace img {
                border: solid 1px black;
                max-height: 150px;
            }

        #payment {
            background-color: black;
        }

            #payment a {
                color: white;
            }

        h2 {
            color: red;
        }
    </style>
    <script>

        //check ID number
        function validateId(oSrc, args) {
            try {
                if (args.Value.length == 9) {
                    id = parseInt(args.Value);
                    sum = 0;
                    LastNumber = id % 10;
                    for (var i = 0; i < 9; i++) {
                        sum = sum + id % 10;
                        id = parseInt(id / 10);
                    }
                    if (parseInt((sum / 7)) % 10 == LastNumber) args.IsValid = true;
                    else args.IsValid = false;
                }
                else args.IsValid = false;
            }
            catch (Exception) {
                args.IsValid = false;
            }
        }

        //check that one of the radio button is selected
        function checkRadioButtons(oSrc, args) {
            phone = document.getElementById('<%=PhonePayment_RB.ClientID%>');
            credit = document.getElementById('<%=creditPayment_RB.ClientID%>');

            args.IsValid = phone.checked || credit.checked;

        }

    </script>




</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class='PaymentForm'>
        <h1 class="Headers">טופס תשלומים</h1>
        <asp:Label ID="priceSuccsesMassege" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="TotalPriceLBL" runat="server" Text="Label"></asp:Label>
        <table>
            <tr>
                <td>Name:</td>
                <td>
                    <asp:TextBox ID="Name_TB" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredNameValidator"
                        ControlToValidate="Name_TB" runat="server"
                        ErrorMessage="Please Enter a Name" Style="color: #FF0000">
                    </asp:RequiredFieldValidator>
                    <br />
                </td>
            </tr>
            <tr>
                <td>Address</td>
                <td>
                    <asp:TextBox ID="Address_TB" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredAddressValidator"
                        ControlToValidate="Address_TB" runat="server"
                        ErrorMessage="Please Enter a Address" Style="color: #FF0000">
                    </asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Email</td>
                <td>
                    <asp:TextBox ID="Email_TB" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RegularExpressionValidator ID="RegularExpressionEmailValidator"
                        ControlToValidate="Email_TB" runat="server"
                        ErrorMessage="Please enter a valid e-mail address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        Style="color: #FF0000"></asp:RegularExpressionValidator>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredEmailValidator"
                        ControlToValidate="Email_TB" runat="server"
                        ErrorMessage="Please Enter a Email" Style="color: #FF0000">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Shippment Date</td>
                <td>
                    <asp:Calendar ID="ShipDate_CLD" runat="server" OnSelectionChanged="checkCalender"></asp:Calendar>
                </td>
                <td>
                    <asp:Label ID="ShipDateValidator" runat="server" Style="color: #FF0000"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Phone payment:<asp:RadioButton ID="PhonePayment_RB" runat="server" GroupName="PaymentOption"
                    AutoPostBack="true" OnCheckedChanged="PhonePayment_RB_CheckedChanged" />
                </td>
                <td>Credit payment:<asp:RadioButton ID="creditPayment_RB" runat="server" AutoPostBack="true"
                    GroupName="PaymentOption" OnCheckedChanged="creditPayment_RB_CheckedChanged" />
                </td>
                <td>
                    <asp:CustomValidator ID="CustomPaymentValidator" runat="server" Display="Dynamic" ClientValidationFunction="checkRadioButtons"
                        ErrorMessage="please choose payment option" Style="color: #FF0000">
                    </asp:CustomValidator>
                </td>
            </tr>
        </table>
        <asp:PlaceHolder ID="PhonePaymentPlaceHolder" Visible="false" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="TelephoneNumber_LBL" runat="server" Text="Telephone Number:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TelephoneNumber_TB" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredTelephoneNumberValidator"
                            ControlToValidate="TelephoneNumber_TB" runat="server"
                            ErrorMessage="Please Enter Telephone number" Style="color: #FF0000">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularPhoneExpressionValidator"
                            ControlToValidate="TelephoneNumber_TB" runat="server"
                            ErrorMessage="Please enter a valid Telephone number"
                            Style="color: #FF0000" ValidationExpression="^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$">
                        </asp:RegularExpressionValidator>
                    </td>
            </table>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="creditPaymentPlaceHolder" Visible="false" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="PaymentsNumber_LBL" runat="server" Text="Number of Payments:">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="PaymentsNumber_DDL" runat="server">
                            <asp:ListItem>Select number of payments</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredPaymentsNumberValidator"
                            ControlToValidate="PaymentsNumber_DDL" runat="server" InitialValue="Select number of payments"
                            ErrorMessage="Please Enter number of payments" Style="color: #FF0000">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="CardNumber_LBL" runat="server" Text="Card Number:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="CardNumber_TB" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredCardNumberValidator"
                            ControlToValidate="CardNumber_TB" runat="server"
                            ErrorMessage="Please Enter card number" Style="color: #FF0000">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularCardNumberExpressionValidator"
                            ControlToValidate="CardNumber_TB" runat="server"
                            ErrorMessage="Please enter a valid card number" Style="color: #FF0000"
                            ValidationExpression="^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ID_LBL" runat="server" Text="ID Number:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="ID_TB" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredIdValidator"
                            ControlToValidate="ID_TB" runat="server"
                            ErrorMessage="Please Enter ID number" Style="color: #FF0000">
                        </asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomIDValidator" ControlToValidate="ID_TB" runat="server"
                            ClientValidationFunction="validateId" ErrorMessage="incorrect ID" Style="color: #FF0000"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="CreditType_LBL" runat="server" Text="Credit Type:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="CreditType_DDL" runat="server">
                            <asp:ListItem>Select Credit Type</asp:ListItem>
                            <asp:ListItem>mastercard</asp:ListItem>
                            <asp:ListItem>visa</asp:ListItem>
                            <asp:ListItem>isracard</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredCreditTypeValidator"
                            ControlToValidate="CreditType_DDL" runat="server" InitialValue="Select Credit Type"
                            ErrorMessage="Please Enter Credit Type" Style="color: #FF0000">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
        <table>
            <tr>
                <td>choose signature file path: </td>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" />
                </td>
                <td>
                                        <asp:RequiredFieldValidator ID="RequiredFileUploadValidator" ControlToValidate="FileUpload" runat="server" ErrorMessage="Must upload signature!" Style="color: #FF0000"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td id="imagePlace" colspan="2">
                    <asp:Image ID="signatureImage" runat="server" />
                    <asp:Label ID="signatureLable" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Button ID="SubmitButton" runat="server" Text="Submit" OnClick="Payment_Submit_Click" />
    </div>
</asp:Content>


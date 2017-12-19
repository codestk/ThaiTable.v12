

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body id="BodyTag">
    <form id="form1">
    <div style="width:800px;margin:auto">
                <div class="ContentRight">
                    <div class="ContentHeader">
                        Order Details
                    </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Amount:</div>
	                    <div class="FormInputTextOnly">10.00 GBP</div>
                        <asp:HiddenField ID="hfAmount" Value="1000" />
                        <asp:HiddenField ID="hfCurrencyISOCode" Value="826" />
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Order Description:</div>
	                    <div class="FormInputTextOnly">A Test Order</div>
                        <asp:HiddenField ID="hfOrderID" Value="Order-1234" />
                        <asp:HiddenField ID="hfOrderDescription" Value="A Test Order" />
	                </div>
	                <div class="ContentRight">
	                    <div class="ContentHeader">
	                        Card Details
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Name On Card:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbCardName" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Card Number:</div>
	                    <div class="FormInput">            
	                        <asp:TextBox ID="tbCardNumber" CssClass="InputTextField" MaxLength="20" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">
	                        Expiry Date:
	                    </div>
	                    <div class="FormInput">
	                        <asp:DropDownList ID="ddExpiryDateMonth" Width="45px">
	                        </asp:DropDownList>
	                        /
	                        <asp:DropDownList ID="ddExpiryDateYear" Width="55px">
	                        </asp:DropDownList>
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">
	                        Start Date:
	                    </div>
	                    <div class="FormInput">
	                        <asp:DropDownList ID="ddStartDateMonth" Width="45px">
	                        </asp:DropDownList>
	                        /
	                        <asp:DropDownList ID="ddStartDateYear" Width="55px">
	                        </asp:DropDownList>
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Issue Number:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbIssueNumber" CssClass="InputTextField" MaxLength="2" Width="50px" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">CV2:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbCV2" CssClass="InputTextField" MaxLength="4" Width="50px" />
	                    </div>
	                </div>
                </div>
                
                <div class="ContentRight">
                    <div class="ContentHeader">
                        Customer Details
                    </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Address:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbAddress1" CssClass="InputTextField" MaxLength="100" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">&nbsp</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbAddress2" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">&nbsp</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbAddress3" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">&nbsp</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbAddress4" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">City:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbCity" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">State:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbState" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">Post Code:</div>
	                    <div class="FormInput">
	                        <asp:TextBox ID="tbPostCode" CssClass="InputTextField" MaxLength="50" />
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormLabel">
	                        Country:
	                    </div>
	                    <div class="FormInput">
	                        <asp:DropDownList ID="ddCountries" Width="200px">
	                            <asp:ListItem></asp:ListItem>
	                        </asp:DropDownList>
	                    </div>
	                </div>
	                <div class="FormItem">
	                    <div class="FormSubmit">
	                        <asp:Button ID="SubmitButton" 
	                            Text="Submit For Processing" 
	                            OnClick="SubmitButton_Click" />
	                    </div>
	                </div>
                </div>
    </div>
    </form>
</body>
</html>

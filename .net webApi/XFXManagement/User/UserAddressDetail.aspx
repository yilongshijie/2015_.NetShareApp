<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserAddressDetail.aspx.cs" Inherits="XFXManagement.UserAddressDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="UserAddressID" HeaderText="用户地址">
        <Fields>

            <asp:BoundField DataField="Tel" HeaderText="联系电话" SortExpression="Tel" />
            <asp:BoundField DataField="ProvincialCityAddress" HeaderText="省市区" SortExpression="ProvincialCityAddress" />
            <asp:BoundField DataField="AddressInfo" HeaderText="详细地址" SortExpression="AddressInfo" />
            <asp:BoundField DataField="Person" HeaderText="联系人" SortExpression="Person" />
        </Fields>
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />


    </asp:DetailsView>

</asp:Content>

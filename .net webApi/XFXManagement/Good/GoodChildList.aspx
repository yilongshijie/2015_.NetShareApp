<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodChildList.aspx.cs" Inherits="XFXManagement.GoodChildList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="GoodChildID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="GoodChildID" HeaderText="子商品ID" ReadOnly="True" SortExpression="GoodChildID" />

            <asp:BoundField DataField="Repertory" HeaderText="库存" SortExpression="Repertory" />
            <asp:BoundField DataField="Specification" HeaderText="规格" SortExpression="Specification" />
            <asp:BoundField DataField="AddPrice" HeaderText="金额变动" SortExpression="AddPrice" />
            <asp:BoundField DataField="SalesVolume" HeaderText="销量" SortExpression="SalesVolume" />
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# Eval("State").ToString()=="1"?"上架":"下架" %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="图片">
                <ItemTemplate>
                    <img src="<%# ConfigurationManager.AppSettings["UploadUrl"]+Eval("Image") %>" style="width: 4em; height: 4em;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderBy" HeaderText="优先级" SortExpression="OrderBy" />

            <asp:CommandField ShowSelectButton="True" />
        </Columns>
        <EmptyDataTemplate>
            <ul class="states">
                <li class="warning">没有数据(当商品至少有一个自商品的时候才能上架)</li>
            </ul>
        </EmptyDataTemplate>

        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

    </asp:GridView>

    <input type="submit" id="Add" value="添加" runat="server" onserverclick="Add_Click" />
    <br />
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WholeFieldActivityList.aspx.cs" Inherits="XFXManagement.WholeFieldActivityList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="WholeFieldActivityID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="WholeFieldActivityID" HeaderText="活动ID" InsertVisible="False" ReadOnly="True" SortExpression="WholeFieldActivityID" />
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" />
            <asp:BoundField DataField="FillPrice" HeaderText="满" SortExpression="FillPrice" />
            <asp:BoundField DataField="DiscountPrice" HeaderText="减" SortExpression="DiscountPrice" />
            <asp:TemplateField HeaderText="类型">
                <ItemTemplate>
                    <%# Eval("Type").ToString()=="0"? "满减":"包邮" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# Eval("State").ToString()=="0"? "停用":"使用" %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowSelectButton="True" />

        </Columns>
        <EmptyDataTemplate>
            <ul class="states">
                <li class="warning">没有数据</li>
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

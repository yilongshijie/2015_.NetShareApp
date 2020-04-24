<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserSMSList.aspx.cs" Inherits="XFXManagement.UserSMSList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="filter">
        <div class="row">

            <label>
                手机号:
                    <input type="text" id="Tel" runat="server" />
            </label>

        </div>
        <div class="row">
            <input type="submit" id="Submit" value="查询" runat="server" onserverclick="Submit_Click" />
        </div>
    </div>


    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="UserSMSID" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>
            <asp:BoundField DataField="UserSMSID" HeaderText="短信ID" InsertVisible="False" ReadOnly="True" SortExpression="UserSMSID" />
            <asp:BoundField DataField="Tel" HeaderText="电话" SortExpression="Tel" />
            <asp:BoundField DataField="Yanzhengma" HeaderText="验证码" SortExpression="Yanzhengma" />
            <asp:TemplateField HeaderText="状态" SortExpression="State">
                <ItemTemplate>
                    <%# Eval("State").ToString()=="0"?"未使用":"已使用"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SentTime" HeaderText="发送时间" SortExpression="SentTime" />
            <asp:BoundField DataField="Ip" HeaderText="Ip" SortExpression="Ip" />
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

    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" CssClass="anpager" CurrentPageButtonClass="cpb" PagingButtonSpacing="0"
        OnPageChanged="AspNetPager1_PageChanged" CurrentPageButtonPosition="Center"
        HorizontalAlign="center" AlwaysShowFirstLastPageNumber="true" FirstPageText="首页" PageSize="20"
        LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页">
    </webdiyer:AspNetPager>
    <br />

</asp:Content>

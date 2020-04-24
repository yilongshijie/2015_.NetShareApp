<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ComplaintList.aspx.cs" Inherits="XFXManagement.ComplaintList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="filter">
        <div class="row">


            <label>
                被举报用户ID:
                    <input type="text" id="UserIDInput" runat="server" />
            </label>


            <label>
                被举报帖子ID:
                    <input type="text" id="CirclePostIDInput" runat="server" />
            </label>

            <label>
                类型:
                    <asp:DropDownList ID="DropDownListType" runat="server">
                        <asp:ListItem Text="全部" Value="-1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="用户" Value="1"></asp:ListItem>
                        <asp:ListItem Text="帖子" Value="2"></asp:ListItem>
                    </asp:DropDownList>
            </label>

        </div>
        <div class="row">
            <input type="submit" id="Submit" value="查询" runat="server" onserverclick="Submit_Click" />
        </div>
    </div>
    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="ComplaintID">
        <Columns>

            <asp:TemplateField HeaderText="举报类型">
                <ItemTemplate>
                    <%# Eval("Type").ToString()=="1"?"用户":"帖子" %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="InitiativeUserID" HeaderText="举报人ID" SortExpression="InitiativeUserID" />
            <asp:BoundField DataField="PassivityUserID" HeaderText="被举报人ID" SortExpression="PassivityUserID" />
            <asp:BoundField DataField="CirclePostID" HeaderText="被举报帖子ID" SortExpression="CirclePostID" />
            <asp:BoundField DataField="Time" HeaderText="时间" SortExpression="Time" />
            <asp:BoundField DataField="Reason" HeaderText="原因" SortExpression="Reason" />
        </Columns>
        <EmptyDataTemplate>
            <ul class="states">
                <li class="warning">没有举报</li>
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

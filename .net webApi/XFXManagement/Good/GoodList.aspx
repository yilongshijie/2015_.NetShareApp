<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodList.aspx.cs" Inherits="XFXManagement.GoodList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="filter">
        <div class="row">
            <label>
                商品ID:
                    <input type="text" id="GoodID" runat="server" />
            </label>
            <label>
                商品类型:
                    <asp:DropDownList ID="DropDownListGoodGategory" runat="server"></asp:DropDownList>

            </label>

            <br />
            <table>
                <tr>
                    <td style="padding-left: 10px;">状态:</td>
                    <td>
                        <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                            <asp:ListItem Text="下架" Value="1"></asp:ListItem>
                            <asp:ListItem Text="上架" Value="2"></asp:ListItem>
                            <asp:ListItem Text="限时特价" Value="4"></asp:ListItem>
                            <asp:ListItem Text="周一新品" Value="8"></asp:ListItem>
                            <asp:ListItem Text="HOT热卖" Value="16"></asp:ListItem>
                            <asp:ListItem Text="包邮专区" Value="32"></asp:ListItem>
                            <asp:ListItem Text="女神必备" Value="64"></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="row">
            <input type="submit" id="Submit" value="查询" runat="server" onserverclick="Submit_Click" />
        </div>
    </div>

    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="GoodID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>
            <asp:BoundField DataField="GoodID" HeaderText="商品ID" InsertVisible="False" ReadOnly="True" SortExpression="GoodID" />
            <asp:TemplateField HeaderText="商品类别">
                <ItemTemplate>
                    <%# Eval("GoodGategory.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="标题" />
            <asp:BoundField DataField="BidPrice" HeaderText="标价" SortExpression="BidPrice" />
            <asp:BoundField DataField="RealPrice" HeaderText="实价" SortExpression="RealPrice" />
            <asp:BoundField DataField="State" HeaderText="状态" SortExpression="State" />
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# goodBLL.StateText(Convert.ToInt32( Eval("State"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Repertory" HeaderText="库存" SortExpression="Repertory" />
            <asp:BoundField DataField="SalesVolume" HeaderText="销量" SortExpression="SalesVolume" />
            <asp:BoundField DataField="EvaluateNum" HeaderText="评价数" SortExpression="EvaluateNum" />
            <asp:BoundField DataField="EvaluateValue" HeaderText="评分" SortExpression="EvaluateValue" />
            <asp:BoundField DataField="UpdateTime" HeaderText="更新时间" SortExpression="UpdateTime" />
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

    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" CssClass="anpager" CurrentPageButtonClass="cpb" PagingButtonSpacing="0"
        OnPageChanged="AspNetPager1_PageChanged" CurrentPageButtonPosition="Center"
        HorizontalAlign="center" AlwaysShowFirstLastPageNumber="true" FirstPageText="首页" PageSize="20"
        LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页">
    </webdiyer:AspNetPager>
    <br />
    <input type="submit" id="Add" value="添加" runat="server" onserverclick="Add_Click" />
    <br />
</asp:Content>

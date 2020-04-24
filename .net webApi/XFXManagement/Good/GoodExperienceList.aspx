<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodExperienceList.aspx.cs" Inherits="XFXManagement.GoodExperienceList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="filter">
        <div class="row">
            <label>
                体验师ID:
                    <input type="text" id="UserID" runat="server" />
            </label>
            <label>
                商品类型:
                    <asp:DropDownList ID="DropDownListGoodGategory" runat="server"></asp:DropDownList>

            </label>
            <label>
                商品ID:
                    <input type="text" id="GoodID" runat="server" />
            </label>
            <br />
            <table>
                <tr>
                    <td style="padding-left: 10px;">状态:</td>
                    <td>
                        <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                            <asp:ListItem Text="未审核" Value="1"></asp:ListItem>
                            <asp:ListItem Text="审核通过" Value="2"></asp:ListItem>
                            <asp:ListItem Text="用户删除" Value="4"></asp:ListItem>
                            <asp:ListItem Text="管理员删除" Value="8"></asp:ListItem>
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
        AutoGenerateColumns="False" DataKeyNames="GoodExperienceID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>

            <asp:BoundField DataField="UserID" HeaderText="用户ID" SortExpression="UserID" />
            <asp:BoundField DataField="GoodID" HeaderText="商品ID" SortExpression="GoodID" />
            <asp:BoundField DataField="State" HeaderText="状态" SortExpression="State" />
            <asp:TemplateField HeaderText="状态" SortExpression="State">
                <ItemTemplate>
                    <%# goodExperienceBLL.StateText(Convert.ToInt32( Eval("State"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" />

            <asp:TemplateField HeaderText="图片">
                <ItemTemplate>
                    <img src="<%# ConfigurationManager.AppSettings["UploadUrl"]+Eval("Image") %>" style="width: 7em;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ReplyNum" HeaderText="回复数" SortExpression="ReplyNum" />
            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" />
            <asp:BoundField DataField="OrderBy" HeaderText="优先级" SortExpression="OrderBy" />
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

</asp:Content>

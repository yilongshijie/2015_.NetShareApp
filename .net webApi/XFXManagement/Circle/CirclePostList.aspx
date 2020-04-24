<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CirclePostList.aspx.cs" Inherits="XFXManagement.CirclePostList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="filter">
        <div class="row">
            <label>
                用户ID:
                    <input type="text" id="UserID" runat="server" />
            </label>
            <label>
                圈子类型:
                    <asp:DropDownList ID="CircleTypeID" runat="server"></asp:DropDownList>

            </label>
            <label>
                帖子ID:
                    <input type="text" id="CirclePostID" runat="server" />
            </label>
            &nbsp;&nbsp;
            <asp:CheckBoxList runat="server" ID="CheckBoxList1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="未处理的帖子举报" Value="1"></asp:ListItem>
            </asp:CheckBoxList>
            <br />
            <table>
                <tr>
                    <td style="padding-left: 10px;">状态:</td>
                    <td>
                        <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                            <asp:ListItem Text="未审核" Value="1"></asp:ListItem>
                            <asp:ListItem Text="审核通过" Value="2"></asp:ListItem>
                            <asp:ListItem Text="置顶" Value="4"></asp:ListItem>
                            <asp:ListItem Text="加精" Value="8"></asp:ListItem>
                            <asp:ListItem Text="用户删除" Value="16"></asp:ListItem>
                            <asp:ListItem Text="管理员删除" Value="32"></asp:ListItem>
                            <asp:ListItem Text="匿名" Value="64"></asp:ListItem>
                            <asp:ListItem Text="位置" Value="128"></asp:ListItem>
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
        AutoGenerateColumns="False" DataKeyNames="CirclePostID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>
            <asp:BoundField DataField="CirclePostID" HeaderText="帖子ID" InsertVisible="False" ReadOnly="True" SortExpression="CirclePostID" />
            <asp:BoundField DataField="CircleType.Title" HeaderText="圈子类型" SortExpression="CircleTypeID" />
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" />
            <asp:BoundField DataField="ReplyNum" HeaderText="回复数" SortExpression="ReplyNum" />
            <asp:BoundField DataField="ComplaintNum" HeaderText="举报总数" SortExpression="ComplaintNum" />
            <asp:BoundField DataField="ComplaintUntreated" HeaderText="未处理举报数" SortExpression="ComplaintUntreated" />
            <asp:BoundField DataField="CreateTime" HeaderText="发帖时间" SortExpression="CreateTime" />
            <asp:BoundField DataField="UpdateTime" HeaderText="最新回帖" SortExpression="UpdateTime" />
            <asp:TemplateField HeaderText="状态" SortExpression="State">
                <ItemTemplate>
                    <%# circlePostBLL.StateText( Convert.ToInt32( Eval("State"))) %>
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

    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" CssClass="anpager" CurrentPageButtonClass="cpb" PagingButtonSpacing="0"
        OnPageChanged="AspNetPager1_PageChanged" CurrentPageButtonPosition="Center"
        HorizontalAlign="center" AlwaysShowFirstLastPageNumber="true" FirstPageText="首页" PageSize="20"
        LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页">
    </webdiyer:AspNetPager>
    <br />
    <input type="submit" id="Add" value="添加" runat="server" onserverclick="Add_Click" />
    <br />
</asp:Content>

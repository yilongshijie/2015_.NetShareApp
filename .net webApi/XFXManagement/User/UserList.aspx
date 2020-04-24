<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="XFXManagement.UserList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="filter">
        <div class="row">
            <label>
                用户ID:
                    <input type="text" id="UserID" runat="server" />
            </label>
            <label>
                手机号:
                    <input type="text" id="Tel" runat="server" />
            </label>
            <label>
                昵称:
                    <input type="text" id="NickName" runat="server" />
            </label>
            <label>
                用户类型:
                    <asp:DropDownList ID="UserTypeDropDownList" runat="server">
                        <asp:ListItem Value="0" Text="全部"></asp:ListItem>
                        <asp:ListItem Value="1" Text="普通用户"></asp:ListItem>
                        <asp:ListItem Value="2" Text="体验师"></asp:ListItem>
                        <asp:ListItem Value="4" Text="咨询师"></asp:ListItem>
                        <asp:ListItem Value="8" Text="系统管理员"></asp:ListItem>
                    </asp:DropDownList>

            </label>
            &nbsp;&nbsp;
            <asp:CheckBoxList runat="server" ID="CheckBoxList1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="未处理的用户举报" Value="1"></asp:ListItem>
            </asp:CheckBoxList>
        </div>
        <div class="row">
            <input type="submit" id="Submit" value="查询" runat="server" onserverclick="Submit_Click" />
        </div>
    </div>


    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="UserID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="用户" ReadOnly="True" InsertVisible="False" SortExpression="UserID"></asp:BoundField>
            <asp:BoundField DataField="Tel" HeaderText="电话" SortExpression="Tel"></asp:BoundField>
            <asp:BoundField DataField="NickName" HeaderText="昵称" SortExpression="NickName"></asp:BoundField>
            <asp:TemplateField HeaderText="类型" SortExpression="Type">
                <ItemTemplate>
                    <%#  userBLL.TypeText( Convert.ToInt32( Eval("Type"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="假数据" SortExpression="Type">
                <ItemTemplate>
                    <%# userBLL.GetCounterfeit( Convert.ToInt32( Eval("Type"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态" SortExpression="State">
                <ItemTemplate>
                    <%# userBLL.StateText( Convert.ToInt32( Eval("State"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserExtend.ExperienceLevel" HeaderText="经验等级" SortExpression="UserExtend.ExperienceLevel"></asp:BoundField>
            <asp:BoundField DataField="UserExtend.ComplaintUntreated" HeaderText="未处理举报数" SortExpression="UserExtend.ComplaintUntreated"></asp:BoundField>

            <asp:TemplateField HeaderText="举报原因" SortExpression="State">
                <ItemTemplate>
                    <%# userBLL.ComplaintUntreatedText( Convert.ToInt32( Eval("UserID")),Convert.ToInt32(Eval("UserExtend.ComplaintUntreated"))) %>
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

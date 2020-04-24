<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodHomeList.aspx.cs" Inherits="XFXManagement.GoodHomeList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="filter">
        <div class="row">

            <label>
                状态:
                    <asp:DropDownList ID="DropDownListState" runat="server">
                        <asp:ListItem Text="全部" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="正常" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="暂停" Value="0"></asp:ListItem>
                    </asp:DropDownList>

            </label>
            <label>
                商品类型:
                    <asp:DropDownList ID="DropDownListGoodGategory" runat="server"></asp:DropDownList>

            </label>

        </div>
        <div class="row">
            <input type="submit" id="Submit" value="查询" runat="server" onserverclick="Submit_Click" />
        </div>
    </div>
    <ul class="states">
        <li class="warning">加入了缓存机制，修改后需要等待10个小时app端才能看到更改</li>
    </ul>
    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="GoodHomeID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>
            <asp:TemplateField HeaderText="商品类别" SortExpression="GoodGategoryID">
                <ItemTemplate>
                    <%# Eval("GoodGategory.Name") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GoodID" HeaderText="商品ID" SortExpression="GoodID" />
            <asp:TemplateField HeaderText="商品" SortExpression="GoodID">
                <ItemTemplate>
                    <%# Eval("Good.Title") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" />
            <asp:BoundField DataField="Label" HeaderText="小标题" SortExpression="Label" />
            <asp:TemplateField HeaderText="状态" SortExpression="State">
                <ItemTemplate>
                    <%# Eval("State").ToString()=="1"?"正常":"暂停" %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="首页图片">
                <ItemTemplate>
                    <img src="<%# ConfigurationManager.AppSettings["UploadUrl"]+Eval("Image") %>" style="height: 6em;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="尺寸" SortExpression="Flex">
                <ItemTemplate>
                    <%# Eval("Flex").ToString()=="1"?"小":"大" %>
                </ItemTemplate>
            </asp:TemplateField>
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
    <input type="submit" id="Add" value="添加" runat="server" onserverclick="Add_Click" />
    <br />
</asp:Content>

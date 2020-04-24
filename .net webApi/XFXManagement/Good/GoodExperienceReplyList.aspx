<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodExperienceReplyList.aspx.cs" Inherits="XFXManagement.GoodExperienceReplyList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="GoodExperienceReplyID" OnRowCommand="GridView1_RowCommand">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="回复用户ID" SortExpression="UserID" />

            <asp:TemplateField HeaderText="回复">

                <ItemTemplate>
                    <%#   HttpUtility.HtmlDecode( Eval("Detail").ToString()) %>
                    <%#  XFXClassLibrary.XFXExt.imgList( Eval("ImgList").ToString(), ConfigurationManager.AppSettings["UploadUrl"])  %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# goodExperienceBLL.ReplyStateText( Convert.ToInt32( Eval("State"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" />
            <asp:BoundField DataField="Floor" HeaderText="楼层" SortExpression="Floor" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="StateDeleteID" CommandName="StateDelete" runat="server" CommandArgument='<%# Eval("GoodExperienceReplyID") %>'>删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <ul class="states">
                <li class="warning">没有回复</li>
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

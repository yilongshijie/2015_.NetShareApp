<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CirclePostReplyList.aspx.cs" Inherits="XFXManagement.CirclePostReplyList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        img {
            width: 15em;
        }

        .phiz {
            width: 2em;
        }
    </style>
    <div class="filter" id="filter" runat="server">
        <div class="row">
            <label>
                用户ID:
                    <input type="text" id="UserID" runat="server" />
            </label>
            <label>
                帖子ID:
                     <input type="text" id="CirclePostID" runat="server" />
            </label>
            <label>
         
                <asp:CheckBox ID="CheckBox1" runat="server" Text="按子回复排序" />
            </label>

        </div>
        <div class="row">
            <input type="submit" id="Submit" value="查询" runat="server" onserverclick="Submit_Click" />
        </div>
    </div>
    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="GridView"
        AutoGenerateColumns="False" DataKeyNames="CirclePostReplyID" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="回复用户ID" SortExpression="UserID" />
            <asp:BoundField DataField="CirclePostID" HeaderText="帖子ID" SortExpression="CirclePostID" />
            <asp:BoundField DataField="CirclePostReplyID" HeaderText="回复ID" SortExpression="CirclePostReplyID" />
            <asp:TemplateField HeaderText="回复">
                <HeaderStyle Width="60%"></HeaderStyle>
                <ItemTemplate>
                    <%#   HttpUtility.HtmlDecode( Eval("Detail").ToString()) %>
                    <%#  XFXClassLibrary.XFXExt.imgList( Eval("ImgList").ToString(), ConfigurationManager.AppSettings["UploadUrl"])  %>

                    <asp:GridView ID="GridViewChild1" runat="server" AutoGenerateColumns="false" CssClass="GridView">

                        <Columns>
                            <asp:BoundField DataField="InitiativeUserID" HeaderText="子回复用户ID" SortExpression="InitiativeUserID" />
                            <asp:TemplateField HeaderText="子回复">
                                <HeaderStyle Width="60%"></HeaderStyle>
                                <ItemTemplate>
                                    <%#   HttpUtility.HtmlDecode( Eval("Detail").ToString()) %>
                                    <%#  XFXClassLibrary.XFXExt.imgList( Eval("ImgList").ToString(), ConfigurationManager.AppSettings["UploadUrl"])  %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" />

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="StateDeleteID" CommandName="ChildDelete" runat="server" CommandArgument='<%# Eval("CirclePostReplyChildID") %>'>删除</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

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
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# circlePostBLL.ReplyStateText( Convert.ToInt32( Eval("State"))) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" />
            <asp:BoundField DataField="Floor" HeaderText="楼层" SortExpression="Floor" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="StateDeleteID" CommandName="StateDelete" runat="server" CommandArgument='<%# Eval("CirclePostReplyID") %>'>删除</asp:LinkButton>
                    <br />
                    <asp:LinkButton ID="StateZhidingID" CommandName="StateZhiding" runat="server" CommandArgument='<%# Eval("CirclePostReplyID") %>'>置顶</asp:LinkButton>
                    <asp:LinkButton ID="LinkQuxiaoID" CommandName="StateQuxiao" runat="server" CommandArgument='<%# Eval("CirclePostReplyID") %>'>取消置顶</asp:LinkButton>
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
    <input type="submit" id="Add" value="添加" runat="server" onserverclick="Add_Click" />
    <br />
</asp:Content>

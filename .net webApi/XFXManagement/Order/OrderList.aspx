<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="XFXManagement.OrderList" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul class="states">
        <li class="warning">发货前，必须要核对 第三方支付平台批次号，并去对应平台审核订单支付状态</li>
    </ul>
    <div class="filter">
        <div class="row">
            <label>
                订单号:
                    <input type="text" id="OrderID" runat="server" />
            </label>
            <label>
                用户ID:
                    <input type="text" id="UserID" runat="server" />
            </label>
            <label>
                物流号:
                    <input type="text" id="LogisticsNumber" runat="server" />
            </label>
            &nbsp;&nbsp;
            <asp:CheckBoxList runat="server" ID="CheckBoxList1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <asp:ListItem Text="需要发货" Value="1"></asp:ListItem>
            </asp:CheckBoxList>
            <br />
            <table>
                <tr>
                    <td style="padding-left: 10px;">状态:</td>
                    <td>
                        <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                            <asp:ListItem Text="未支付" Value="1"></asp:ListItem>
                            <asp:ListItem Text="已支付" Value="2"></asp:ListItem>
                            <asp:ListItem Text="已发货" Value="4"></asp:ListItem>
                            <asp:ListItem Text="已收货" Value="8"></asp:ListItem>
                            <asp:ListItem Text="已评价" Value="16"></asp:ListItem>
                            <asp:ListItem Text="已隐藏" Value="32"></asp:ListItem>
                            <asp:ListItem Text="申请退货" Value="64"></asp:ListItem>
                            <asp:ListItem Text="退货处理" Value="128"></asp:ListItem>
                            <asp:ListItem Text="同意退货" Value="256"></asp:ListItem>
                            <asp:ListItem Text="已退货" Value="512"></asp:ListItem>
                            <asp:ListItem Text="已收货" Value="1024"></asp:ListItem>
                            <asp:ListItem Text="已退款" Value="2048"></asp:ListItem>
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
        AutoGenerateColumns="False" DataKeyNames="OrderID"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True" OnSorting="GridView1_Sorting">
        <Columns>
            <asp:BoundField DataField="OrderID" HeaderText="订单号" ReadOnly="True" SortExpression="OrderID" />
            <asp:BoundField DataField="UserID" HeaderText="用户ID" SortExpression="UserID" />
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%#  XFXClassLibrary.OrderBLL.GetText( Convert.ToInt32( Eval("State")))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Num" HeaderText="总数量" SortExpression="Num" />

            <asp:TemplateField HeaderText="图片">
                <ItemTemplate>
                    <img src="<%# ConfigurationManager.AppSettings["UploadUrl"]+Eval("Image") %>" style="width: 4em; height: 4em;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LogisticsNumber" HeaderText="物流号" SortExpression="LogisticsNumber" />
            <asp:BoundField DataField="OrderExtend.PaymentPrice" HeaderText="支付金额" SortExpression="OrderExtend.PaymentPrice" />
            <asp:BoundField DataField="OrderExtend.ThirdPartyPayment" HeaderText="支付平台" SortExpression="OrderExtend.ThirdPartyPayment" />
            <asp:BoundField DataField="OrderExtend.ThirdPartyPaymentNumber" HeaderText="支付平台批次号" SortExpression="OrderExtend.ThirdPartyPaymentNumber" />
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

</asp:Content>

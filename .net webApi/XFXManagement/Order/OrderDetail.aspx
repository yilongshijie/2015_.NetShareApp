<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="XFXManagement.OrderDetail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .shopcarUl {
            flex: 1;
        }

            .shopcarUl li {
                display: flex;
                font-size: 0.8em;
                line-height: 1.1em;
                padding: 1.6em 0.5em 1.6em 0.1em;
                border-bottom: 1px dashed #ccc;
            }

                .shopcarUl li > div {
                    display: flex;
                    width: 80%;
                }

                .shopcarUl li .shopcarCheckbox {
                    display: flex;
                    justify-content: center;
                    flex-direction: column;
                    text-align: center;
                    width: 2.2em;
                }

            .shopcarUl .shopcarImg {
                display: flex;
                justify-content: center;
                flex-direction: column;
                text-align: center;
                padding: 0 0.3em 0 0;
            }

            .shopcarUl li img {
                height: 5em;
                width: 5em;
            }

            .shopcarUl li .shopcarCenter {
                flex: 1;
                padding: 0 0.2em;
                line-height: 1.4em;
            }

            .shopcarUl li .shopcarPrice {
                display: flex;
                flex-direction: column;
                justify-content: space-between;
                text-align: center;
                flex-basis: 7.8em;
                word-wrap: break-word;
            }

            .shopcarUl li var {
                color: #EB6996 !important;
            }

            .shopcarUl li .shopcarPrice p b {
                font-size: 1.8em;
                padding: 0.3em;
                color: #aaa;
            }

                .shopcarUl li .shopcarPrice p b:active {
                    color: #444;
                }

            .shopcarUl li .shopcarCenter .title {
                font-size: 0.8em;
                min-height: 2em;
            }

            .shopcarUl li .shopcarCenter .guigeText var {
                font-style: normal;
                color: #848484;
                font-size: 0.7em;
            }

            .shopcarUl li .shopcarCenter .num p {
                display: flex;
                border: 1px solid #ccc;
                line-height: 1.7em;
                float: left;
            }

            .shopcarUl li .shopcarCenter .num b {
                font-size: 1.5em;
                border-left: 1px solid #ccc;
                padding: 0 0.2em;
                text-align: center;
            }

                .shopcarUl li .shopcarCenter .num b:active {
                    background-color: #f0f0f0;
                }

            .shopcarUl li .shopcarCenter .num var {
                padding: 0 0.8em;
                font-style: normal;
            }

            .shopcarUl li .shopcarCenter .num b:first-child {
                border-right: 1px solid #ccc;
                border-left: 0px;
            }

        .orderUl {
            flex: initial;
        }

            .orderUl li {
                padding: 0.8em 0.5em;
            }

                .orderUl li .shopcarPrice {
                    flex-basis: 10em;
                    font-size: 1.2em;
                }

                    .orderUl li .shopcarPrice p b {
                        font-size: 1em;
                        padding: 0;
                    }
    </style>
    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="OrderID" HeaderText="订单详细">
        <Fields>
            <asp:BoundField DataField="OrderID" HeaderText="订单ID" ReadOnly="True" SortExpression="OrderID" />
            <asp:BoundField DataField="UserID" HeaderText="用户ID" SortExpression="UserID" ReadOnly="true" />
            <asp:TemplateField HeaderText="状态*">
                <ItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal" Enabled="false">
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

                </ItemTemplate>
                <EditItemTemplate>
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

                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Num" HeaderText="购买数" SortExpression="Num" ReadOnly="true" />
            <asp:TemplateField HeaderText="图片">
                <ItemTemplate>
                    <img src="<%# Eval("Image") %>" style="width: 4em; height: 4em;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详细">

                <ItemTemplate>
                    <%

                        XFXClassLibrary.OrderDetailsModel orderDetailsModel = Newtonsoft.Json.JsonConvert.DeserializeObject<XFXClassLibrary.OrderDetailsModel>(order.Detail.ToString());


                    %>
                    <ul class="shopcarUl orderUl">
                        <% foreach (var data in orderDetailsModel.OrderDetailModelList)
                            { %>
                        <li>
                            <div>
                                <p class="shopcarImg">
                                    <img src="<%= ConfigurationManager.AppSettings["UploadUrl"] +data.Image%>" />
                                </p>
                                <div class="shopcarCenter">
                                    <p class="title"><%=data.Title%> </p>
                                    <p class="guigeText">
                                        <%=data.SubTitle %><br />
                                        商品ID:<var><%=data.GoodID %></var>&nbsp;&nbsp;基本价:<var><%=
(data.RealPrice ).ToString("#0.00")%><br />
                                            子商品ID:<var><%=data.GoodChildID %></var>&nbsp;&nbsp; <%=data.Specification %>  &nbsp;&nbsp;金额变动:<%=
(data.AddPrice ).ToString("#0.00")%>  

                                    </p>
                                </div>
                                <div class="shopcarPrice">
                                    <span>单价:<var>￥<%=(data.RealPrice+data.AddPrice).ToString("#0.00")%></var></span>
                                    <p>
                                        <var>×<%=data.num%></var>
                                    </p>
                                </div>
                            </div>

                        </li>
                        <%} %>
                    </ul>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LogisticsNumber" HeaderText="物流号" SortExpression="LogisticsNumber" />
            <asp:TemplateField HeaderText="物流公司">
                <ItemTemplate>
                    <asp:DropDownList ID="DropDownList1" runat="server" Enabled="false">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="申通" Value="申通"></asp:ListItem>
                        <asp:ListItem Text="顺丰" Value="顺丰"></asp:ListItem>
                        <asp:ListItem Text="中通速递" Value="中通速递"></asp:ListItem>
                        <asp:ListItem Text="圆通速递" Value="圆通速递"></asp:ListItem>
                        <asp:ListItem Text="韵达快运" Value="韵达快运"></asp:ListItem>
                        <asp:ListItem Text="天天快递" Value="天天快递"></asp:ListItem>
                        <asp:ListItem Text="德邦" Value="德邦"></asp:ListItem>
                         <asp:ListItem Text="ems快递" Value="ems快递"></asp:ListItem>
                    </asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="申通" Value="申通"></asp:ListItem>
                        <asp:ListItem Text="顺丰" Value="顺丰"></asp:ListItem>
                        <asp:ListItem Text="中通速递" Value="中通速递"></asp:ListItem>
                        <asp:ListItem Text="圆通速递" Value="圆通速递"></asp:ListItem>
                        <asp:ListItem Text="韵达快运" Value="韵达快运"></asp:ListItem>
                        <asp:ListItem Text="天天快递" Value="天天快递"></asp:ListItem>
                         <asp:ListItem Text="德邦" Value="德邦"></asp:ListItem>
                         <asp:ListItem Text="ems快递" Value="ems快递"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LogisticsAddress" HeaderText="收货地址" SortExpression="LogisticsAddress" ReadOnly="true" />
            <asp:BoundField DataField="LogisticsTel" HeaderText="收货电话" SortExpression="LogisticsTel" ReadOnly="true" />
            <asp:BoundField DataField="LogisticsPerson" HeaderText="收货人" SortExpression="LogisticsPerson" ReadOnly="true" />
            <asp:BoundField DataField="DiscountCouponID" HeaderText="优惠券ID" SortExpression="DiscountCouponID" ReadOnly="true" />
            <asp:BoundField DataField="Experience" HeaderText="得到经验" SortExpression="Experience" ReadOnly="true" />
            <asp:BoundField DataField="Remark" HeaderText="备注" SortExpression="Remark" ReadOnly="true" />
            <asp:BoundField DataField="Remark1" HeaderText="买家留言" SortExpression="Remark1" ReadOnly="true" />
            <asp:BoundField DataField="Remark2" HeaderText="商家留言" SortExpression="Remark2" />
            <asp:BoundField DataField="CreateTime" HeaderText="创建世界" SortExpression="CreateTime" ReadOnly="true" />
            <asp:BoundField DataField="UpdateTime" HeaderText="更新时间" SortExpression="UpdateTime" ReadOnly="true" />

            <asp:BoundField DataField="OrderExtend.TotalPrice" HeaderText="商品总价" SortExpression="TotalPrice" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.DiscountPrice" HeaderText="优惠价" SortExpression="DiscountPrice" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.LogisticsPrice" HeaderText="邮费" SortExpression="LogisticsPrice" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.PaymentPrice" HeaderText="支付价 （包含邮费）" SortExpression="PaymentPrice" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.DiscountRemark" HeaderText="优惠备注" SortExpression="DiscountRemark" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.ThirdPartyPayment" HeaderText="第三方支付平台" SortExpression="ThirdPartyPayment" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.ThirdPartyPaymentNumber" HeaderText="第三方支付批次号" SortExpression="ThirdPartyPaymentNumber" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.GainIntegral" HeaderText="得到积分值" SortExpression="GainIntegral" ReadOnly="true" />
            <asp:BoundField DataField="OrderExtend.UseIntegral" HeaderText="使用积分值" SortExpression="UseIntegral" ReadOnly="true" />

            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server"
                        CausesValidation="false" CommandName="Edit" Text="编辑" />

                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CausesValidation="True"
                        CommandName="Update" Text="更新" OnClientClick="if( confirm('确定要更新此记录吗？')){  
                        return true;}else{ return false;}" />

                    <asp:Button ID="btnCancle" runat="server"
                        CausesValidation="false" CommandName="Cancel" Text="取消" />

                </EditItemTemplate>

            </asp:TemplateField>
        </Fields>
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    </asp:DetailsView>


    <asp:Panel ID="Panel1" runat="server">
        <hr />
        <br />
        <fieldset>

            <legend>订单日志</legend>
            <iframe src="OrderLogList.aspx?orderID=<%= order.OrderID %>" style="border: 0; min-height: 500px; width: 100%;"></iframe>
        </fieldset>
        <br />
    </asp:Panel>


</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodExperienceDetail.aspx.cs" Inherits="XFXManagement.GoodExperienceDetail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="GoodExperienceID" HeaderText="体验师">
        <Fields>
            <asp:BoundField DataField="GoodExperienceID" HeaderText="体验ID" InsertVisible="False" ReadOnly="True" SortExpression="GoodExperienceID" />
            <asp:BoundField DataField="UserID" HeaderText="体验师ID" SortExpression="UserID" ReadOnly="true" />
            <asp:BoundField DataField="GoodGategoryID" HeaderText="商品类型ID" SortExpression="GoodGategoryID" ReadOnly="true" />
            <asp:BoundField DataField="GoodID" HeaderText="商品ID" SortExpression="GoodID" ReadOnly="true" />
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="未审核" Value="1"></asp:ListItem>
                        <asp:ListItem Text="审核通过" Value="2"></asp:ListItem>
                        <asp:ListItem Text="用户删除" Value="4"></asp:ListItem>
                        <asp:ListItem Text="管理员删除" Value="8"></asp:ListItem>

                    </asp:CheckBoxList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                        <asp:ListItem Text="未审核" Value="1"></asp:ListItem>
                        <asp:ListItem Text="审核通过" Value="2"></asp:ListItem>
                        <asp:ListItem Text="用户删除" Value="4"></asp:ListItem>
                        <asp:ListItem Text="管理员删除" Value="8"></asp:ListItem>
                    </asp:CheckBoxList>
                </EditItemTemplate>

            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" ReadOnly="true" />

            <asp:TemplateField HeaderText="详细">
                <ItemTemplate>
                    <div id="detail"></div>
                    <style>
                        #detail {
                        }

                            #detail img {
                                width: 15em;
                                display: block;
                            }

                            #detail label {
                                display: block;
                                margin-bottom: 0.5em;
                            }
                    </style>
                    <script>

                        var Images = "<%#goodExperience.Images%>".split(",");
                        var Deatils = "<%#goodExperience.Deatil%>".split("####");
                        _.each(Images, function (item, index) {
                            $("#detail").append("<img  src=\"" + item + "\"   /> <label>" + Deatils[index] + "</label>")
                        });
                    </script>
                </ItemTemplate>
            </asp:TemplateField> 
            <asp:BoundField DataField="ReplyNum" HeaderText="回复数" SortExpression="ReplyNum" ReadOnly="true" />

            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" ReadOnly="true" />
            <asp:BoundField DataField="UpdateTime" HeaderText="更新时间" SortExpression="UpdateTime" ReadOnly="true" />
            <asp:BoundField DataField="OrderBy" HeaderText="排序" SortExpression="OrderBy" />

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

            <legend>体验师回复</legend>
            <iframe src="GoodExperienceReplyList.aspx?goodExperienceID=<%=    ViewState["goodExperienceID"]  %>" style="border: 0; min-height: 300px; width: 100%;"></iframe>
        </fieldset>
        <br />
    </asp:Panel>
</asp:Content>

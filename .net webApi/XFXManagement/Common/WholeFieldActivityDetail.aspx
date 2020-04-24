<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WholeFieldActivityDetail.aspx.cs" Inherits="XFXManagement.WholeFieldActivityDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="WholeFieldActivityID" HeaderText="全场活动" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="WholeFieldActivityID" HeaderText="活动ID" InsertVisible="False" ReadOnly="True" SortExpression="WholeFieldActivityID" />
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" />
            <asp:BoundField DataField="FillPrice" HeaderText="满" SortExpression="FillPrice" />
            <asp:BoundField DataField="DiscountPrice" HeaderText="减" SortExpression="DiscountPrice" />

            <asp:TemplateField HeaderText="类型*">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="包邮" Value="1"></asp:ListItem>
                        <asp:ListItem Text="满减" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="包邮" Value="1"></asp:ListItem>
                        <asp:ListItem Text="满减" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="包邮" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="满减" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态*">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="使用" Value="1"></asp:ListItem>
                        <asp:ListItem Text="停用" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="使用" Value="1"></asp:ListItem>
                        <asp:ListItem Text="停用" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="使用" Value="1"></asp:ListItem>
                        <asp:ListItem Text="停用" Value="0" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server"
                        CausesValidation="false" CommandName="Edit" Text="编辑" />

                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CausesValidation="True"
                        CommandName="Update" Text="更新" OnClientClick="return confirm('确定要更新此记录吗？');" />

                    <asp:Button ID="btnCancle" runat="server"
                        CausesValidation="false" CommandName="Cancel" Text="取消" />

                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:Button ID="btnAdd" runat="server" CausesValidation="True"
                        CommandName="Insert" Text="添加" OnClientClick="return confirm('确定要添加此记录吗？');" />
                    <asp:Button ID="btnCancle" runat="server"
                        CausesValidation="false" CommandName="Cancel" Text="取消" />
                </InsertItemTemplate>
            </asp:TemplateField>
        </Fields>
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />


    </asp:DetailsView>

</asp:Content>

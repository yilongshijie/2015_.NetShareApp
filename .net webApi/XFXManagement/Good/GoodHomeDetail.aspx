<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodHomeDetail.aspx.cs" Inherits="XFXManagement.GoodHomeDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="GoodHomeID" HeaderText="首页商品<span style='color:#dc6200'>( 当一个商品类别下，至少有1个大尺寸和4个小尺寸的首页商品，app端才会显示)</span>" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:TemplateField HeaderText="商品类别*">
                <ItemTemplate>
                    <%#  Eval("GoodGategory.Name")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList runat="server" ID="DropDownList_GoodGategoryID" DataValueField="GoodGategoryID" DataTextField="Name"></asp:DropDownList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList runat="server" ID="DropDownList_GoodGategoryID" DataValueField="GoodGategoryID" DataTextField="Name"></asp:DropDownList>

                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GoodID" HeaderText="对应商品ID*" SortExpression="GoodID" />
            <asp:BoundField DataField="Title" HeaderText="标题" SortExpression="Title" />
            <asp:BoundField DataField="Label" HeaderText="小标题" SortExpression="Label" />
            <asp:TemplateField HeaderText="状态*">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="暂停" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="暂停" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="正常" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="暂停" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="尺寸*<br/> 生成最大尺寸<br/>小{ 宽: 198, 高: 240 } <br/> 大{ 宽: 396, 高: 240 }]">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Flex" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="小" Value="1"></asp:ListItem>
                        <asp:ListItem Text="大" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Flex" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="小" Value="1"></asp:ListItem>
                        <asp:ListItem Text="大" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Flex" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="小" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="大" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="首页图片*<br/>生成最大尺寸<br/>宽：640 ，高：192">
                <ItemTemplate>
                    <img src="<%# Eval("Image") %>" style="height: 10em;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <img src="<%#  goodHome.Image %>" id="imgHome_url" style="height: 10em;" runat="server" />
                    <input type="hidden" id="fileHome_url" runat="server" />
                    <input id="fileHome_upload" name="fileHome_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <img id="imgHome_url" style="height: 10em;" runat="server" />
                    <input type="hidden" id="fileHome_url" runat="server" />
                    <input id="fileHome_upload" name="fileHome_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="OrderBy" HeaderText="优先级" SortExpression="OrderBy" />

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
    <script>

        $("#fileHome_upload").uploadify({
            fileTypeDesc: 'Image Files',
            fileTypeExts: '*.gif; *.jpg; *.png;*.jpeg;*.bmp',
            queueSizeLimit: 1,
            buttonClass: 'upload',
            fileSizeLimit: '5MB',
            buttonText: "上传图片",
            height: 30,
            formData: { width:640, height: 192 },
            swf: '/uploadify/uploadify.swf',
            uploader: '/uploadify/UploadImageHandler.ashx',
            UploadUrl: "<%=ConfigurationManager.AppSettings["UploadUrl"] %>",
            width: 120,
            onUploadSuccess: function (file, data, response) {
                $("#ContentPlaceHolder1_DetailsView1_fileHome_url").val(data);
                $("#ContentPlaceHolder1_DetailsView1_imgHome_url").attr("src", this.settings.UploadUrl + data);
            },
            onUploadStart: function () {

                $("#fileHome_upload").uploadify("settings", "formData", ($("#ContentPlaceHolder1_DetailsView1_RadioButtonList_Flex_0").get(0).checked ? { width: 198, height: 240 } : { width: 396, height: 240 }));
            }
        });
    </script>
</asp:Content>

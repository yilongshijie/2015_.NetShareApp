<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdvertisementDetail.aspx.cs" Inherits="XFXManagement.AdvertisementDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="AdvertisementID" HeaderText="滚动图片" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="AdvertisementID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="AdvertisementID" />
            <asp:BoundField DataField="Title" HeaderText="标题*" SortExpression="Title" />


            <asp:TemplateField HeaderText="类型*">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="圈子" Value="1"></asp:ListItem>
                        <asp:ListItem Text="商城" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="圈子" Value="1"></asp:ListItem>
                        <asp:ListItem Text="商城" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Type" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="圈子" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="商城" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="图片*">
                <ItemTemplate>
                    <img src="<%# Eval("image") %>" style="width: 10em;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <img src="<%# advertisement.Image %>" id="img_url" style="width: 10em;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <img id="img_url" style="width: 10em;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Link" HeaderText="对应(商品/帖子)ID*" SortExpression="Link" />
            <asp:TemplateField HeaderText="状态">
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
            <asp:BoundField DataField="Time" HeaderText="创建时间" SortExpression="Time" InsertVisible="false" ReadOnly="true" />
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
        $("#file_upload").uploadify({
            fileTypeDesc: 'Image Files',
            fileTypeExts: '*.gif; *.jpg; *.png;*.jpeg;*.bmp',
            queueSizeLimit: 1,
            buttonClass: 'upload',
            fileSizeLimit: '5MB',
            buttonText: "上传图片",
            height: 30,
            formData: ($("#ContentPlaceHolder1_DetailsView1_RadioButtonList_Type_0").get(0).checked ? { width: 500, height: 250 } : { width: 500, height: 500 }),
            swf: '/uploadify/uploadify.swf',
            uploader: '/uploadify/UploadImageHandler.ashx',
            UploadUrl: "<%=ConfigurationManager.AppSettings["UploadUrl"] %>",
            width: 120,
            onUploadSuccess: function (file, data, response) {
                $("#ContentPlaceHolder1_DetailsView1_file_url").val(data);
                $("#ContentPlaceHolder1_DetailsView1_img_url").attr("src", this.settings.UploadUrl + data);
            },
            onUploadStart: function () {

                $("#file_upload").uploadify("settings", "formData", ($("#ContentPlaceHolder1_DetailsView1_RadioButtonList_Type_0").get(0).checked ? { width: 640, height: 320 } : { width: 640, height: 448 }));
            }
        });
    </script>
</asp:Content>

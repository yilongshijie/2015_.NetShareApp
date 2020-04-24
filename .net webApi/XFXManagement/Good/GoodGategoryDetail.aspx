<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodGategoryDetail.aspx.cs" Inherits="XFXManagement.GoodGategoryDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="GoodGategoryID" HeaderText="商品类目" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="GoodGategoryID" HeaderText="商品类目ID" InsertVisible="False" ReadOnly="True" SortExpression="GoodGategoryID" />
            <asp:BoundField DataField="Name" HeaderText="名称" SortExpression="Name" />
            <asp:BoundField DataField="Describe" HeaderText="描述" SortExpression="Describe" />
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
            <asp:TemplateField HeaderText="图片*<br/><b  style='color:red'>背景色为 #fefefe</b>">
                <ItemTemplate>
                    <img src="<%# Eval("Image") %>" style="width: 4em; height: 4em; border-radius: 50%;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <img src="<%# goodGategory.Image %>" id="img_url" style="width: 5em; height: 5em; border-radius: 50%;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <img id="img_url" style="width: 5em; height: 5em; border-radius: 50%;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="首页图片*">
                <ItemTemplate>
                    <img src="<%# Eval("ImageHome") %>" style="width: 16em;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <img src="<%# goodGategory.ImageHome %>" id="imgHome_url" style="width: 16em;" runat="server" />
                    <input type="hidden" id="fileHome_url" runat="server" />
                    <input id="fileHome_upload" name="fileHome_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <img id="imgHome_url" style="width: 16em;" runat="server" />
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
        $("#file_upload").uploadify({
            fileTypeDesc: 'Image Files',
            fileTypeExts: '*.gif; *.jpg; *.png;*.jpeg;*.bmp',
            queueSizeLimit: 1,
            buttonClass: 'upload',
            fileSizeLimit: '5MB',
            buttonText: "上传图片",
            height: 30,
            formData: { width: 200, height: 200 },
            swf: '/uploadify/uploadify.swf',
            uploader: '/uploadify/UploadImageHandler.ashx',
            UploadUrl: "<%=ConfigurationManager.AppSettings["UploadUrl"] %>",
            width: 120,
            onUploadSuccess: function (file, data, response) {
                $("#ContentPlaceHolder1_DetailsView1_file_url").val(data);
                $("#ContentPlaceHolder1_DetailsView1_img_url").attr("src", this.settings.UploadUrl + data);
            }
        });
        $("#fileHome_upload").uploadify({
            fileTypeDesc: 'Image Files',
            fileTypeExts: '*.gif; *.jpg; *.png;*.jpeg;*.bmp',
            queueSizeLimit: 1,
            buttonClass: 'upload',
            fileSizeLimit: '5MB',
            buttonText: "上传图片",
            height: 30,
            formData: { width: 640, height: 192 },
            swf: '/uploadify/uploadify.swf',
            uploader: '/uploadify/UploadImageHandler.ashx',
            UploadUrl: "<%=ConfigurationManager.AppSettings["UploadUrl"] %>",
            width: 120,
            onUploadSuccess: function (file, data, response) {
                $("#ContentPlaceHolder1_DetailsView1_fileHome_url").val(data);
                $("#ContentPlaceHolder1_DetailsView1_imgHome_url").attr("src", this.settings.UploadUrl + data);
            }
                });
    </script>
</asp:Content>

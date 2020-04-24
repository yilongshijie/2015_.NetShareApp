<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodChildDetail.aspx.cs" Inherits="XFXManagement.GoodChildDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="GoodChildID" HeaderText="子商品" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="GoodChildID" HeaderText="GoodChildID" SortExpression="GoodChildID" InsertVisible="false" ReadOnly="true" />
            <asp:BoundField DataField="GoodId" HeaderText="商品ID" SortExpression="GoodId" InsertVisible="false" ReadOnly="true" />
            <asp:BoundField DataField="Repertory" HeaderText="库存" SortExpression="Repertory" />
            <asp:BoundField DataField="Specification" HeaderText="规格" SortExpression="Specification" />
            <asp:BoundField DataField="AddPrice" HeaderText="金额变动" SortExpression="AddPrice" />
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="下架" Value="0"></asp:ListItem>
                        <asp:ListItem Text="上架" Value="1"></asp:ListItem>

                    </asp:CheckBoxList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                        <asp:ListItem Text="下架" Value="0"></asp:ListItem>
                        <asp:ListItem Text="上架" Value="1"></asp:ListItem>

                    </asp:CheckBoxList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SalesVolume" HeaderText="销量" SortExpression="SalesVolume" InsertVisible="false"  ReadOnly="true" />
            <asp:TemplateField HeaderText="图片">
                <ItemTemplate>
                    <img src="<%# Eval("Image") %>" style="width: 4em; height: 4em;  " />
                </ItemTemplate>
                <EditItemTemplate>
                    <img src="<%# goodChild.Image %>" id="img_url" style="width: 5em; height: 5em;  " runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <img id="img_url" style="width: 5em; height: 5em;  " runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" InsertVisible="false" ReadOnly="true" />
            <asp:BoundField DataField="UpdateTime" HeaderText="更新时间" SortExpression="UpdateTime" InsertVisible="false" ReadOnly="true" />
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
 
    </script>
    <br />
    <input type="submit" id="Add" value="返回列表" runat="server" onserverclick="Add_Click" />
    <br />
</asp:Content>

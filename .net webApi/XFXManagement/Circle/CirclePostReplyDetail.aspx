
<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CirclePostReplyDetail.aspx.cs" Inherits="XFXManagement.CirclePostReplyDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None"   AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="CircleTypeID" HeaderText="圈子类型" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="CirclePostReplyID" HeaderText="CirclePostReplyID" InsertVisible="False" ReadOnly="True" SortExpression="CirclePostReplyID" />
            <asp:BoundField DataField="CirclePostID" HeaderText="CirclePostID" SortExpression="CirclePostID" InsertVisible="false" />

            <asp:TemplateField HeaderText="用户ID（假）">
                <ItemTemplate>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList runat="server" ID="DropDownList_UserID" DataValueField="UserID" DataTextField="NickName"></asp:DropDownList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="图片*">
                <ItemTemplate>
                </ItemTemplate>
                <InsertItemTemplate>
                    <img id="img_url" style="width: 8em;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="详细">
                <ItemTemplate>
                </ItemTemplate>
                <InsertItemTemplate>
                      <asp:HiddenField ID="HiddenField1" runat="server" />
                    <div runat="server" id="div_Detail" class="div_Detail" contenteditable="true"></div>

                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" InsertVisible="false" />
            <asp:BoundField DataField="CreateTime" HeaderText="CreateTime" SortExpression="CreateTime" InsertVisible="false" />
            <asp:BoundField DataField="Floor" HeaderText="Floor" SortExpression="Floor" InsertVisible="false" />

            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server"
                        CausesValidation="false" CommandName="Edit" Text="编辑" />

                </ItemTemplate>
 
                <InsertItemTemplate>
                    <asp:Button ID="btnAdd" runat="server" CausesValidation="True"
                        CommandName="Insert" Text="添加" OnClientClick="if ( confirm('确定要添加此记录吗？')){$('#ContentPlaceHolder1_DetailsView1_HiddenField1').val($('#ContentPlaceHolder1_DetailsView1_div_Detail').html());
                           return true;}else{ return false;};" />
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
            formData: { cut: false, width: 640, height: 0 },
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
</asp:Content>

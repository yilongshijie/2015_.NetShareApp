<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CirclePostDetail.aspx.cs" Inherits="XFXManagement.CirclePostDetail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="CirclePostID" HeaderText="帖子" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>

            <asp:BoundField DataField="CirclePostID" HeaderText="帖子ID" InsertVisible="False" ReadOnly="True" SortExpression="CirclePostID" />
            <asp:TemplateField HeaderText="圈子ID">
                <ItemTemplate>
                    <%#  Eval("CircleTypeID")%>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList runat="server" ID="DropDownList_CircleTypeID" DataValueField="CircleTypeID" DataTextField="Title"></asp:DropDownList>
                </InsertItemTemplate>

            </asp:TemplateField>
            <asp:BoundField DataField="CircleType.Title" HeaderText="圈子标题" ReadOnly="True" InsertVisible="false" />
            <asp:TemplateField HeaderText="用户ID（假）">
                <ItemTemplate>
                    <%# Eval("UserID") %>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList runat="server" ID="DropDownList_UserID" DataValueField="UserID" DataTextField="NickName"></asp:DropDownList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="User.Tel" HeaderText="用户手机" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="User.NickName" HeaderText="用户昵称" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="CoordX" HeaderText="坐标X" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="CoordY" HeaderText="坐标Y" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="Title" HeaderText="标题" ReadOnly="True" />
            <asp:TemplateField HeaderText="详细">
                <ItemTemplate>
                    <div class="div_Detail_show"><%# Eval("Detail") %>  <%# Eval("ImgList") %></div>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField4" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                    <div runat="server" id="div_Detail" class="div_Detail" contenteditable="true"></div>
                    <div id="img_div" class="img_div" runat="server" contenteditable="true">
                    </div>
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ReplyNum" HeaderText="回复数" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="Floor" HeaderText="楼层" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="ComplaintNum" HeaderText="举报数" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="ComplaintNum" HeaderText="举报数" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="ComplaintNum" HeaderText="举报数" ReadOnly="True" InsertVisible="false" />
            <asp:TemplateField HeaderText="省份" InsertVisible="false">
                <ItemTemplate>
                    <asp:DropDownList ID="DropDownList_Province" runat="server" Enabled="false">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="北京市" Value="北京市"></asp:ListItem>
                        <asp:ListItem Text="天津市" Value="天津市"></asp:ListItem>
                        <asp:ListItem Text="河北省" Value="河北省"></asp:ListItem>
                        <asp:ListItem Text="山西省" Value="山西省"></asp:ListItem>
                        <asp:ListItem Text="内蒙古自治区" Value="内蒙古自治区"></asp:ListItem>
                        <asp:ListItem Text="辽宁省" Value="辽宁省"></asp:ListItem>
                        <asp:ListItem Text="吉林省" Value="吉林省"></asp:ListItem>
                        <asp:ListItem Text="黑龙江省" Value="黑龙江省"></asp:ListItem>
                        <asp:ListItem Text="上海市" Value="上海市"></asp:ListItem>
                        <asp:ListItem Text="江苏省" Value="江苏省"></asp:ListItem>
                        <asp:ListItem Text="浙江省" Value="浙江省"></asp:ListItem>
                        <asp:ListItem Text="安徽省" Value="安徽省"></asp:ListItem>
                        <asp:ListItem Text="福建省" Value="福建省"></asp:ListItem>
                        <asp:ListItem Text="江西省" Value="江西省"></asp:ListItem>
                        <asp:ListItem Text="山东省" Value="山东省"></asp:ListItem>

                        <asp:ListItem Text="河南省" Value="河南省"></asp:ListItem>
                        <asp:ListItem Text="湖北省" Value="湖北省"></asp:ListItem>
                        <asp:ListItem Text="湖南省" Value="湖南省"></asp:ListItem>
                        <asp:ListItem Text="广东省" Value="广东省"></asp:ListItem>
                        <asp:ListItem Text="广西壮族自治区" Value="广西壮族自治区"></asp:ListItem>
                        <asp:ListItem Text="海南省" Value="海南省"></asp:ListItem>
                        <asp:ListItem Text="重庆市" Value="重庆市"></asp:ListItem>
                        <asp:ListItem Text="四川省" Value="四川省"></asp:ListItem>
                        <asp:ListItem Text="贵州省" Value="贵州省"></asp:ListItem>
                        <asp:ListItem Text="云南省" Value="云南省"></asp:ListItem>

                        <asp:ListItem Text="西藏自治区" Value="西藏自治区"></asp:ListItem>
                        <asp:ListItem Text="陕西省" Value="陕西省"></asp:ListItem>
                        <asp:ListItem Text="甘肃省" Value="甘肃省"></asp:ListItem>
                        <asp:ListItem Text="青海省" Value="青海省"></asp:ListItem>
                        <asp:ListItem Text="宁夏回族自治区" Value="宁夏回族自治区"></asp:ListItem>
                        <asp:ListItem Text="新疆维吾尔自治区" Value="新疆维吾尔自治区"></asp:ListItem>
                        <asp:ListItem Text="香港特别行政区" Value="香港特别行政区"></asp:ListItem>
                        <asp:ListItem Text="澳门特别行政区" Value="澳门特别行政区"></asp:ListItem>
                        <asp:ListItem Text="台湾省" Value="台湾省"></asp:ListItem>

                    </asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="DropDownList_Province" runat="server" Enabled="true">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="北京市" Value="北京市"></asp:ListItem>
                        <asp:ListItem Text="天津市" Value="天津市"></asp:ListItem>
                        <asp:ListItem Text="河北省" Value="河北省"></asp:ListItem>
                        <asp:ListItem Text="山西省" Value="山西省"></asp:ListItem>
                        <asp:ListItem Text="内蒙古自治区" Value="内蒙古自治区"></asp:ListItem>
                        <asp:ListItem Text="辽宁省" Value="辽宁省"></asp:ListItem>
                        <asp:ListItem Text="吉林省" Value="吉林省"></asp:ListItem>
                        <asp:ListItem Text="黑龙江省" Value="黑龙江省"></asp:ListItem>
                        <asp:ListItem Text="上海市" Value="上海市"></asp:ListItem>
                        <asp:ListItem Text="江苏省" Value="江苏省"></asp:ListItem>
                        <asp:ListItem Text="浙江省" Value="浙江省"></asp:ListItem>
                        <asp:ListItem Text="安徽省" Value="安徽省"></asp:ListItem>
                        <asp:ListItem Text="福建省" Value="福建省"></asp:ListItem>
                        <asp:ListItem Text="江西省" Value="江西省"></asp:ListItem>
                        <asp:ListItem Text="山东省" Value="山东省"></asp:ListItem>

                        <asp:ListItem Text="河南省" Value="河南省"></asp:ListItem>
                        <asp:ListItem Text="湖北省" Value="湖北省"></asp:ListItem>
                        <asp:ListItem Text="湖南省" Value="湖南省"></asp:ListItem>
                        <asp:ListItem Text="广东省" Value="广东省"></asp:ListItem>
                        <asp:ListItem Text="广西壮族自治区" Value="广西壮族自治区"></asp:ListItem>
                        <asp:ListItem Text="海南省" Value="海南省"></asp:ListItem>
                        <asp:ListItem Text="重庆市" Value="重庆市"></asp:ListItem>
                        <asp:ListItem Text="四川省" Value="四川省"></asp:ListItem>
                        <asp:ListItem Text="贵州省" Value="贵州省"></asp:ListItem>
                        <asp:ListItem Text="云南省" Value="云南省"></asp:ListItem>

                        <asp:ListItem Text="西藏自治区" Value="西藏自治区"></asp:ListItem>
                        <asp:ListItem Text="陕西省" Value="陕西省"></asp:ListItem>
                        <asp:ListItem Text="甘肃省" Value="甘肃省"></asp:ListItem>
                        <asp:ListItem Text="青海省" Value="青海省"></asp:ListItem>
                        <asp:ListItem Text="宁夏回族自治区" Value="宁夏回族自治区"></asp:ListItem>
                        <asp:ListItem Text="新疆维吾尔自治区" Value="新疆维吾尔自治区"></asp:ListItem>
                        <asp:ListItem Text="香港特别行政区" Value="香港特别行政区"></asp:ListItem>
                        <asp:ListItem Text="澳门特别行政区" Value="澳门特别行政区"></asp:ListItem>
                        <asp:ListItem Text="台湾省" Value="台湾省"></asp:ListItem>

                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="City" HeaderText="市" ReadOnly="True" InsertVisible="false" />

            <asp:TemplateField HeaderText="状态" InsertVisible="false">
                <ItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="未审核" Value="1"></asp:ListItem>
                        <asp:ListItem Text="审核通过" Value="2"></asp:ListItem>
                        <asp:ListItem Text="置顶" Value="4"></asp:ListItem>
                        <asp:ListItem Text="加精" Value="8"></asp:ListItem>
                        <asp:ListItem Text="用户删除" Value="16" Enabled="false"></asp:ListItem>
                        <asp:ListItem Text="管理员删除" Value="32"></asp:ListItem>
                        <asp:ListItem Text="匿名" Value="64"></asp:ListItem>
                        <asp:ListItem Text="位置" Value="128"></asp:ListItem>

                    </asp:CheckBoxList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                        <asp:ListItem Text="未审核" Value="1"></asp:ListItem>
                        <asp:ListItem Text="审核通过" Value="2"></asp:ListItem>
                        <asp:ListItem Text="置顶" Value="4"></asp:ListItem>
                        <asp:ListItem Text="加精" Value="8"></asp:ListItem>
                        <asp:ListItem Text="用户删除" Value="16" Enabled="false"></asp:ListItem>
                        <asp:ListItem Text="管理员删除" Value="32"></asp:ListItem>
                        <asp:ListItem Text="匿名" Value="64"></asp:ListItem>
                        <asp:ListItem Text="位置" Value="128"></asp:ListItem>
                    </asp:CheckBoxList>
                </EditItemTemplate>

            </asp:TemplateField>
            <asp:BoundField DataField="Mark" HeaderText="删帖原因（可为空）" InsertVisible="false" />
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
                        CommandName="Insert" Text="添加" OnClientClick="if( confirm('确定要添加此记录吗？')){ $('#ContentPlaceHolder1_DetailsView1_HiddenField1').val($('#ContentPlaceHolder1_DetailsView1_div_Detail').html());
                         $('#ContentPlaceHolder1_DetailsView1_HiddenField4').val($('#ContentPlaceHolder1_DetailsView1_div_Detail').text());
                            var tempimg = $('.img_div img');
                            $('.img_div').html('');
                            $('.img_div').append(tempimg);
                        $('#ContentPlaceHolder1_DetailsView1_HiddenField2').val($('.img_div').html());
                        var imglist=[]; $.each(tempimg,function(i){ console.log(tempimg[i].src);imglist.push(tempimg[i].src); });
                         $('#ContentPlaceHolder1_DetailsView1_HiddenField3').val(imglist.toString());
                        return true;}else{ return false;}" />
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
                var tempimg = $(".img_div img");
                $(".img_div").html("");
                $(".img_div").append(tempimg);
                $(".img_div").append("<img src ='" + this.settings.UploadUrl + data + "' />");
            }
        });
    </script>
    <asp:Panel ID="Panel1" runat="server">
        <hr />
        <br />
        <fieldset>

            <legend>帖子回复</legend>
            <iframe src="CirclePostReplyList.aspx?circlePostID=<%=    ViewState["circlePostID"]  %>" style="border: 0; min-height: 300px; width: 100%;"></iframe>
        </fieldset>
        <br />
    </asp:Panel>

</asp:Content>

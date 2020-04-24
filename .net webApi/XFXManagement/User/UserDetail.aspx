<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="XFXManagement.UserDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="UserID" HeaderText="用户详情" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="UserID" HeaderText="用户ID" InsertVisible="False" ReadOnly="True" SortExpression="UserID" />
            <asp:TemplateField HeaderText="真假" InsertVisible="false">
                <ItemTemplate>
                    <%# userBLL.GetCounterfeit(user.Type) %>
                </ItemTemplate>
                <InsertItemTemplate>
                    假
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Tel" HeaderText="电话*" ReadOnly="True" SortExpression="Tel" />
            <asp:BoundField DataField="PassWord" HeaderText="密码*" ReadOnly="True" InsertVisible="true" ControlStyle-CssClass="warning" />
            <asp:BoundField DataField="NickName" HeaderText="昵称" ReadOnly="True" />
            <asp:TemplateField HeaderText="性别*">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Gender" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="男" Value="男"></asp:ListItem>
                        <asp:ListItem Text="女" Value="女"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Gender" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="男" Value="男"></asp:ListItem>
                        <asp:ListItem Text="女" Value="女" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="婚恋" InsertVisible="false">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Married" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="单身" Value="单身"></asp:ListItem>
                        <asp:ListItem Text="热恋中" Value="热恋中"></asp:ListItem>
                        <asp:ListItem Text="已婚" Value="已婚"></asp:ListItem>
                        <asp:ListItem Text="离异/丧偶" Value="离异/丧偶"></asp:ListItem>
                        <asp:ListItem Text="保密" Value="保密"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Married" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="单身" Value="单身" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="热恋中" Value="热恋中"></asp:ListItem>
                        <asp:ListItem Text="已婚" Value="已婚"></asp:ListItem>
                        <asp:ListItem Text="离异/丧偶" Value="离异/丧偶"></asp:ListItem>
                        <asp:ListItem Text="保密" Value="保密"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="性取向" InsertVisible="false">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_SexualOrientation" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="男" Value="男"></asp:ListItem>
                        <asp:ListItem Text="女" Value="女"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_SexualOrientation" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="男" Value="男" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="女" Value="女"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Age" HeaderText="年龄" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="Location" HeaderText="所在地" ReadOnly="True" />
            <asp:BoundField DataField="Constellation" HeaderText="星座" ReadOnly="True" InsertVisible="false" />
            <asp:TemplateField HeaderText="头像">
                <ItemTemplate>
                    <img src="<%# Eval("HeadPortrait") %>" style="width: 4em; height: 4em; border-radius: 50%;" />
                </ItemTemplate>
                <InsertItemTemplate>

                    <img id="img_url" style="width: 4em; height: 4em; border-radius: 50%;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="类型">
                <ItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_Type" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="普通用户" Value="1"></asp:ListItem>
                        <asp:ListItem Text="体验师" Value="2"></asp:ListItem>
                        <asp:ListItem Text="咨询师" Value="4"></asp:ListItem>
                        <asp:ListItem Text="系统管理员" Value="8"></asp:ListItem>
                    </asp:CheckBoxList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_Type" RepeatDirection="Horizontal">
                        <asp:ListItem Text="普通用户" Value="1" Enabled="false"></asp:ListItem>
                        <asp:ListItem Text="体验师" Value="2"></asp:ListItem>
                        <asp:ListItem Text="咨询师" Value="4"></asp:ListItem>
                        <asp:ListItem Text="系统管理员" Value="8"></asp:ListItem>
                    </asp:CheckBoxList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_Type" RepeatDirection="Horizontal">
                        <asp:ListItem Text="普通用户" Value="1" Enabled="false" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="体验师" Value="2"></asp:ListItem>
                        <asp:ListItem Text="咨询师" Value="4"></asp:ListItem>
                        <asp:ListItem Text="系统管理员" Value="8"></asp:ListItem>
                    </asp:CheckBoxList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="冻结" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="冻结" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_State" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="正常" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="冻结" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="CreatTime" HeaderText="注册时间" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UpdateTime" HeaderText="更新时间" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.ExperienceValue" HeaderText="经验值" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.ExperienceLevel" HeaderText="经验等级" />
            <asp:BoundField DataField="UserExtend.ExperienceName" HeaderText="经验名称" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.Integral" HeaderText="积分" ReadOnly="True" InsertVisible="false" Visible="false" />
            <asp:BoundField DataField="UserExtend.ComplaintSum" HeaderText="举报总数" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.ComplaintUntreated" HeaderText="未处理举报数" ReadOnly="True" InsertVisible="false" />
            <asp:TemplateField HeaderText="禁言" InsertVisible="false">
                <ItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Banned" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="正常" Value="0"></asp:ListItem>
                        <asp:ListItem Text="禁言" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Banned" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="正常" Value="0"></asp:ListItem>
                        <asp:ListItem Text="禁言" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="RadioButtonList_Banned" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="正常" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="禁言" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserExtend.BannedStartTime" HeaderText="禁言开始时间" ReadOnly="true" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.BannedEndTime" HeaderText="禁言结束时间" ReadOnly="true" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.Bust" HeaderText="胸围" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.Waist" HeaderText="腰围" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.Hips" HeaderText="臀围" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.Stature" HeaderText="身高" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.Weight" HeaderText="体重" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.ProfessionalTitle" HeaderText="职称" ReadOnly="True" InsertVisible="false" />
            <asp:BoundField DataField="UserExtend.ProfessionalDescription" HeaderText="描述" ReadOnly="True" InsertVisible="false" />

            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server"
                        CausesValidation="false" CommandName="Edit" Text="编辑" />
                    &nbsp;

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

    <asp:Panel ID="Panel1" runat="server">
        <hr /> <br />
        <asp:Label runat="server" Text="管理员私信" Width="160">   </asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" Width="360"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="发信" OnClick="Button1_Click" />

        <br />
        <br />
        <asp:Label runat="server" Text="修改用户头像昵称"  Width="160">    </asp:Label>
        <asp:Button ID="Button2" runat="server" Text="用户头像" OnClick="Button2_Click" />
        <asp:Button ID="Button3" runat="server" Text="用户昵称" OnClick="Button3_Click" />

        <br />
        <br />
        <fieldset>

            <legend>收货地址</legend>
            <iframe src="UserAddressDetail.aspx?userID=<%= user.UserID %>" style="border: 0; min-height: 250px; width: 100%;"></iframe>
        </fieldset>
        <br />
    </asp:Panel>
</asp:Content>

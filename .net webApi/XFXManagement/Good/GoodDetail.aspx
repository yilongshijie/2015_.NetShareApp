<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GoodDetail.aspx.cs" Inherits="XFXManagement.GoodDetail" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DetailsView ID="DetailsView1" runat="server" GridLines="None" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanging="DetailsView1_ModeChanging" AutoGenerateRows="False" CssClass="Detail"
        DataKeyNames="GoodID" HeaderText="商品" OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="GoodID" HeaderText="商品ID" InsertVisible="False" ReadOnly="True" SortExpression="GoodID" />
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
            <asp:BoundField DataField="Title" HeaderText="标题*" SortExpression="Title" />
            <asp:BoundField DataField="SubTitle" HeaderText="子标题" SortExpression="SubTitle" />
            <asp:BoundField DataField="BidPrice" HeaderText="标价*" SortExpression="BidPrice" />
            <asp:BoundField DataField="RealPrice" HeaderText="实价*" SortExpression="RealPrice" />
            <asp:TemplateField HeaderText="状态*">
                <ItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="下架" Value="1"></asp:ListItem>
                        <asp:ListItem Text="上架" Value="2"></asp:ListItem>
                        <asp:ListItem Text="限时特价" Value="4"></asp:ListItem>
                        <asp:ListItem Text="周一新品" Value="8"></asp:ListItem>
                        <asp:ListItem Text="HOT热卖" Value="16"></asp:ListItem>
                        <asp:ListItem Text="包邮专区" Value="32"></asp:ListItem>
                        <asp:ListItem Text="女神必备" Value="64"></asp:ListItem>
                    </asp:CheckBoxList>
                    <p style="color: red">当商品至少有一个子商品上架后，该商品才能上架</p>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                        <asp:ListItem Text="下架" Value="1"></asp:ListItem>
                        <asp:ListItem Text="上架" Value="2"></asp:ListItem>
                        <asp:ListItem Text="限时特价" Value="4"></asp:ListItem>
                        <asp:ListItem Text="周一新品" Value="8"></asp:ListItem>
                        <asp:ListItem Text="HOT热卖" Value="16"></asp:ListItem>
                        <asp:ListItem Text="包邮专区" Value="32"></asp:ListItem>
                        <asp:ListItem Text="女神必备" Value="64"></asp:ListItem>
                    </asp:CheckBoxList>
                    <p style="color: red">当商品至少有一个子商品上架后，该商品才能上架</p>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:CheckBoxList runat="server" ID="CheckBoxList_State" RepeatDirection="Horizontal">
                        <asp:ListItem Text="下架" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="上架" Value="2"></asp:ListItem>
                        <asp:ListItem Text="限时特价" Value="4"></asp:ListItem>
                        <asp:ListItem Text="周一新品" Value="8"></asp:ListItem>
                        <asp:ListItem Text="HOT热卖" Value="16"></asp:ListItem>
                        <asp:ListItem Text="包邮专区" Value="32"></asp:ListItem>
                        <asp:ListItem Text="女神必备" Value="64"></asp:ListItem>
                    </asp:CheckBoxList>
                    <p style="color: red">当商品至少有一个子商品上架后，该商品才能上架</p>
                </InsertItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="详情*">
                <ItemTemplate>
                    <div class="div_Detail_show"><%# Eval("Detail") %></div>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                    <script id="container" name="content" type="text/plain">
                          
                    </script>
                    <script>
                        $("#container").html($("#ContentPlaceHolder1_DetailsView1_HiddenField1").val())
                        window.initueditorboo = true;
                    </script>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                    <script id="container" name="content" type="text/plain">
                          
                    </script>
                    <script>
                        $("#container").html($("#ContentPlaceHolder1_DetailsView1_HiddenField1").val())
                        window.initueditorboo = true;
                    </script>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="商品详细页滚动图片*<br/>生成最大尺寸<br/> 宽：640，640">
                <ItemTemplate>
                    <div class="div_Detail_show"><%# Eval("ImageList") %></div>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                    <div id="img_div" class="img_div" runat="server" contenteditable="true">
                        <%# Eval("ImageList") %>
                    </div>
                    <input id="file_upload" name="file_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                    <div id="img_div" class="img_div" runat="server" contenteditable="true">
                    </div>
                    <input id="file_upload" name="file_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="商品列表页图片*<br/>生成最大尺寸<br/>宽：300 ，高：对应变化">
                <ItemTemplate>
                    <img src=" <%#good.Image %>" style="width: 16em;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <img src=" <%#good.Image %>" id="img_url" style="width: 7em;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="fileHome_upload" name="fileHome_upload" type="file" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <img id="img_url" style="width: 7em;" runat="server" />
                    <input type="hidden" id="file_url" runat="server" />
                    <input id="fileHome_upload" name="fileHome_upload" type="file" />
                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="SalesVolume" HeaderText="销量" SortExpression="SalesVolume" />
            <asp:BoundField DataField="EvaluateNum" HeaderText="评价数" SortExpression="EvaluateNum" />
            <asp:BoundField DataField="EvaluateValue" HeaderText="评价平均分" SortExpression="EvaluateValue" />

            <asp:BoundField DataField="OrderBy" HeaderText="优先级" SortExpression="OrderBy" />
            <asp:BoundField DataField="Repertory" HeaderText="库存" SortExpression="Repertory" InsertVisible="false" ReadOnly="true" />
            <asp:BoundField DataField="CreateTime" HeaderText="创建时间" SortExpression="CreateTime" InsertVisible="false" ReadOnly="true" />
            <asp:BoundField DataField="UpdateTime" HeaderText="更新时间" SortExpression="UpdateTime" InsertVisible="false" ReadOnly="true" />
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server"
                        CausesValidation="false" CommandName="Edit" Text="编辑" />

                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CausesValidation="True"
                        CommandName="Update" Text="更新" OnClientClick="if( confirm('确定要更新此记录吗？')){  
                             $('#ContentPlaceHolder1_DetailsView1_HiddenField1').val(ue.getContent());
                            var tempimg = $('.img_div img');
                            $('.img_div').html('');
                            $('.img_div').append(tempimg);
                        $('#ContentPlaceHolder1_DetailsView1_HiddenField2').val($('.img_div').html());
                        var imglist=[]; $.each(tempimg,function(i){ console.log(tempimg[i].src);imglist.push(tempimg[i].src); });
                         $('#ContentPlaceHolder1_DetailsView1_HiddenField3').val(imglist.toString());
                        return true;}else{ return false;}" />

                    <asp:Button ID="btnCancle" runat="server"
                        CausesValidation="false" CommandName="Cancel" Text="取消" />

                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:Button ID="btnAdd" runat="server" CausesValidation="True"
                        CommandName="Insert" Text="添加" OnClientClick="if( confirm('确定要添加此记录吗？')){ 
                                 $('#ContentPlaceHolder1_DetailsView1_HiddenField1').val(ue.getContent());
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
            formData: { width: 640, height: 640 },
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
        $("#fileHome_upload").uploadify({
            fileTypeDesc: 'Image Files',
            fileTypeExts: '*.gif; *.jpg; *.png;*.jpeg;*.bmp',
            queueSizeLimit: 1,
            buttonClass: 'upload',
            fileSizeLimit: '5MB',
            buttonText: "上传图片",
            height: 30,
            formData: { width: 300, height: 0, cut: "false" },
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
    <!-- 配置文件 -->
    <script type="text/javascript" src="/ueditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="/ueditor/ueditor.all.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        function initueditor() {


            window.ue = UE.getEditor('container', {
                initialFrameWidth: '90%',
                toolbars: [
    [

    'bold', //加粗
    'indent', //首行缩进

    'italic', //斜体
    'underline', //下划线
    'strikethrough', //删除线
    'subscript', //下标
    'fontborder', //字符边框
    'superscript', //上标
    'formatmatch', //格式刷
    'source', //源代码

    'pasteplain', //纯文本粘贴模式
    'selectall', //全选

    'preview', //预览
    'horizontal', //分隔线
    'removeformat', //清除格式
    'time', //时间
    'date', //日期



    'cleardoc', //清空文档


    'fontfamily', //字体
    'fontsize', //字号
    'paragraph', //段落格式
    'simpleupload', //单图上传
    'insertimage', //多图上传

    'map', //Baidu地图


    'justifyleft', //居左对齐
    'justifyright', //居右对齐
    'justifycenter', //居中对齐
    'justifyjustify', //两端对齐
    'forecolor', //字体颜色
    'backcolor', //背景色
    'insertorderedlist', //有序列表
    'insertunorderedlist', //无序列表
    'fullscreen', //全屏
    'directionalityltr', //从左向右输入
    'directionalityrtl', //从右向左输入
    'rowspacingtop', //段前距
    'rowspacingbottom', //段后距



    'lineheight', //行间距

    'customstyle', //自定义标题
    'autotypeset', //自动排版

    'touppercase', //字母大写
    'tolowercase', //字母小写
    'background', //背景 
    ]
                ]
            });
        }
        if (window.initueditorboo) {
            initueditor();
        }
    </script>
    <br />

    <asp:Panel ID="Panel1" runat="server">
        <hr />
        <br />
        <fieldset>

            <legend>子产品</legend>
            <iframe src="GoodChildList.aspx?goodID=<%=   ViewState["goodID"] %>" style="border: 0; min-height: 500px; width: 100%;"></iframe>
        </fieldset>
        <br />
    </asp:Panel>


</asp:Content>

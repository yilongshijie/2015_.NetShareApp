<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="XFXManagement.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/lrtk.css" />
    <link rel="stylesheet" href="Css/main.css" />
    <script src="JavaScript/jquery-1.11.3.js"></script>
    <script>
        setInterval(function () {

            $.ajax({
                url: "/NewNum.ashx",
                type: "POST",
                success: function (data) {
       
                    var list = data.split(',');
                    if (list[5] == "0")
                    {
                        alert("请先登录");
                        location.href = "/";
                    }
                    $(".tiezi").html(list[0]);
                    $(".dingdan").html(list[1]);
                    $(".jubaotiezi").html(list[2]);
                    $(".jubaoyonghu").html(list[3]);
                    $(".tiyanshi").html(list[4]);
                },
                error: function () {
                }
            })
        }, 6000)

    </script>
</head>
<body>
    <div id="wrapper">
        <div id="content">

            <div class="controls">
                <nav class="links">
                    <ul>
                        <li><a href="Circle/CirclePostList.aspx?state=1" target="iframe" class="ico1">未审核帖子 <span class="num tiezi"></span></a></li>
                        <li><a href="Order/OrderList.aspx?state=s2f4" class="ico2" target="iframe">需要发货订单 <span class="num dingdan"></span></a></li>
                        <li><a href="Circle/CirclePostList.aspx?state=jubao" class="ico3" target="iframe">未处理帖子举报 <span class="num jubaotiezi"></span></a></li>
                        <li><a href="User/UserList.aspx?state=jubao" class="ico3" target="iframe">未处理用户举报 <span class="num jubaoyonghu"></span></a></li>
                        <li><a href="Good/GoodExperienceList.aspx?state=1" class="ico3" target="iframe">未处理体验师 <span class="num tiyanshi"></span></a></li>
                    </ul>
                </nav>
                <div class="profile-box">
                    <span class="profile">
                        <a href="#" class="section">
                            <%--   <img class="image" src="src" alt="image description" width="26"
                                height="26" />--%>
                            <span class="text-box">Welcome
									<strong class="name">Asif Aleem</strong>
                            </span>
                        </a>
                        <a href="#" class="opener">opener</a>
                    </span>
                    <a href="/" class="btn-on">On</a>
                </div>
            </div>
            <iframe src="User/UserList.aspx" name="iframe" style="width: 100%"></iframe>


        </div>
        <div id="sidebar">
            <strong class="logo">性<br />
                分<br />
                享</strong>
            <div id="firstpane" class="menu_list">
                <p class="menu_head current">用户</p>
                <div style="display: block" class="menu_body">
                    <ul class="tabset buttons">
                        <li class="active">
                            <a href="User/UserList.aspx" target="iframe" class="ico1"><span>用户列表</span> </a>
                            <span class="tooltip"><span>用户列表</span></span>
                        </li>
                        <li>
                            <a href="User/UserGradeLogList.aspx" target="iframe" class="ico1"><span>经验积分</span> </a>
                            <span class="tooltip"><span>经验日志</span></span>
                        </li>
                    </ul>
                </div>
                <p class="menu_head">圈子</p>
                <div style="display: none" class="menu_body">
                    <ul class="tabset buttons">
                        <li>
                            <a href="Circle/CircleTypeList.aspx" target="iframe" class="ico2"><span>圈子类型</span> </a>
                            <span class="tooltip"><span>圈子类型</span></span>
                        </li>
                        <li>
                            <a href="Circle/CirclePostList.aspx" target="iframe"><span>帖子列表</span> </a>
                            <span class="tooltip"><span>帖子列表</span></span>
                        </li>
                        <li>
                            <a href="Circle/CirclePostReplyList.aspx?all=true" target="iframe"><span>回复列表</span> </a>
                            <span class="tooltip"><span>回复列表</span></span>
                        </li>
                    </ul>
                </div>
                <p class="menu_head">商城</p>
                <div style="display: none" class="menu_body">
                    <ul class="tabset buttons">
                        <li>
                            <a href="Good/GoodGategoryList.aspx" target="iframe"><span>商品类目</span> </a>
                            <span class="tooltip"><span>商品类目</span></span>
                        </li>
                        <li>
                            <a href="Good/GoodList.aspx" target="iframe"><span>商品列表</span> </a>
                            <span class="tooltip"><span>商品列表</span></span>
                        </li>
                        <li>
                            <a href="Good/GoodEvaluateList.aspx" target="iframe"><span>评论列表</span> </a>
                            <span class="tooltip"><span>评论列表</span></span>
                        </li>
                        <li>
                            <a href="Good/GoodExperienceList.aspx" target="iframe"><span>体验列表</span> </a>
                            <span class="tooltip"><span>体验列表</span></span>
                        </li>
                         <li>
                            <a href="Good/GoodExperienceReplyList.aspx" target="iframe"><span>体验回复</span> </a>
                            <span class="tooltip"><span>体验回复</span></span>
                        </li>
                        <li>
                            <a href="Order/OrderList.aspx" target="iframe"><span>订单列表</span> </a>
                            <span class="tooltip"><span>订单列表</span></span>
                        </li>
                    </ul>
                </div>
                <p class="menu_head">管理</p>
                <div style="display: none" class="menu_body">
                    <ul class="tabset buttons">
                        <li>
                            <a href="User/UserSMSList.aspx" target="iframe"><span>短信列表</span> </a>
                            <span class="tooltip"><span>短信列表</span></span>
                        </li>
                        <li>
                            <a href="Common/AdvertisementList.aspx" target="iframe"><span>滚动图片</span> </a>
                            <span class="tooltip"><span>滚动图片</span></span>
                        </li>
                        <li>
                            <a href="Good/GoodHomeList.aspx" target="iframe"><span>商城专栏</span> </a>
                            <span class="tooltip"><span>商城专栏</span></span>
                        </li>
                        <li>
                            <a href="Common/WholeFieldActivityList.aspx" target="iframe"><span>全场活动</span> </a>
                            <span class="tooltip"><span>全场活动</span></span>
                        </li>
                        <li>
                            <a href="Common/ComplaintList.aspx" target="iframe"><span>举报列表</span> </a>
                            <span class="tooltip"><span>举报列表</span></span>
                        </li>
                    </ul>
                </div>


            </div>

        </div>
    </div>
</body>
</html>
<script>
    $(".tabset li").on("click", function (e) {
        $(".tabset li").removeClass("active");
        $(this).addClass("active");
    });


</script>
<script>
    $(document).ready(function () {
        $("#firstpane .menu_body:eq(0)").show();
        $("#firstpane p.menu_head").click(function () {
            $(this).addClass("current").next("div.menu_body").slideToggle(300).siblings("div.menu_body").slideUp("slow");
            $(this).siblings().removeClass("current");
        });
        $("#secondpane .menu_body:eq(0)").show();
        $("#secondpane p.menu_head").mouseover(function () {
            $(this).addClass("current").next("div.menu_body").slideDown(500).siblings("div.menu_body").slideUp("slow");
            $(this).siblings().removeClass("current");
        });

    });
</script>

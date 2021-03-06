﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace XFXWebAPI.Controllers
{
    public class JsonController : ApiController
    {
        [HttpGet]
        public string about()
        {
            return "{\"icon\":\"\",\"tel\":\"400-696-6218\",\"email\":\"290870535@qq.com\",\"qq\":\"290870535\",\"webChat\":\"aishenghuo1616\",\"bq\":\"分享版权所有\",\"gs\":\"上海创界文化传播有限公司\",\"banben\":\"版本 2.0\"}";
        }
        [HttpGet]
        public string djgz()
        {
            return "{\"djtx\":\"等级与头衔是你的身份象征。等级越高头衔也越牛叉，同样等级男生和女生的头衔是不一样的。如果您还未设置性别可以到个人资料中修改，另外一些特权（比如私信），只有在一定等级后才能使用\",\"hqjy\":[{\"xw\":\"每日签到\",\"jy\":\"+20/天\",\"sm\":\"每天首次进入社区获得\"},{\"xw\":\"发帖\",\"jy\":\"+3/次\",\"sm\":\"每天限5次，帖子审核通过后生效\"},{\"xw\":\"回帖/评论\",\"jy\":\"+1/次\",\"sm\":\"每天限10次，在回复帖子后获得\"},{\"xw\":\"加精\",\"jy\":\"+10/次\",\"sm\":\"帖子被加精华后获得\"}],\"kcjy\":[{\"xw\":\"禁言\",\"jy\":\"-50/次\",\"sm\":\"严重违规会被直接禁言，小心哦\"},{\"xw\":\"警告\",\"jy\":\"-20/次\",\"sm\":\"如果违法规定，小编会直接警告，第二次警告就要被禁言哦\"},{\"xw\":\"恶意盖楼\",\"jy\":\"-1/条\",\"sm\":\"恶意刷回复的，删除一条扣除1经验，无封顶\"}],\"djxj\":\"0级：经验被扣成负的就会变成0级此时无法发帖和回帖等，只能浏览<br/>1级：初始化等级可以自由的发帖和回帖<br/>2级：解锁自定义头像<br/>3级：解锁发图<br/>5级：解锁私信功能\",\"dytx\":[{\"dj\":\"0(1以下)\",\"nan\":\"待解放\",\"nv\":\"待解码\",\"wzxb\":\"未受孕\"},{\"dj\":\"1(1-20)\",\"nan\":\"懵懂少年\",\"nv\":\"含苞待放\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"2(21-50)\",\"nan\":\"青涩稚嫩\",\"nv\":\"蜜桃成熟\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"3(51-80)\",\"nan\":\"才疏学浅\",\"nv\":\"可爱萌妹\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"4(81-150)\",\"nan\":\"寒窗苦练\",\"nv\":\"白领小资\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"5(151-350)\",\"nan\":\"秀才一枚\",\"nv\":\"性感火辣\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"6(351-620)\",\"nan\":\"风流倜傥\",\"nv\":\"电臀巨乳\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"7(621-1120)\",\"nan\":\"暖男欧巴\",\"nv\":\"女神驾到\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"8(1121-1920)\",\"nan\":\"文武双全\",\"nv\":\"梦中情人\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"9(1920-2220)\",\"nan\":\"少女杀手\",\"nv\":\"红颜知己\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"10(2221-3320)\",\"nan\":\"万人敬仰\",\"nv\":\"皇后娘娘\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"11(3321-4920)\",\"nan\":\"老少通吃\",\"nv\":\"迷倒众生\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"12(4921-6120)\",\"nan\":\"武林盟主\",\"nv\":\"人间极品\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"13(6121-7320)\",\"nan\":\"一统江湖\",\"nv\":\"仙女下凡\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"14(7321-9520)\",\"nan\":\"东方不败\",\"nv\":\"沉鱼落雁\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"15(9521-11720)\",\"nan\":\"世界霸主\",\"nv\":\"全球最美\",\"wzxb\":\"亦男亦女\"},{\"dj\":\"16(11721以上)\",\"nan\":\"已非人类\",\"nv\":\"宇宙无敌\",\"wzxb\":\"亦男亦女\"}]}";
        }
        [HttpGet]
        public string help()
        {
            return "{\"categorys\":[{\"id\":\"1\",\"title\":\"常见问题\",\"questions\":[{\"question\":\"你们商城的宝贝是正品么?\",\"answer\":\"我们所售商品都是来自正规品牌厂家,全部商品均具有品牌授权书与质量报告!所有产品都符合相关标准，我们拒绝劣质商品和假货，并且所有商品体验评分达到80分以上才会上线\"},{\"question\":\"发货保护隐私吗?\",\"answer\":\"100%隐私保护，发货包装上和快递单上不会体现任何产品信息!，别人代签也没关系\"},{\"question\":\"售后有保障吗?\",\"answer\":\"支持七天无理由退换货，产品在没有使用前，所有产品都可以无条件退换货，如有假货，一万倍赔偿用户。如有质量问题或发错货，退换来回的邮费分享承担，其他原因想退换货，寄回的邮费用户承担，换好的新品寄出的邮费分享承担。\"},{\"question\":\"社区发不了贴？\",\"answer\":\"1：被禁言后不能进行任何操作，包括发帖；2：等级没有大于1级也不能发帖，但可以通过回帖和评论获得经验升级后就可以发帖了\"},{\"question\":\"APP使用问题怎么解决？\",\"answer\":\"欢迎给我们反馈问题以及提供建议，一个好的产品需要大家多提宝贵建议，加QQ:290870535,最后采纳的意见和建议都会给予建议者奖励\"}]},{\"id\":\"2\",\"title\":\"关于购物\",\"questions\":[{\"question\":\"满多少可以包邮呢?怎么操作？\",\"answer\":\"在包邮板块所有商品都是包邮的，非包邮板块达到59元包邮，购买多件商品时需要先放入购物车再进行结算，这样统计商品总金额达到规定就可以享受包邮\"},{\"question\":\"宝贝需要退换货了怎么处理？款多久可以退回？\",\"answer\":\"需要加QQ290870535进行登记，然后寄回给我们，我们收到后并确认无误就可以办理退款，一般在24小时内会收到退款\"},{\"question\":\"几点前下单可以当天发货？哪家快递？\",\"answer\":\"每天下午四点之前的订单可以安排当天发货，货到付款只有圆通快递，在线支付可以选择申通，圆通等等\"},{\"question\":\"发货后几天可以收到产品？\",\"answer\":\"这个根据城市距离而定，江浙沪一般是隔天到，其他地区一般是3-5天，最终到达还要以快递公司为准\"},{\"question\":\"怎么查看物流信息？\",\"answer\":\"打开分享APP，点击我的-我的订单中就可以查看\"}]},{\"id\":\"3\",\"title\":\"社区问答\",\"questions\":[{\"question\":\"怎么发帖?\",\"answer\":\"在等级升到1级以上后就可以发帖，之前可以通过签到，回帖来获取经验升级\"},{\"question\":\"等级的作用？如何提升等级?\",\"answer\":\"等级越高获得的特权越多，比如私信需要达到5级，通过发帖，签到，发精华帖等方式可以获取升级所需经验\"},{\"question\":\"为什么被禁言了?\",\"answer\":\"被多次投诉，发广告，色情内容，QQ号，微信号，等违规内容，被禁言后如需申诉加Q：290870535\"},{\"question\":\"我的帖子怎么审核不通过?\",\"answer\":\"帖子发布的内容涉及到敏感信息或禁止发布的内容，建议修改后再发布\"},{\"question\":\"怎么私信和查看私信?\",\"answer\":\"达到5级后可以给对方发私信，在个人信息中心可以查看私信内容\"}]}]}";
        }
    }
}

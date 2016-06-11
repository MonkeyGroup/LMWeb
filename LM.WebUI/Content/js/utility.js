
var currTitle = 'lmyw';
// tab切换
function toggleTabs(dom) {
    var id = $(dom).attr("id"); // tab 元素节点 id
    var cid = id + "_content"; // tab 对应内容节点 id

    // tab 选中
    $(dom).parent().children().removeClass("hit");
    $(dom).addClass("hit");

    // 内容节点选中
    $("#" + cid).siblings().removeClass("show");
    $("#" + cid).addClass("show");

    // 全局变量
    currTitle = id == 'tabs_1' ? 'lmyw' : 'tbgz';
}

function showMoreArticle(target) {
    if (target == 'lmdt') {
        window.open('/Article/InfoList');
    }
    if (target == 'hyxw') {
        window.open('/Article/InfoList?type=行业信息');
    }
    if (target == 'ywgz') {
        window.open(currTitle == 'lmyw' ? '/Article/InfoList?type=联盟要闻' : '/Article/InfoList?type=特别关注');
    }
}

function showMoreCompany(target) {
    if (target == 'clly') {
        window.open('/Company/List');
    }
    if (target == 'yyly') {
        window.open('/Company/List');
    }
    if (target == 'kyxh') {
        window.open('/Company/List');
    }
}






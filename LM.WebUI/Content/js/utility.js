
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

}

function showMoreArticle(target) {
    alert(target);
}

function showMoreCompany(target) {
    alert(target);
}







// 幻灯片
function slide(imgList, idList) {
    // 初始化
    $("#aa #aa_index li:eq(0)").addClass("active");

    // 自动跳转事件
    var currInd = 0; // 起始下标
    window.setInterval(function () {
        currInd = ++currInd % imgList.length;
        $("#aa img").attr("src", imgList[currInd]).fadeOut(100).fadeIn(1000);
        $("#aa img").bind('click', function() {
            window.open('/Article/Detail?id=' + idList[currInd]);
        });
        $("#aa_index").find("li").removeClass("active");
        $("#aa #aa_index li:eq(" + currInd + ")").addClass("active");
    }, 3000);
    
    // 点击事件
	$("body").on("click", "#aa_index .li", function () {
		var $this = $(this);
		currInd = $this.text() - 1;
		$("#aa img").attr("src", imgList[currInd]).fadeOut(100).fadeIn(1000);
		$("#aa img").bind('click', function () {
		    //window.location.href = '/Article/InfoList?type=' + typeList[currInd];
		    window.open('/Article/Detail?id=' + idList[currInd]);
		});
		$("#aa_index").find("li").removeClass("active");
		$this.addClass("active");
	});
}
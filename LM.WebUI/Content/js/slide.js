
// �õ�Ƭ
function slide(imglist) {
    // �Զ���ת�¼�
    var currInd = 0; // ��ʼ�±�
    window.setInterval(function () {
        currInd = currInd % imglist.length;
        $("#aa img").attr("src", imglist[currInd]).fadeOut(100).fadeIn(1000);
        $("#aa_index").find("li").removeClass("active");
        $("#aa #aa_index li:eq(" + currInd + ")").addClass("active");
        currInd++;
    }, 3000);
    
    // ����¼�
	$("body").on("click", "#aa_index .li", function () {
		var $this = $(this);
		currInd = $this.text() - 1;
		$("#aa img").attr("src", imglist[currInd]).fadeOut(100).fadeIn(1000);
		$("#aa_index").find("li").removeClass("active");
		$this.addClass("active");
	});
}

// �õ�Ƭ
function slide(imgList, idList) {
    // ��ʼ��
    $("#aa #aa_index li:eq(0)").addClass("active");

    // �Զ���ת�¼�
    var currInd = 0; // ��ʼ�±�
    window.setInterval(function () {
        currInd = ++currInd % imgList.length;

        $("#aa img").attr("src", imgList[currInd]).fadeOut(200).fadeIn(800); // �л�ͼƬ
        $("#aa > a").attr("href", '/Article/Detail?id=' + idList[currInd]); // �л�����

        $("#aa_index").find("li").removeClass("active");
        $("#aa #aa_index li:eq(" + currInd + ")").addClass("active");
    }, 3000);

    // ����¼�
    $("body").on("click", "#aa_index .li", function () {
        var $this = $(this);
        thisId = $this.text() == undefined || $this.text() == '' ? $this.attr('cid') : $this.text();
        currInd = thisId - 1;

        $("#aa img").attr("src", imgList[currInd]).fadeOut(200).fadeIn(800);
        $("#aa > a").attr("href", '/Article/Detail?id=' + idList[currInd]);

        $("#aa_index").find("li").removeClass("active");
        $this.addClass("active");
    });
}
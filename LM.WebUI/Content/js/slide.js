// »ÃµÆÆ¬

function slide(imglist,currIndex){
	window.setInterval(function () {
		if (imglist.length == currIndex) {
			currIndex = 0;
			$("#aa img").attr("src", imglist[currIndex]).fadeOut(100).fadeIn(1000);
			$("#aa_index").find("li").removeClass("active");
			$("#aa #aa_index li:eq(" + currIndex + ")").addClass("active");
		} else {
			if (imglist.length == currIndex) {
				$("#aa img").attr("src", imglist[0]).fadeOut(100).fadeIn(1000);
				$("#aa_index").find("li").removeClass("active");
				$("#aa #aa_index li:eq(" + 0 + ")").addClass("active");
			}
			$("#aa img").attr("src", imglist[currIndex]).fadeOut(100).fadeIn(1000);
			$("#aa_index").find("li").removeClass("active");
			$("#aa #aa_index li:eq(" + currIndex + ")").addClass("active");
			++currIndex;
		}
	}, 3000);
	$("body").on("click", "#aa_index .li", function () {
		var $this = $(this);
		currIndex = $this.text() - 1;
		$("#aa img").attr("src", imglist[currIndex]).fadeOut(100).fadeIn(1000);
		$("#aa_index").find("li").removeClass("active");
		$this.addClass("active");
	});
}
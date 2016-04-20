
function createAd(){
    var p = document.createElement("DIV");
    var del = document.createElement("span");
    var p1 = document.createElement("DIV");
    p.id = "MyAlertBoxMasker";
    p.style.position = "fixed";
    p.style.width = '100%';
    p.style.height = '100%';
    p.style.zIndex = '998';
    p.style.margin = '0 auto';
    p.style.top = '0';
    p.style.left = '0';
    p.style.backgroundColor = "gray";
    p.style.opacity = '0.5';
    p.style.filter = "alpha(opacity=80)";
		
    //内容层
    // var top = parseInt(parseInt(document.body.scrollHeight) * 0.25) + document.body.scrollTop;
    p1.id = "MyAlertBox";
    p1.style.position = "fixed";
    // var left = document.documentElement.offsetHeight / 2;
    var left = (document.body.offsetWidth - 1000) / 2;
    // var left = 0;
    p1.style.zIndex = '999';
    p1.style.top = '0';
    p1.style.left = left + 'px';

    // 删除按钮
    p1.innerHTML =
        "<img src='bg_pop.png' width='1000px' height='auto'>"+
        '<span id="deleteAd" style="position:absolute;right:20px;top:90px;cursor:pointer;">'+
            '<img src="bg_pop_close.png">'+
        '</span>';//这里是浮动层的具体HTML内容
    document.body.appendChild(p);
    document.body.appendChild(p1);
}	
	
$(function(){
		createAd();
        $('#deleteAd').bind('click',function(){
            alert(123);
            document.body.removeChild(document.getElementById('MyAlertBoxMasker'));
            document.body.removeChild(document.getElementById('MyAlertBox'));
        });
});

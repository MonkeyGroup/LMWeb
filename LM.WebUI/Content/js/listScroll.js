
// 企业名称列表滚动
// first
var oMarquee1 = document.getElementById('newlist'); //滚动对象
var iLineHeight1 = 31; //单行高度，像素
var iLineCount1 = 20; //实际行数
var iScrollAmount1 = 1; //每次滚动高度，像素
oMarquee1.innerHTML += oMarquee1.innerHTML;//元素内部html代码
window.setTimeout("scroll_list()", 2000);
function scroll_list() { //定义函数
    oMarquee1.scrollTop += iScrollAmount1;
    if (oMarquee1.scrollTop == iLineCount1 * iLineHeight1)
        oMarquee1.scrollTop = 0;
    if (oMarquee1.scrollTop % iLineHeight1 == 0) {
        window.setTimeout("scroll_list()", 2000);
    } else {
        window.setTimeout("scroll_list()", 50);
    }
}


// second
var oMarquee2 = document.getElementById('newlist1'); //滚动对象
var iLineHeight2 = 31; //单行高度，像素
var iLineCount2 = 20; //实际行数
var iScrollAmount2 = 1; //每次滚动高度，像素
oMarquee2.innerHTML += oMarquee2.innerHTML;//元素内部html代码
window.setTimeout("scroll_list1()", 2000);
function scroll_list1() { //定义函数
    oMarquee2.scrollTop += iScrollAmount2;
    if (oMarquee2.scrollTop == iLineCount2 * iLineHeight2)
        oMarquee2.scrollTop = 0;
    if (oMarquee2.scrollTop % iLineHeight2 == 0) {
        window.setTimeout("scroll_list1()", 2000);
    } else {
        window.setTimeout("scroll_list1()", 50);
    }
}


// third
var oMarquee3 = document.getElementById('newlist2'); //滚动对象
var iLineHeight3 = 31; //单行高度，像素
var iLineCount3 = 20; //实际行数
var iScrollAmount3 = 1; //每次滚动高度，像素
oMarquee3.innerHTML += oMarquee3.innerHTML;//元素内部html代码
window.setTimeout("scroll_list2()", 2000);
function scroll_list2() { //定义函数
    oMarquee3.scrollTop += iScrollAmount3;
    if (oMarquee3.scrollTop == iLineCount3 * iLineHeight3)
        oMarquee3.scrollTop = 0;
    if (oMarquee3.scrollTop % iLineHeight3 == 0) {
        window.setTimeout("scroll_list2()", 2000);
    } else {
        window.setTimeout("scroll_list2()", 50);
    }
}









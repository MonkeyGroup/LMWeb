
// ��ҵ�����б����
// first
var oMarquee1 = document.getElementById('newlist'); //��������
var iLineHeight1 = 31; //���и߶ȣ�����
var iLineCount1 = 20; //ʵ������
var iScrollAmount1 = 1; //ÿ�ι����߶ȣ�����
oMarquee1.innerHTML += oMarquee1.innerHTML;//Ԫ���ڲ�html����
window.setTimeout("scroll_list()", 2000);
function scroll_list() { //���庯��
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
var oMarquee2 = document.getElementById('newlist1'); //��������
var iLineHeight2 = 31; //���и߶ȣ�����
var iLineCount2 = 20; //ʵ������
var iScrollAmount2 = 1; //ÿ�ι����߶ȣ�����
oMarquee2.innerHTML += oMarquee2.innerHTML;//Ԫ���ڲ�html����
window.setTimeout("scroll_list1()", 2000);
function scroll_list1() { //���庯��
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
var oMarquee3 = document.getElementById('newlist2'); //��������
var iLineHeight3 = 31; //���и߶ȣ�����
var iLineCount3 = 20; //ʵ������
var iScrollAmount3 = 1; //ÿ�ι����߶ȣ�����
oMarquee3.innerHTML += oMarquee3.innerHTML;//Ԫ���ڲ�html����
window.setTimeout("scroll_list2()", 2000);
function scroll_list2() { //���庯��
    oMarquee3.scrollTop += iScrollAmount3;
    if (oMarquee3.scrollTop == iLineCount3 * iLineHeight3)
        oMarquee3.scrollTop = 0;
    if (oMarquee3.scrollTop % iLineHeight3 == 0) {
        window.setTimeout("scroll_list2()", 2000);
    } else {
        window.setTimeout("scroll_list2()", 50);
    }
}










$(function() {
    $('.mycontent').find('table').text() &&
    // 处理复制的表格的自带宽高
    $('.mycontent').find('p>table').each(function () {
        $(this).removeAttr('style');
        $(this).attr('border', '0');
        $(this).attr('cellspacing', '0');
        $(this).find('tr').each(function () {
            $(this).removeAttr('width').removeAttr('height');
        });
        $(this).find('td').each(function () {
            $(this).removeAttr('width').removeAttr('height');
        });
        $(this).find('p').each(function () {
            $(this).css('width', 'auto');
            $(this).css('text-indent', '0');
            //$(this).css('max-width', '50px');
        });
    });
})
$(function () {

    //highlight keyword search
    $.ui.autocomplete.prototype._renderItem = function (ul, item) {
        var term = this.term.split(' ').join('|');
        var re = new RegExp("(" + term + ")", "gi");
        var t = item.label.replace(re, "<span class='highlight'>$1</span>");
        return $("<li></li>")
     .data("item.autocomplete", item)
     .append("<a>" + t + "</a>")
     .appendTo(ul);
    }; 
    
    //auto complete
    $("#search-input").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AutoComplete/Index",
                data: "{ 'prefix': '" + request.term + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(textStatus);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            window.location.replace("/Result?keyword="+ui.item.value);
        }
    });   

});

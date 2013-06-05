//js for admin

$(document).ready(function () {


    // Change order searching history list
    $("#order-search-history").change(function () {

        var url = window.location.href;
        var indexOfOrderBy = url.indexOf("orderby");
        var indexOfPage = url.indexOf("?");

        if (indexOfPage != -1) {
            if (indexOfOrderBy != -1) {
                url = url.substr(0, indexOfOrderBy) + "orderby=" + $("#order-search-history").val();
            } else {
                url = window.location + "&orderby=" + $("#order-search-history").val();
            }
        } else {
            url = window.location + "?orderby=" + $("#order-search-history").val();
        }
        window.location.href = url;

    });




    // Add synonyms

    var synonyms = new Array();
    $("#list-synonyms li").each(function (index) {
        console.log(index + ": " + $(this).children().children().first().text());
        synonyms.push($(this).children().children().first().text());
    });

    $("#add-synonym").click(function () {
        // Check contains in list li
        if ($.trim($("#input-synonym").val()) == "") {
            alert("Chưa nhập từ!");
            $("#input-synonym").focus();
        } else {
            if (synonyms.indexOf($("#input-synonym").val()) == -1) {
                synonyms.push($("#input-synonym").val());
                $("#list-synonyms").append("<li><span class='label label-success'><span>" + $("#input-synonym").val() + "</span> <a title='xóa' class='delete-synonym' href='javascript:'>x</a></span></li>")
                $("#input-synonym").val("");
                $("#input-synonym").focus();
            }
        }
    });

    // Remove synonym
    $(".delete-synonym").live('click', function () {
        $(this).parent().parent().remove();
        var i = synonyms.indexOf($(this).prev().text());
        synonyms.splice(i, 1);
    });


    // Edit synset form -- add array synonym and send request to edit
    $("#edit-synset-form").submit(function () {
        $("#synonyms-data").val(synonyms);
        return true;
    });


    // Delete comment
    $(".btn-comment-delete").click(function () {

        var checkDelete = confirm('Bạn có chắc muốn xóa không?');

        var row = $(this).parent().parent();

        if (checkDelete) {
            $.ajax({
                url: '/Comment/Delete',
                type: 'POST',
                data: "{ 'commentId': '" + $(this).children().val() + "'}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.message == 'SUCCESS') {
                        row.slideUp('slow');
                    } else {
                        alert("Lỗi");
                    }
                },
                error: function () {
                    alert("error");
                }
            });
        }
    });

    // Clear reported comment
    $(".btn-accept-cm").click(function () {


        var row = $(this).parent().parent();

        $.ajax({
            url: '/Comment/ClearReport',
            type: 'POST',
            data: "{ 'commentId': '" + $(this).children().val() + "'}",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.message == 'SUCCESS') {
                    row.removeClass('reported');
                    row.children().find('.label-success').remove();
                    row.children().find('.btn-accept-cm').remove();                    
                } else {
                    alert("Lỗi");
                }
            },
            error: function () {
                alert("error");
            }
        });

    });


    // Delete search history
    $(".btn-search-history-delete").click(function () {

        var checkDelete = confirm('Bạn có chắc muốn xóa không?');

        var row = $(this).parent().parent();

        if (checkDelete) {
            $.ajax({
                url: '/ManageSearchHistory/Delete',
                type: 'POST',
                data: "{ 'keyword': '" + $(this).children().val() + "'}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.message == 'SUCCESS') {
                        row.slideUp('slow');
                    } else {
                        alert("Lỗi");
                    }
                },
                error: function () {
                    alert("error");
                }
            });
        }
    });


    // Delete User
    $("#btn-delete-user").click(function () {

        var checkDelete = confirm("Bạn có chắc muốn xóa thành viên này!");
        if (checkDelete) {
            $.ajax({
                url: '/Users/Delete',
                type: 'POST',
                data: "{ 'userId': '" + $("#userId").val() + "'}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.message == 'SUCCESS') {
                        alert("Xóa thành công!");
                        window.location.href = '/Users';
                    } else {
                        alert("Lỗi");
                    }
                },
                error: function () {
                    alert("error");
                }
            });
        }
    });


    // Upload file and import
    $("#ajaxUploadForm").ajaxForm({
        iframe: true,
        type: 'POST',
        dataType: "json",
        cache: false,
        timeout: 1200000,
        async: false,
        beforeSubmit: function () {
            // Do something here if needed like show in progress message
            var check = $("#import-file").val();
            if (check != '')
            { popUpLoadingOpen(); }
            else {
                alert("Bạn chưa chọn file dữ liệu!");
                return;
            }
        },
        success: function (result) {
            popUpLoadingClose();
            if (result.data == "SUCCESS") {
                alert('Nhập thành công ' + result.rows + ' thuật ngữ và nghĩa.');
            } else {
                alert(result.message);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            popUpLoadingClose();
            //alert("Error uploading file");
        }
    });


    // DELETE SYNSET OF TERM
    $(".synset-delete").click(function () {

        var checkDelete = confirm('Bạn có chắc muốn xóa không?');

        //var row = $(this).parent().parent();

        if (checkDelete) {
            $.ajax({
                url: '/ManageEntries/Delete',
                type: 'POST',
                data: "{ 'synsetId': '" + $(this).attr("data-synset") + "','headWord': '" + $(this).attr("data-index") + "'}",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.message == 'SUCCESS') {
                        alert("Xóa thành công!");
                        window.location.href = '/ManageEntries';
                    } else {
                        alert("Lỗi");
                    }
                },
                error: function () {
                    alert("error");
                }
            });
        }
    });

});

/*LOADING POPUP FUNCTION*/
//-------------------------------
function popUpLoadingOpen() {
    //over-lay
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();
    $('.ui-widget-overlay').css("width", windowWidth + "px");
    $('.ui-widget-overlay').css("height", windowHeight + "px");
    $('.ui-widget-overlay').css("opacity", 0.5);
    $('.ui-widget-overlay').show();

    //popup div
    var top = windowHeight / 2 - $('.ui-dialog').height() / 2;
    var left = windowWidth / 2 - $('.ui-dialog').width() / 2;
    //console.log(top + "-" + left)
    $('#loading-popup').css("top", top + "px");
    $('#loading-popup').css("left", left + "px");
    $('#loading-popup').slideDown('slow');
}

function popUpLoadingClose() {
    $('.ui-widget-overlay').hide();
    $('#loading-popup').hide();
}
//-------------------------------------
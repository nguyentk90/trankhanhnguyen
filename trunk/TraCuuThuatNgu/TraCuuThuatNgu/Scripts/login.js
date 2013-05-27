$(document).ready(function () {

    /*LOGIN*/
    //------------------------------------------------
    //dropdown-menu user login
    $('#user-login').click(function () {
        $('.dropdown-menu').slideToggle('slow');
    });

    //close menu when click outside form this
    $(document).mouseup(function (e) {
        console.log(e.target.id);
        if (e.target.id != 'user-login') {
            var container = $('.dropdown-menu');
            if (container.has(e.target).length == 0) {
                container.hide();
            }
        }
    });


    //Open logon popup
    $('.popUpLogon').click(popUpLogonOpen);

    //Close logon popup
    $('.close-popup').click(popUpLogonClose);

    //-----------------------------------------------------------
    
     //Open Add Content popup
    $('#add-content-btn').click(popUpAddContentOpen);

    //Close Add Content popup
    $('.close-popup').click(popUpAddContentClose);


    /*LIKE*/
    //-----------------------------------------------
    //like button
    $('.like-check').click(function () {
        var checkLogOn = confirm('Hãy đăng nhập dùng chức năng này.');
        if (checkLogOn) {
            popUpLogonOpen();
        }
    });

    //like button
    $('#like-submit').click(function () {
        $.ajax({
            url: '/Like/Add',
            type: 'POST',
            data: "{ 'headWord': '" + $('#headWord').text() + "'}",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.message == 'SUCCESS') {
                    var counter = $('.count-like').html();
                    counter++;
                    $('.count-like').html(counter);
                    alert("Đã thêm vào thuật ngữ yêu thích.");
                } else if (data.message == 'EXISTED') {
                    alert("Bạn đã thích từ này");
                } else {
                    alert("Lỗi thêm yêu thích");
                }
            },
            error: function () {
                alert("error");
            }
        });
    });


    /*VALIDATE*/
    //----------------------------------------------------
    //check space and special characters
    $("#search-form").submit(function () {
        if ($.trim($("#search-input").val()) == "") {
            alert("Bạn chưa nhập từ");
            return false;            
        } else { 
            var regex = /[~`!#@$%\^&*+=\-\[\]\\;,/{}|\\"':<>\?]/g ;   
            if(regex.test($("#search-input").val()))
            {
                alert("Không nhập ký tự đặc biệt.");
                return false;
            }
            return true;                       
        }
    });

    //check space comment form
    $("#comment-form").submit(function () {
        if ($.trim($("#comment-content").val()) == "") {
            alert("Bạn chưa nhập nhận xét");
            $("#comment-content").focus();
            return false;            
        } else {             
            return true;                       
        }
    });



    /*DELETE USER HISTORY*/
    $(".btn-history").click(function(){
            
        var checkDelete = confirm('Bạn có chắc muốn xóa không?');

        var row = $(this).parent().parent();

        if (checkDelete) {        
            $.ajax({
                url: '/UserHistory/Delete',
                type: 'POST',
                data: "{ 'historyId': '" + $(this).children().val() + "'}",
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


    /*DELETE USER FAVORITES*/
    $(".btn-like").click(function(){
            
        var checkDelete = confirm('Bạn có chắc muốn xóa không?');

        var row = $(this).parent().parent();

        if (checkDelete) {        
            $.ajax({
                url: '/Like/Delete',
                type: 'POST',
                data: "{ 'headWord': '" + $(this).children().val() + "'}",
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

    /*DELETE USER ADD CONTENT*/
    $(".btn-add-content").click(function(){
            
        var checkDelete = confirm('Bạn có chắc muốn xóa không?');

        var row = $(this).parent().parent();

        if (checkDelete) {        
            $.ajax({
                url: '/AddContent/Delete',
                type: 'POST',
                data: "{ 'rawDataID': '" + $(this).children().val() + "'}",
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


    /*ADD CONTENT*/
    $("#add-content-submit").click(function () {
        if ($.trim($("#def").val()) == "") {
            alert("Bạn chưa nhập nội dung."); 
            $("#def").focus();           
        } else {         
           //$("#add-content-submit").css("background-color","#ccc");
           $.ajax({
                url: '/AddContent/Add',
                type: 'POST',
                data: "{ 'def': '" + $("#def").val() + "','catagory': '" + $("#catagory").val() + "','keyword': '" + $("#keyword").text() + "',"
                + "'exa': '" + $("#exa").val() + "'}",                
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.message == 'SUCCESS') {                        
                         alert('Cảm ơn bạn đã đóng góp nội dung!');
                         popUpAddContentClose();
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

    //close add content form
    $("#add-content-close").click(popUpAddContentClose);


});


/*LOGIN POPUP FUNCTION*/
//-------------------------------
function popUpLogonOpen() {
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
    $('#logon-popup').css("top", top + "px");
    $('#logon-popup').css("left", left + "px");
    $('#logon-popup').slideDown('slow'); 
}

function popUpLogonClose() {
    $('.ui-widget-overlay').hide();
    $('#logon-popup').hide();
}
//-------------------------------------

/*ADD CONTENT POPUP FUNCTION*/
//-------------------------------
function popUpAddContentOpen() {
    //over-lay
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();
    $('.ui-widget-overlay').css("width", windowWidth + "px");
    $('.ui-widget-overlay').css("height", windowHeight + "px");
    $('.ui-widget-overlay').css("opacity", 0.25);
    $('.ui-widget-overlay').show();

    //popup div
    var position =  $('#add-content-btn').position();    

    $('#add-content-popup').css("top", position.top + 15 + "px");
    $('#add-content-popup').css("left", position.left - 484 + 113 + "px");
    $('#add-content-popup').slideDown('slow');

    //clear form
    $("#def").val("");
    $("#exa").val("");
}

function popUpAddContentClose() {
    $('.ui-widget-overlay').hide();
    $('#add-content-popup').hide();
}
//-------------------------------------

//validate function
function validateRequired(id)
{
    var element = $("'#"+id+"'");
    if ($.trim(element.val()) == "") {
            alert("Bạn chưa nhập từ");
            return false;            
        } else {             
            return true;                       
        }
}
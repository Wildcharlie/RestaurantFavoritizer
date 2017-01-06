$(document).ready(function () {
    $('#feed').on('click', '.restaurant-add', function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        console.log(postData);
        $.ajax({
            url: '/MyFavorites/AddFavoriteRestaurant',
            data: postData,
            success: function (returnData) {
                $("#closeButton").trigger("click");
                $("#FavoriteList").prepend(returnData);
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });
});
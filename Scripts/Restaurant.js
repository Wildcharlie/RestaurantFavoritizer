$(document).ready(function(){
    $('.restaurant-remove').hover(
        function () {
            $(this).removeClass('btn-success');
            $(this).addClass('btn-danger');
            $('#UnfavoriteButton .restaurant-favorited').toggle();
            $('#UnfavoriteButton .restaurant-unfavorite').toggle();
        },
        function () {
            $(this).removeClass('btn-danger');
            $(this).addClass('btn-success');
            $('#UnfavoriteButton .restaurant-favorited').toggle();
            $('#UnfavoriteButton .restaurant-unfavorite').toggle();
        }
    );

    $('#FavoriteButton').click(function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/AddFavoriteRestaurant',
            context: this,
            data: postData,
            success: function (returnData) {
                $(this).removeClass('active');
                $('#UnfavoriteButton').addClass('active');
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#UnfavoriteButton').click(function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/RemoveFavoriteRestaurant',
            context: this,
            data: postData,
            success: function (returnData) {
                $(this).removeClass('active');
                $('.menu-item-add').removeClass('favorited');
                $('#FavoriteButton').addClass('active');
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#Save').click(function () {
        var form = $("#restaurantEventForm");
        var url = form.attr("action");
        var formData = form.serialize();
        console.log(formData);
        $.post(url, formData, function (data) {
            if (data) {
               location.reload();
            } else {
                $('#msg').html('<div class="alert alert-dismissible alert-danger"><button type="button" class="close" data-dismiss="alert">×</button>\
                                <span>Failed to edit restaurant!</span></div>');
            }
        });
    })
});
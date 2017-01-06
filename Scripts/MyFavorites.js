$(document).ready(function () {
    $('#Search').click(function () {
        var form = $("#searchEventForm");
        var url = form.attr("action");
        var formData = form.serialize();
        $.post(url, formData, function (data) {
            $("#searchTarget").html(data);
            $(".restaurant-add").click(function () {
                var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
                var postData = JSON.stringify(temp);
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
    });

    $('#FavoriteList').on('click', '.restaurant-delete', function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/RemoveFavoriteRestaurant',
            context: this,
            data: postData,
            success: function (returnData) {
                $(this).closest('.favorite-item').remove();
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#FavoriteList').on('click', '.favorite-food-delete', function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/RemoveFavoriteFood',
            context: this,
            data: postData,
            success: function (returnData) {
                $(this).closest('.list-group-item').remove();
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#FavoriteList').on('click', '.favorite-drink-delete', function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/RemoveFavoriteDrink',
            context: this,
            data: postData,
            success: function (returnData) {
                $(this).closest('.list-group-item').remove();
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#FavoriteList').on('click', '.favorite-employee-delete', function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/RemoveFavoriteEmployee',
            context: this,
            data: postData,
            success: function (returnData) {
                $(this).closest('.list-group-item').remove();
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $("form").submit(function (e) {
        e.preventDefault();
    });

    $("#addRestaurant").click(function () {
        $("#closeButton").trigger("click");
    });

    $("#addMenuItemModal").on("show.bs.modal", function (event) {
        if (!$('#addMenuItemModal').data('bs.modal').isShown) {
            $('#addMenuItemTarget').html('<div class="loading text-center"><i class="fa fa-spinner fa-spin fa-3x"></i></div>')
            var postData = event.relatedTarget.dataset.json;
            $.ajax({
                url: '/Restaurant/ShowMenu',
                data: postData,
                success: function (returnData) {
                    $('#addMenuItemTarget').html(returnData);
                },
                type: 'POST',
                contentType: 'application/json, charset=utf-8'
            });
        }
    });

    $("#addMenuItemModal").on("hidden.bs.modal", function (event) {
        if (!$('#addMenuItemModal').data('bs.modal').isShown) {
            $('#foodModal').modal('hide');
            $('#drinkModal').modal('hide');
        }
    });

    $('#PrivacyFormSubmit').click(function () {
        var isPublic = {isPublic: (!$("#isPublic").prop("checked"))};
        var temp = $.extend({}, $('#userInfo').data('json'), isPublic);
        var postData = JSON.stringify(temp);
        $.ajax({
            url: '/MyFavorites/TogglePrivacy',
            context: this,
            data: postData,
            success: function (returnData) {
                $('#PrivacyMsg').html('<div class="alert alert-dismissible alert-success"><button type="button" class="close" data-dismiss="alert">×</button>\
                        <span>Privacy Setting Saved.</span></div>');
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#addRestaurant').click(function () {
        $('body').addClass('modal-open');
    });

});
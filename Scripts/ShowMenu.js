$(document).ready(function () {
    $('#SaveFood').click(function () {
        if ($('#FoodItemName').val().length < 3) {
            return;
        }
        var form = $("#eventFoodForm");
        var url = form.attr("action");
        var formData = form.serialize();
        $.post(url, formData, function (data) {
            if (data) {
                $('#FoodContainer').prepend(data);
                $('#msgFood').html('<div class="alert alert-dismissible alert-success"><button type="button" class="close" data-dismiss="alert">×</button>\
                            <span>New Food Added. Thank you for helping to improve Favoritizer.</span></div>');
            } else {
                $('#msgFood').html('<div class="alert alert-dismissible alert-danger"><button type="button" class="close" data-dismiss="alert">×</button>\
                                    <span>Failed to add food! Make sure all forms are filled out.</span></div>');
            }
        });
    });

    $('#SaveDrink').click(function () {
        if ($('#DrinkItemName').val().length < 3) {
            return;
        }
        var form = $("#eventDrinkForm");
        var url = form.attr("action");
        var formData = form.serialize();
        $.post(url, formData, function (data) {
            if (data) {
                $('#DrinkContainer').prepend(data);
                $('#msgDrink').html('<div class="alert alert-dismissible alert-success"><button type="button" class="close" data-dismiss="alert">×</button>\
                                <span>New Drink Added. Thank you for helping to improve Favoritizer.</span></div>');
            } else {
                $('#msgDrink').html('<div class="alert alert-dismissible alert-danger"><button type="button" class="close" data-dismiss="alert">×</button>\
                                        <span>Failed to add drink! Make sure all forms are filled out.</span></div>');
            }
        });
    });

    $('#SaveEmployee').click(function () {
        if ($('#EmployeeName').val().length < 3) {
            return;
        }
        var form = $("#eventEmployeeForm");
        var url = form.attr("action");
        var formData = form.serialize();
        $.post(url, formData, function (data) {
            if (data) {
                $('#EmployeeContainer').prepend(data);
                $('#msgEmployee').html('<div class="alert alert-dismissible alert-success"><button type="button" class="close" data-dismiss="alert">×</button>\
                            <span>New Employee Added. Thank you for helping to improve Favoritizer.</span></div>');
            } else {
                $('#msgEmployee').html('<div class="alert alert-dismissible alert-danger"><button type="button" class="close" data-dismiss="alert">×</button>\
                                    <span>Failed to add employee! Make sure all forms are filled out.</span></div>');
            }
        });
    });

    $('#MenuContainer').on('click', '.menu-item-add', function () {
        var temp = $.extend({}, $('#userInfo').data('json'), $(this).data('json'));
        var postData = JSON.stringify(temp);
        console.log(postData);
        var postUrl = $(this).data('action');
        $.ajax({
            url: postUrl,
            data: postData,
            context: this,
            success: function (returnData) {
                if (returnData) {
                    if ($('#FavoriteList').length > 0) {
                        $('#addMenuItemCloseButton').trigger('click');
                        var target = $(this).data('container');
                        var type = $(this).data('type')
                        var spot = $('#' + target).find('.' + type);
                        if (spot.length > 0) {
                            spot.replaceWith(returnData);
                        } else {
                            $('#' + target).append(returnData);
                        }
                    }
                    $('.menu-item-add').removeClass('favorited');
                    $(this).addClass('favorited');
                }
            },
            type: 'POST',
            contentType: 'application/json, charset=utf-8'
        });
    });

    $('#MenuContainer').on('click', '.menu-item-likes', function () {
        if (!$(this).hasClass("liked")) {
            var postData = JSON.stringify($(this).data('json'));
            var postUrl = $(this).data('action');
            console.log(postData);
            $.ajax({
                url: postUrl,
                data: postData,
                context: this,
                success: function (returnData) {
                    if (returnData) {
                        var dislike = $(this).parent().children('.disliked');
                        if (dislike) {
                            dislike.removeClass("disliked")
                            dislikeCount = parseInt(dislike.find('.dislikeCount').text());
                            dislike.find('.dislikeCount').text(dislikeCount - 1);
                            dislike.prop("title", "I Dislike This");
                        }
                        $(this).addClass("liked");
                        var likes = parseInt($(this).find('.likeCount').text());
                        $(this).find('.likeCount').text(likes + 1);
                        $(this).prop("title", "Undo Like");
                    }
                },
                type: 'POST',
                contentType: 'application/json, charset=utf-8'
            });
        } else {
            var postData = JSON.stringify($(this).data('json'));
            postData.like = 2;
            var postUrl = $(this).data('action');
            $.ajax({
                url: postUrl,
                data: postData,
                context: this,
                success: function (returnData) {
                    if (returnData) {
                        $(this).removeClass("liked");
                        var likes = parseInt($(this).find('.likeCount').text());
                        $(this).find('.likeCount').text(likes - 1);
                        $(this).prop("title", "I Like This");
                    }
                },
                type: 'POST',
                contentType: 'application/json, charset=utf-8'
            });
        }
    });

    $('#MenuContainer').on('click', '.menu-item-dislikes', function () {
        if (!$(this).hasClass("disliked")) {
            var postData = JSON.stringify($(this).data('json'));
            var postUrl = $(this).data('action');
            $.ajax({
                url: postUrl,
                data: postData,
                context: this,
                success: function (returnData) {
                    if (returnData) {
                        var like = $(this).parent().children('.liked');
                        if (like) {
                            like.removeClass("liked")
                            likeCount = parseInt(like.find('.likeCount').text());
                            like.find('.likeCount').text(likeCount - 1);
                            like.prop("title", "I Like This");
                        }

                        $(this).addClass("disliked");
                        var dislikes = parseInt($(this).find('.dislikeCount').text());
                        $(this).find('.dislikeCount').text(dislikes + 1);
                        $(this).prop("title", "Undo Dislike");
                    }
                },
                type: 'POST',
                contentType: 'application/json, charset=utf-8'
            });
        } else {
            var postData = JSON.stringify($(this).data('json'));
            postData.like = 2;
            var postUrl = $(this).data('action');
            $.ajax({
                url: postUrl,
                data: postData,
                context: this,
                success: function (returnData) {
                    if (returnData) {
                        $(this).removeClass("disliked");
                        var dislikes = parseInt($(this).find('.dislikeCount').text());
                        $(this).find('.dislikeCount').text(dislikes - 1);
                        $(this).prop("title", "I Dislike This");
                    }
                },
                type: 'POST',
                contentType: 'application/json, charset=utf-8'
            });
        }
    });

    $('#foodModalCloseButton').click(function () {
        $('#foodModal').modal('hide');
    });

    $('#drinkModalCloseButton').click(function () {
        $('#drinkModal').modal('hide');
    });

    $('#employeeModalCloseButton').click(function () {
        $('#employeeModal').modal('hide');
    });


});
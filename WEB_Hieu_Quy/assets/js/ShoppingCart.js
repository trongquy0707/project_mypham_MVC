/*/*    const { data } = require("jquery");*/

$(document).ready(function () {
    ShowCount();
    $("body").on('click', '.cart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('#quantity').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        /* alert(id + " " + quatity)*/

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_cart').html(rs.count);
                    alert(rs.msg);
                }
            }
        });
    });




    // updatecart
    $('body').on('click', '.prev', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quatity = $('#quantity_' + id).text();
  
        Update(id, quatity);

    });

    $('body').on('click', '.next', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quatity = $('#quantity_'+id).text();
        //var tQuantity = $('#quantity'+id).text();
        //if (tQuantity != '') {
        //    quatity = parseInt(tQuantity);
        //}
        Update(id, quatity);

    });

});


function ShowCount() {
    $.ajax({
        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_cart').html(rs.Count);
        }
    });
}

function Update(id, quantity) {
    $.ajax({
        url: '/ShoppingCart/updatequantity',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                location.reload();
            }
        }
    });
}

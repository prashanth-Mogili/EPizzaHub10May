function AddToCart(itemId, name, unitPrice, quantity) {
    $.ajax({
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: '/Cart/AddToCart/' + itemId + "/" + unitPrice + "/" + quantity,
        success: function (response) {
            if (response.status == 'success') {
                //alert(name + " added to cart successfully.");
                var counter = response.count;
                $("#cartCounter").text(counter);
            }
        }
    });
}


function deleteItem(id) {
    if (id > 0) {
        $.ajax({
            type: "GET",
            url: '/Cart/DeleteItem/' + id,
            success: function (data) {
                if (data > 0) {
                    location.reload();
                }
            }
        });
    }
}

function updateQuantity(id, currentQuantity, quantity) {
    if ((currentQuantity >= 1 && quantity == 1) || (currentQuantity > 1 && quantity == -1)) {
        $.ajax({
            url: '/Cart/UpdateQuantity/' + id + "/" + quantity,
            type: 'GET',
            success: function (response) {
                if (response > 0) {
                    location.reload();
                }
            }
        });
    }
}


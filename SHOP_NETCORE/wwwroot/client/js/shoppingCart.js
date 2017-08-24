var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.addItem(productId);
        });

        $('.btnDelete').off('click').on('click', function () {
            var productId = parseInt($(this).data('id'));
            cart.DeleteItem(productId);
        });

        $('.txtQuantity').off('change').on('change', function () {
            var quantity = parseInt($(this).val());
            //var productId = parseInt($(this).data('id'));
            var price = 1000000;//parseFloat($(this).data('price'));

            if (isNaN(quantity) == false) {
                var amount = quantity * price;

                //$('#amount_' + productId).text(amount);
                alert(amount);
            }
            else {
                $('#amount_' + productId).text(0);
            }
            //$('#totalMoney').text(cart.getTotalMoney());
        });

        //$('.txtQuantity').off('keyup').on('keyup', function () {
        //    var quantity = parseInt($(this).val());
        //    var productId = parseInt($(this).data('id'));
        //    var price = parseFloat($(this).data('price'));

        //    if (isNaN(quantity) == false) {
        //        var amount = quantity * price;

        //        $('#amount_' + productId).text(amount);
        //    }
        //    else {
        //        $('#amount_' + productId).text(0);
        //    }
        //    $('#totalMoney').text(cart.getTotalMoney());
        //});

        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            cart.deleteAll();
        });

        $('#btnCheckOut').off('click').on('click', function () {
            $('#divCheckout').show();
        });

        //$('#checkLoginInfo').off('click').on('click', function () {
        //    if ($(this).prop("checked")) {
        //        cart.getLoginUser();
        //    }
        //    else {
        //        $('#txtHoten').val("");
        //        $('#txtDiachi').val("");
        //        $('#txtEmail').val("");
        //        $('#txtDienthoai').val("");
        //    }
        //});

        $('#btnCreateOrder').off('click').on('click', function () {
            cart.createOrder();
        });

    },
    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (responce) {
                if (responce.status) {
                    var html = '';
                    var template = $('#tplCart').html();
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            Id: item.productId,
                            ProductCode: item.product.productCode,
                            Name: item.product.name,
                            Images: item.product.images,
                            Price: item.product.price,
                            Quantity: item.quantity,
                            Amount: item.quantity * item.product.price
                        });
                    });

                    $('#cartBody').html(html);

                    if (html == '') {
                        $('#cartContent').html("Không có sản phẩm nào trong giỏ hàng");
                    }
                    // bắt buộc RegisterEvent
                    cart.registerEvent();
                }
            }
        })
    },
    addItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/Add',
            data: {
                productID: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status) {
                    alert("Thêm sản phẩm thành công");
                }
                else {
                    alert(responce.message);
                }
            }
        })
    },
    DeleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status) {
                    alert("xóa sản phẩm thành công");
                    cart.loadData();
                }
                else {
                    alert("xóa sản phẩm thất bại");
                }
            }
        })
    },
    getTotalMoney: function () {
        var lstTextBox = $('.txtQuantity');
        var total = 0;
        $.each(lstTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },

    deleteAll: function () {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',

            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status) {
                    cart.loadData();
                }
            }
        })
    },

    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ID: $(item).data('id'),
                Quantity: $(item).val()
            });
        });
        $.ajax({
            url: '/ShoppingCart/Update',
            data: {
                cartData: JSON.stringify(cartList)
            },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status) {
                    cart.loadData();
                }
            }
        })
    },

    //getLoginUser: function () {
    //    $.ajax({
    //        url: '/ShoppingCart/GetUser',

    //        type: 'POST',
    //        dataType: 'json',
    //        success: function (responce) {
    //            if (responce.status) {
    //                var user = responce.data;
    //                $('#txtHoten').val(user.FullName);
    //                $('#txtDiachi').val(user.Address);
    //                $('#txtEmail').val(user.Email);
    //                $('#txtDienthoai').val(user.PhoneNumber);
    //            }
    //            else {
    //                alert("Bạn chưa đăng nhập");
    //            }
    //        }
    //    })

    //},
    createOrder: function () {
        var order = {
            CustomerName: $("#txtHoten").val(),
            CustomerAddress: $("#txtDiachi").val(),
            CustomerEmail: $("#txtEmail").val(),
            CustomerMobile: $("#txtDienthoai").val(),
            CustomerMessage: $("#txtMes").val(),
            PaymentMethod: "thanh toán tiền mặt",


            Status: false
        }
        $.ajax({
            url: '/ShoppingCart/CreateOrrder',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            type: 'POST',
            dataType: 'json',
            success: function (responce) {
                if (responce.status) {
                    $('#divCheckout').hide();
                    cart.deleteAll();
                    $('#cartContent').html("Cảm ơn bạn đã mua hàng, chúng tôi sẽ sớm liên lạc lại với bạn");
                }
                else {
                    alert("Mua hàng thất bại");
                }
            }
        })

    }
}
cart.init();
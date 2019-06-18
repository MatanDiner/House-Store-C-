function getCategory(renderCategory) {
    $.ajax({
        url: 'ajaxWebService.asmx/getCategory',
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset = utf-8',
        success: function (results) {
            renderCategory(results);
        },
        error: function (request, error) {
            alert('Network error has occurred please try again!');
        }
    });
}


function getProductsByCat(CategoryInfo, renderProducts) {

    // serialize the object to JSON string
    var dataString = JSON.stringify(CategoryInfo);

    $.ajax({
        url: 'ajaxWebService.asmx/getProductsByCat',
        data: dataString,
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset = utf-8',
        success: function (results) {
            renderProducts(results);
        },
        error: function (request, error) {
            alert('Network error has occurred please try again!');
        }
    });
}

function getProduct(ProductInfo, renderFullProduct) {

    // serialize the object to JSON string
    var dataString = JSON.stringify(ProductInfo);

    $.ajax({
        url: 'ajaxWebService.asmx/getProduct',
        data: dataString,
        type: 'POST',
        dataType: "json",
        contentType: 'application/json; charset = utf-8',
        success: function (results) {
            renderFullProduct(results);
        },
        error: function (request, error) {
            alert('Network error has occurred please try again!');
        }
    });
}

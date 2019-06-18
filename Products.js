CategoryInfo = new Object();
ProductInfo = new Object();
ImageSitePath='http://localhost:56647/';
ImageDefultPath='images/no-img.jpg';


$(document).on('pagebeforeshow', '#homePage', function () {
    getCategory(renderCategory);
});

function renderCategory(results) {
    //this is the callBackFunc 
    results = $.parseJSON(results.d);
    $('#categoryList').empty();
    $.each(results, function (i, row) {
        console.log(JSON.stringify(row));
        dinamicli = '<li><a href="" data-id=' + row.Id + '><h3>' + row.Name +'</h3></a></li>'
        $('#categoryList').append(dinamicli);

    });
    $('#categoryList').listview('refresh');
}


$(document).on('vclick', '#categoryList li a', function () {
    CategoryInfo.id = $(this).attr('data-id');
    $.mobile.changePage('#searchProductPage', { transition: "slide",changeHase:false});

});

$(document).on('pagebeforeshow', '#searchProductPage', function () {

    getProductsByCat(CategoryInfo, renderProducts);
});

function renderProducts(results) {
    //this is the callBackFunc 
    results = $.parseJSON(results.d);
    $('#ProductList').empty();
    $.each(results, function (i,row) {
        console.log(JSON.stringify(row));
        if (row.ImagePath == null) { row.ImagePath = ImageSitePath + ImageDefultPath; }
        dinamicli = '<li><a href="" data-id=' + row.Id + '><img src=' + row.ImagePath + ' /><h3>' + row.ProductName + '</h3><p>price:' + row.Price + '</p><p> Inventory:' + row.Inventory + '</p></a></li>';
        $('#ProductList').append(dinamicli);
    });
    $('#ProductList').listview('refresh');
}


$(document).on('vclick', '#ProductList li a', function () {
    ProductInfo.id = $(this).attr('data-id');
    $.mobile.changePage('#productPage',{transition:"slide",changeHase:false})
});
 
$(document).on('pagebeforeshow', '#productPage', function () {
    getProduct(ProductInfo, renderFullProduct);
});


function renderFullProduct(results) {
    //this is the callBackFunc 
    results = $.parseJSON(results.d);
    $('#ph').empty();
    $('#title').html(results.ProductName);
    if (results.ImagePath == null) { results.ImagePath = ImageSitePath + ImageDefultPath; }
    $('#imgProduct').attr('src', results.ImagePath);
    $('#inventory').html("Inventory: " + results.Inventory);
  
    
}




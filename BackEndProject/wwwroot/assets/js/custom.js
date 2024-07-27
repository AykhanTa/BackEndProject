$(document).ready(function () {
    //addBasket
    $(".addToBasket").click(function (ev) {
        ev.preventDefault();
        var id = $(this).data("id");
        axios.get("/basket/addbasket?id="+id)
            .then(function (datas) {
                $(".minicart-content-box").html(datas.data);
            })
    })




    //search
    $(document).on("keyup", "#searchInput", function () {
        $("#searchList").html("")
        let searchValue = $(this).val();
        axios.get("/product/searchProduct?search="+searchValue)
            .then(function (datas) {
                $("#searchList").html(datas.data)
            })
    })



    //modal
    $(".productModal").click(function (ev) {
        ev.preventDefault();
        let url = $(this).attr("href");
        axios.get(url)
            .then(function (response) {
                // handle success
                $(".modal-body").html(response.data)

                $('.product-large-slider').slick({
                    fade: true,
                    arrows: false,
                    asNavFor: '.pro-nav'
                });


                // product details slider nav active
                $('.pro-nav').slick({
                    slidesToShow: 4,
                    asNavFor: '.product-large-slider',
                    arrows: false,
                    focusOnSelect: true
                });
            })
    })
})
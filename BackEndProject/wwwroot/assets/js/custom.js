$(document).ready(function () {
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
$(document).ready(function() {

    //This function enables sticky navbar functionality. To make it work uncomment the next line
    //enableStickyNav();

    function enableStickyNav() {
        $(".navbar").sticky({
            topSpacing: 0,
            className: "navbar-fixed"
        });
    }


    $('.carousel').carousel({
        interval: 5000,
        pause: 'hover'
    });

    // Scroll to top
 //   $().UItoTop({ easingType: 'easeOutQuart' });

    // Aside Menu
    $("#cmdAsideMenu, #btnHideAsideMenu, .navbar-toggle-aside-menu").click(function () {
        if ($("#asideMenu").is(":visible")) {
            $("#asideMenu").hide();
            $("body").removeClass("aside-menu-in");
        }
        else {
            $("body").addClass("aside-menu-in");
            $("#asideMenu").show();
        }
        return false;
    });
});
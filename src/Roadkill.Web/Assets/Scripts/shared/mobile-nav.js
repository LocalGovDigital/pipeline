$(document).ready(function () {

    var $mobileNav = $(".mobile-nav");
    if ($mobileNav) {
        $mobileNav.each(function () {
            var $parent = $(this);
            var $handle = $parent.find("button");
            var $target = $(".mobile-nav-content");

            $handle.on("click", function () {
                $target.toggleClass("navigation-in");
                $("body").toggleClass("lock-scroll");

                this.wait = setTimeout(function () {
                    $target.toggleClass("navigation-show");
                    clearTimeout(this.wait);
                }, 100);
            });

        });
    }
});
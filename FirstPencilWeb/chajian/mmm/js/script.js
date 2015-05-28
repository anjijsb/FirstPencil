$(function () {
    start();
    var mySwiper = new Swiper('.swiper-container', {
        paginationClickable: false,
        slidesPerView: 1,
        mode: 'vertical',
        onTouchEnd: function () {
            var info = $(mySwiper.activeSlide()).find('.info');
            var info1 = info.find('.info1');
            var info2 = info.find('.info2');
            var info3 = info.find('.info3');
            var info4 = info.find('.info4');
            var info5 = info.find('.info5');
            var info6 = info.find('.info6');
            var info7 = info.find('.info7');
            var info8 = info.find('.info8');
            var info9 = info.find('.info9');
            var info10 = info.find('.info10');
            $(".info1,.info2,.info3,.info4,.info5,.info6,.info7,.info8,.info9,.info10").css('opacity', '0');
            setTimeout(function () {
                info1.animate({ opacity: 1 }, 800);
            }, 200);
            setTimeout(function () {
                info2.animate({ opacity: 1 }, 800);
            }, 1000);
            setTimeout(function () {
                info3.animate({ opacity: 1 }, 800);
            }, 1600);
            setTimeout(function () {
                info4.animate({ opacity: 1 }, 800);
            }, 2200);
            setTimeout(function () {
                info5.animate({ opacity: 1 }, 800);
            }, 2800);
            setTimeout(function () {
                info6.animate({ opacity: 1 }, 800);
            }, 3400);
            setTimeout(function () {
                info7.animate({ opacity: 1 }, 800);
            }, 4000);
            setTimeout(function () {
                info8.animate({ opacity: 1 }, 800);
            }, 4600);
            setTimeout(function () {
                info9.animate({ opacity: 1 }, 800);
            }, 5200);
            setTimeout(function () {
                info10.animate({ opacity: 1 }, 800);
            }, 5800);
        }
    })

    $('.music').on('click', function () {
        if ($(this).hasClass('on')) {
            $('audio').get(0).pause();
            $(this).removeClass('on music-off');
            $(this).attr('src', 'http://www.anjismart.com/FirstPencilWeb/chajian/mmm/images/off.png');
        } else {
            $('audio').get(0).play();
            $(this).addClass('on music-off');
            $(this).attr('src', 'http://www.anjismart.com/FirstPencilWeb/chajian/mmm/images/on.png');
        }
    })

    function start() {
        var info = $(".n2").find('.info');
        var info1 = info.find('.info1');
        var info2 = info.find('.info2');
        var info3 = info.find('.info3');
        var info4 = info.find('.info4');
        var info5 = info.find('.info5');
        $(".info1,.info2,.info3,.info4,.info5").css('opacity', '0');
        setTimeout(function () {
            info1.animate({ opacity: 1 }, 800);
        }, 200);
        setTimeout(function () {
            info2.animate({ opacity: 1 }, 800);
        }, 800);
        setTimeout(function () {
            info3.animate({ opacity: 1 }, 800);
        }, 1400);
        setTimeout(function () {
            info4.animate({ opacity: 1 }, 800);
        }, 2000);
        setTimeout(function () {
            info5.animate({ opacity: 1 }, 800);
        }, 2600);
    }

    $(".swiper-slide-small").find(".infoimg").each(function () {
        $(this).css("width", "80%");
    })
})
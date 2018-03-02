$(function () {
    $('#actionButton').click(function () {
        invokeCSCode($('#callbackData').val());
    });

    // remove existing highlights, then find the next occurrence and highlight it
    ////$('#trigger').click(function () {
    ////    if (index == 0) {
    ////        highlightAll('ipsum');
    ////        index = 1;
    ////    } else {
    ////        scrollMe('ipsum');
    ////    }

    ////});

    function removeAllHighlights(searchText) {
        $('#mainContent:contains("<span class="highlight">' + searchText + '</span>")').each(function () {
            $(this).html($(this).html().replace(new RegExp(searchText, 'g'), searchText));
            return false;
        });
    }

    function highlightAll(searchText) {
        $('#mainContent:contains("' + searchText + '")').each(function () {
            $(this).html($(this).html().replace(new RegExp(searchText, 'g'), '<span class="highlight">' + searchText + '</span>'));
            $(this).find('.highlight').fadeIn("slow");
            
            var offset = $(this).offset().top;
            $('html,body').animate({
                scrollTop: offset
            }, 500);
            return false;
        });
    }
    
    // stores the currently highlighted occurence
    var index = 0;
    var searchTextLocal;

    $('#trigger').click(function () {
        highlightNext(searchTextLocal);
    });

    function highlightNext(searchText) {
      
        var allHighlighted = $('.highlight');
        if (index == allHighlighted.length) {
            index = 0;
        }
     
        if (allHighlighted.length <= 0) {
            highlightAll(searchText);
            allHighlighted = $('.highlight');
            index = 0;
        }

        // Reset active/highlighted item(s)
        allHighlighted.find('span.highlightActive').replaceWith(searchText);

        var $next = allHighlighted.eq(index++);
        $next.html($next.html().replace(searchText, '<span class="highlightActive">' + searchText + '</span>'));

        var offset = $next.offset().top - 60;
        console.log("scroll to offset=" + offset);

        $('html,body').animate({
            scrollTop: offset
        }, 100);
        return false;
    };

    window.highlightNext = highlightNext;


    function searchHighlight(searchText) {
        if (searchTextLocal == searchText) {
            highlightNext(searchText);
            
        } else {
            removeAllHighlights(searchTextLocal);
            index = 0;
            searchTextLocal = searchText;
            highlightAll(searchText);
            highlightNext(searchText);
        }
    }

    window.searchHighlight = searchHighlight;

    // scroll our trigger link when the screen moves so we can click it again
    $(window).scroll(function () {
        var top = $(window).scrollTop();
        $('#trigger').offset({ top: top, left: 0 });
    });

});
$(function () {

    // JavaScript to C# action returns string to C#
    $('#actionButton').click(function () {
        invokeCSCode($('#callbackData').val());
    });

    // the input field
    var $input = $("input[type='search']"),
        // clear button
        $clearBtn = $("button[data-search='clear']"),
        // prev button
        $prevBtn = $("button[data-search='prev']"),
        // next button
        $nextBtn = $("button[data-search='next']"),
        // the context where to search
        $content = $(".content"),
        // jQuery object to save <mark> elements
        $results,
        // the class that will be appended to the current
        // focused element
        currentClass = "current",
        // top offset for the jump (the search bar)
        offsetTop = 100,
        // the current index of the focused element
        currentIndex = 0,

        // Local search text cache
        searchTextLocal;

    /**
     * Jumps to the element matching the currentIndex
     */
    function jumpTo() {
        if ($results != undefined && $results.length) {
            var position,
                $current = $results.eq(currentIndex);
            $results.removeClass(currentClass);
            if ($current.length) {
                $current.addClass(currentClass);
                position = $current.offset().top - offsetTop;
                window.scrollTo(0, position);
            }
        }
    }

    /**
     * Searches for the entered keyword in the
     * specified context
     */
    function markSearchText(searchText) {
        $content.unmark({
            done: function () {
                $content.mark(searchText, {
                    separateWordSearch: true,
                    done: function () {
                        $results = $content.find("mark");
                        currentIndex = 0;
                        jumpTo();
                    }
                });
            }
        });
    }

    /**
     * Searches for the entered keyword in the
     * specified context on input
     */
    $input.on("input", function () {
        var searchText = this.value;
        markSearchText(searchText);
    });

    /**
     * Clears the search
     */
    $clearBtn.on("click", function () {
        $content.unmark();
        $input.val("").focus();
    });

    /**
     * Moves the currently highlighted search text
     * by +1 (next) or -1 (previous).
     */
    function moveCurrent(step) {
        if ($results != undefined && $results.length) {
            currentIndex += step;
            if (currentIndex < 0) {
                currentIndex = $results.length - 1;
            }
            if (currentIndex > $results.length - 1) {
                currentIndex = 0;
            }
            jumpTo();
        }
    }

    /**
     * Next and previous search jump to
     */
    $nextBtn.add($prevBtn).on("click", function () {
        var step = $(this).is($prevBtn) ? -1 : 1;
        moveCurrent(step);
    });

    /**
     * Search text update method called from C# code.
     */
    function searchHighlight(searchText) {
        if (searchText == searchTextLocal) {
            moveCurrent(1);
        } else {
            searchTextLocal = searchText;
            if (searchText == undefined || searchText == '') {
                $content.unmark();
            }
            else {
                markSearchText(searchText);
            }
        }
    }

    window.searchHighlight = searchHighlight;
});

$(function () {
    $("#btnSearch").click(function () {
        var self = this;
        $(self).text("In Progress...");
        $(self).attr("disabled", true);

        $("#Results").val("");
        $('.table').html("");

        setPagination(1, 10, 1);
        
        setTimeout(function () {

            var url = $('#SearchApiUrl').val();// "../api/locks/search/";
            $.getJSON(url, { term: $("#terms").val(), page: $("#Page").val() })
                .done(function (json) {
                    console.log("JSON Data: ");
                    console.log(json);
                    var val = '';

                    val = setResult("", json.result.Total, json.result.Results);
                    setPagination(json.result.Page, json.result.PageSize, json.result.PageCount);

                    $("#Results").val(val);

                    createTableFromJson('.table1', json.result.Results);

                    $(self).text("Search");
                    $(self).removeAttr("disabled");
                })
                .fail(function (jqxhr, textStatus, error) {
                    setPagination(1, 10, 1);
                    var err = textStatus + ", " + error;
                    console.log("Request Failed: " + err);
                    $("#Results").val("Request Failed: " + err);
                    $(self).text("Search");
                    $(self).removeAttr("disabled");
                });

        }, 1000);
    });
    setPagination(1, 10, 1);

    $(".pager li:first").click(function () {
        var page = $("#Page").val();
        var pageSize = $("#PageSize").val();
        var pageCount = $("#PageCount").val();
        if (page == 1)
            navigate(page);
        else
            navigate(--page);
    });

    $(".pager li:last").click(function () {
        var page = $("#Page").val();
        var pageSize = $("#PageSize").val();
        var pageCount = $("#PageCount").val();

        if (page == pageCount)
            navigate(page);
        else
            navigate(++page);
    });

    function navigate(page) {

        $("#Results").val("");
        $('.table').html("");

        var url = $('#SearchApiUrl').val();// "../api/locks/search/";
        $.getJSON(url, { term: $("#terms").val(), page: page })
            .done(function (json) {
                console.log("JSON Data: ");
                console.log(json);
                var val = '';

                val = setResult("", json.result.Total, json.result.Results);
                setPagination(json.result.Page, json.result.PageSize, json.result.PageCount);

                $("#Results").val(val);

                createTableFromJson('.table1', json.result.Results);

            })
            .fail(function (jqxhr, textStatus, error) {
                setPagination(1, 10, 1);
                var err = textStatus + ", " + error;
                console.log("Request Failed: " + err);
                $("#Results").val("Request Failed: " + err);
            });

    }

    function setPagination(page, pageSize, pageCount) {
        $("#Page").val(page);
        $("#PageSize").val(pageSize);
        $("#PageCount").val(pageCount);

        if (page == 1 && pageCount == 1) {
            $(".pager li").addClass("disabled");
        }
        else if (page == 1 && pageCount > 1) {
            $(".pager li:first").addClass("disabled");
            $(".pager li:last").removeClass("disabled");
        }
        else if (page > 1 && page == pageCount) {
            $(".pager li:last").addClass("disabled");
            $(".pager li:first").removeClass("disabled");
        }
        else {
            $(".pager li").removeClass("disabled");
        }
    }

    function setResult(title, count, list) {

        if (count == 0) return "Not found";

        var val = "Count: " + count + "\n";
        var matched = JSON.stringify(list, undefined, 2);
        val += matched;
        val += "\n----------------------------------\n";

        return val;
    }

    function createTableFromJson(divSelector, jsonObject) {

        if (jsonObject.length == 0) {
            return;
        }
        //$(divSelector).parent('.row').addClass('show').removeClass('hide');
        var data = jsonObject;

        $(divSelector).createTable(data, {
            // General Style for Table
            borderWidth: '1px',
            borderStyle: 'solid',
            borderColor: '#DDDDDD',
            fontFamily: 'Verdana, Helvetica, Arial, FreeSans, sans-serif',

            // Table Header Style
            thBg: '#F3F3F3',
            thColor: '#0E0E0E',
            thHeight: '30px',
            thFontFamily: '"Open Sans Condensed", sans-serif',
            thFontSize: '14px',
            thTextTransform: 'capitalize',

            // Table Body/Row Style
            trBg: '#fff',
            trColor: '#0E0E0E',
            trHeight: '25px',
            trFontFamily: '"Open Sans", sans-serif',
            trFontSize: '13px',

            // Table Body's Column Style
            tdPaddingLeft: '10px',
            tdPaddingRight: '10px'
        });
    }
});
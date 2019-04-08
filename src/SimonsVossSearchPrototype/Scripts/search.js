$(function () {
    $("#btnSearch").click(function () {
        var self = this;
        $(self).attr("disabled", true);

        $("#Results").val("");
        $('.result .row').removeClass('show').addClass('hide');

        var url = $('#SearchApiUrl').val();// "../api/locks/search/";
        $.getJSON(url, { term: $("#terms").val() })
            .done(function (json) {
                console.log("JSON Data: ");
                console.log(json);
                var val = '';

                val = setResult("", json.count, json.list);
                $("#Results").val(val);

                createTableFromJson('.table1', json.list);

                $(self).removeAttr("disabled");
            })
            .fail(function (jqxhr, textStatus, error) {
                var err = textStatus + ", " + error;
                console.log("Request Failed: " + err);
                $("#Results").val("Request Failed: " + err);
                $(self).removeAttr("disabled");
            });
    });

    function setResult(title, count, list) {

        if (count == 0) return "";

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
        $(divSelector).parent('.row').addClass('show').removeClass('hide');
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
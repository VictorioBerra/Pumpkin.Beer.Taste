// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const enAlphabet = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
const aspNetCoreMomentFormat = "YYYY-MM-DDTHH:mm";

$(function () {
    // Convert any UTC labels to local datetime and display as `fromNow()`
    $(".utc-local-from-now").each(function (i, e) {
        let e$ = $(e);
        let utcData = e$.data().date;
        let utcMoment = moment.utc(utcData);
        let localMoment = utcMoment.local();
        e$.text(`Opened ${localMoment.fromNow()}`);
    });
});
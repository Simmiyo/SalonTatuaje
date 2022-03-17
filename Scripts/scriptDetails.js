var idTatuaj = 0;
var idDesen = 0;
const urlGalerie = "/Artist/GalerieTatuaje";
const urlProiecte = "/Artist/ProiectePortofoliu";
const idGal = '#gal';
const idProj = '#proj';

$(idGal).delegate("#butgal-next", "click", function () {
    var tatSize = $("#ist-tat-size").val();
    idTatuaj = (idTatuaj < tatSize - 1) ? idTatuaj + 1 : 0;
    var idArtist = $("#artist").val();
    GetData(idArtist, idTatuaj, urlGalerie, idGal);
});

$(idGal).delegate("#butgal-prev", "click", function () {
    var tatSize = $("#ist-proj-size").val();
    idTatuaj = (idTatuaj > 0) ? idTatuaj - 1 : tatSize - 1;
    var idArtist = $("#artist").val();
    GetData(idArtist, idTatuaj, urlGalerie, idGal);
});

$(idProj ).delegate("#but-proj-next", "click", function () {
    var desenSize = $("#ist-proj-size").val();
    idDesen = (idDesen < desenSize - 1) ? idDesen + 1 : 0;
    var idArtist = $("#artist").val();
    GetData(idArtist, idDesen, urlProiecte, idProj);
});

$(idProj).delegate("#but-proj-prev", "click", function () {
    var desenSize = $("#ist-proj-size").val();
    idDesen = (idDesen > 0) ? idDesen - 1 : desenSize - 1;
    var idArtist = $("#artist").val();
    GetData(idArtist, idDesen, urlProiecte, idProj);
});

function GetData(Id, idt, url_, div_id) {
    $.ajax({
        url: url_,
        type: "get",
        data: { idArtist: Id, index: idt},
        error: function (result) {
            alert("a fost un esec");
        },
        success: function (result) {
            console.log('a fost un succes');
            $(div_id).html(result);
        }
    });
}
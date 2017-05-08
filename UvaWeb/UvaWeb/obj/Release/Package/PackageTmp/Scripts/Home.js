function UserLevel(type){
    if (type = 1) 
    {
        console.log(type);
    }else if(type == 2){

    } else if (type == 3) {

    }
}

$(function () {
    $(window).on('resize', function () {
        if ($(this).width() <= 800) {
            $(".sidebar").addClass('compact');
        } else {
            $(".sidebar").removeClass('compact');
        }
    });
});

function no_submit() {
    return false;
}

function pushMessage(t) {
    var mes = 'Info|Implement independently';
    $.Notify({
        caption: mes.split("|")[0],
        content: mes.split("|")[1],
        type: t
    });
}

$(function () {
    $('.sidebar').on('click', 'li', function () {
        if (!$(this).hasClass('active')) {
            $('.sidebar li').removeClass('active');
            $(this).addClass('active');
        }
    })
})


 function UnionNotify() {
    setTimeout(function () {
        $.Notify({ keepOpen: true, type: 'success', caption: 'Felicidades!!!', content: "Su union al grupo ha sido procesada exisatosamente" });
    }, 1000);
 }

$(document).ready(function () {

    

    table = $('#DataTables_Table_0').DataTable();
    table2 = $('#DataTables_Table_2').DataTable();
    $('#selectCarrera').select2();
    $('#selectSemestre').select2();
    $('#selectSeccion').select2();
    $('#selectTurno').select2();
    SearchSuscriptions();
    SearchGroups();
    $('#DataTables_Table_0 tbody').on('click', 'tr', function () {
        
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            $('#DataTables_Table_0 tbody tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#selectTurno').change(function () {
        var semestre = $('#selectSemestre option:selected').val();
        var turno = $('#selectTurno option:selected').val();
        $.ajax({
            url: "GetSectionsInformation",
            dataType: "json",
            data: { semestre: semestre, turno:turno },
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            type: "get",
            async: true,
            processData: true,
            cache: false,
            success: function (data) {
                for (var i in  data) {
                    if (data[i][0] == "A" || data[i][0] == "B" || data[i][0] == "C") {
                        $('#selectSeccion').append($('<option>', { value: data[i][0] }).text(data[i][0]));
                        console.log(i);
                    }
                        
                }
                $('#selectSeccion').select2();
                
            },
            error: function (xhr) {
                alert("error");
            }

        });
    })

    $('#optionsAccordion').accordion();

    $('#DataTables_Table_1 tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            $('#DataTables_Table_1 tbody tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnUnirGrupo').click(function () {
        var seccion = $('#selectSeccion').val();
        var semestre = $('#selectSemestre').val();
        var turno = $('#selectTurno').val();
        var carrera = $('#selectCarrera').val();
        $.ajax({
            url: "UnionGroup",
            dataType: "json",
            data: {carrera:carrera, seccion:seccion, semestre: semestre, turno: turno},
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            type: "get",
            async: true,
            processData: true,
            cache: false,
            success: function (data) {
                showDialog('dialogUnionGroup');
                UnionNotify();
            },
            error: function (xhr) {
                alert("error");
            }

        });
    });


    $('#btnEliminarSuscripcion').click(function () {
        mtable = $('#DataTables_Table_0').DataTable();
        var a = mtable.rows('.selected').data();
        var i = a[0][0];
        $.ajax({
            url: "DeleteSuscription",
            dataType: "json",
            data: { dato: i},
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            type:"get",
            async: true,
            processData:true,
            cache: false,
            success: function (data) {
                alert(data);
            },
            error: function (xhr) {
                alert("error");
            }

        });
    });

    $('#btnCrearSuscripcion').click(function () {
        var code = $('#Icode').val();
        console.log(code);
        $.ajax({
            url: "CreateSuscripcion",
            dataType: "json",
            data: { dato: code },
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            type: "get",
            async: true,
            processData: true,
            cache: false,
            success: function (data) {
                showDialog('dialogCreateSuscription');
                alert(data);
            },
            error: function (xhr) {
                alert("error");
            }

        });
    });

    

    $("#carga").click(function () {
        SearchSuscriptions();   
    });

    $("#cargaGrupo").click(function () {
        SearchGroups();
    });
           
});


function SearchSuscriptions() {
    $('#DataTables_Table_0').dataTable({
        "bServerSide": true,
        "bDestroy": true,
        "sAjaxSource": "GetSuscriptions",
        "bProcessing": true,
        "aoColumns": [

                        { "sName": "CODIGO" },
                        { "sName": "VIDEO" },
                        { "sName": "PROFESOR" },
                        { "sName": "STATUS" }
        ]
    });


}


function SearchGroups() {
    $('#DataTables_Table_2').dataTable({
        "bServerSide": true,
        "bDestroy": true,
        "sAjaxSource": "/Home/GetGroups",
        "bProcessing": true,
        "aoColumns": [

                        { "sName": "CARRERA" },
                        { "sName": "SECCION" },
                        { "sName": "SEMESTRE" },
                        { "sName": "TURNO" }
        ]
    });


}

function showDialog(id) {
    var dialog = $("#" + id).data('dialog');
    if (!dialog.element.data('opened')) {
        dialog.open();
    } else {
        dialog.close();
    }
}
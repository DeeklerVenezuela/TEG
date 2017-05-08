$(document).ready(function(){
    var form = $(".login-form");

            form.css({
                opacity: 1,
                "-webkit-transform": "scale(1)",
                "transform": "scale(1)",
                "-webkit-transition": ".5s",
                "transition": ".5s"
            });

            $(function () {
                $("#carousel").carousel();
            });

            function notifyOnErrorInput(input) {
                var message = input.data('validateHint');
                $.Notify({
                    caption: 'Error',
                    content: message,
                    type: 'alert'
                });
            }
            
            function no_submit() {
                return false;
            }
   
            
            
            
            $(function() {
                var height = ($(document).height());
                var nuevo = height * 0.85;
                $('.resize').css({ 'height': nuevo + 'px' });
                $('.frame').css({ 'max-height': nuevo * 0.9 + 'px' });
                $('#principalContent').css({ 'max-height': nuevo  + 'px' });
            });
            
            $(function () {
                $("#tabMensajeLeft").click(function () {
                    $("#tabmensaje").click();
                });

                $("#tabVideoLeft").click(function () {
                    $("#tabvideo").click();
                });

                $("#tabSuscripcionLeft").click(function () {
                    $("#tabsuscripcion").click();
                });

                $("#tabEstadisticaLeft").click(function () {
                    $("#tabestadistica").click();
                });

                $("#tabProfesorLeft").click(function () {
                    $("#tabprofesor").click();
                });

                $("#tabOpcionLeft").click(function () {
                    $("#tabopcion").click();
                });

                ///////////////////////////////Captura click en el tab para cambiar estilo a la izquierda
                $("#tabmensaje").click(function () {
                    $("#tabMensajeLeft").addClass("active");
                    $("#tabVideoLeft").removeClass("active");
                    $("#tabSuscripcionLeft").removeClass("active");
                    $("#tabEstadisticaLeft").removeClass("active");
                    $("#tabProfesorLeft").removeClass("active");
                    $("#tabOpcionLeft").removeClass("active");
                });

                $("#tabvideo").click(function () {
                    $("#tabMensajeLeft").removeClass("active");
                    $("#tabVideoLeft").addClass("active");
                    $("#tabSuscripcionLeft").removeClass("active");
                    $("#tabEstadisticaLeft").removeClass("active");
                    $("#tabProfesorLeft").removeClass("active");
                    $("#tabOpcionLeft").removeClass("active");
                });

                $("#tabsuscripcion").click(function () {
                    $("#tabMensajeLeft").removeClass("active");
                    $("#tabVideoLeft").removeClass("active");
                    $("#tabSuscripcionLeft").addClass("active");
                    $("#tabEstadisticaLeft").removeClass("active");
                    $("#tabProfesorLeft").removeClass("active");
                    $("#tabOpcionLeft").removeClass("active");
                });

                $("#tabestadistica").click(function () {
                    $("#tabMensajeLeft").removeClass("active");
                    $("#tabVideoLeft").removeClass("active");
                    $("#tabSuscripcionLeft").removeClass("active");
                    $("#tabEstadisticaLeft").addClass("active");
                    $("#tabProfesorLeft").removeClass("active");
                    $("#tabOpcionLeft").removeClass("active");
                });

                $("#tabprofesor").click(function () {
                    $("#tabMensajeLeft").removeClass("active");
                    $("#tabVideoLeft").removeClass("active");
                    $("#tabSuscripcionLeft").removeClass("active");
                    $("#tabEstadisticaLeft").removeClass("active");
                    $("#tabProfesorLeft").addClass("active");
                    $("#tabOpcionLeft").removeClass("active");
                });

                $("#tabopcion").click(function () {
                    $("#tabMensajeLeft").removeClass("active");
                    $("#tabVideoLeft").removeClass("active");
                    $("#tabSuscripcionLeft").removeClass("active");
                    $("#tabEstadisticaLeft").removeClass("active");
                    $("#tabProfesorLeft").removeClass("active");
                    $("#tabOpcionLeft").addClass("active");
                });

               
                
            });
});


var mainapp = angular.module('ionicApp', ['ionic','ngResource','ngRoute','highcharts-ng'])

.config(function($stateProvider, $urlRouterProvider) {

  $stateProvider
  .state('inicio',{
    url:"/inicio",
            templateUrl:"views/templates/inicio.html",
            controller: "InitController"
  })
  .state('registro',{
    url:"/registro",
            templateUrl:"views/templates/registro.html",
            controller: "RegisterController"
  })
  .state('recuperar',{
    url:"/recuperar",
            templateUrl:"views/templates/recuperar.html",
            controller: "RecuperarCtlr"
  })
  .state('grupos',{
    url:"/grupos",
    controller: "GruposCtlr",
    templateUrl: "views/templates/grupos.html"
  })
   .state('config', {
      url: "/configuracion",
      controller:'ConfigCtlr',
      templateUrl: "views/templates/configuracion.html"
    })
  .state('tareas',{
    url:"/tareas",
    controller: "TareasCtlr",
    templateUrl:"views/templates/tareas.html"
  })
  .state('estadisticas',{
    url:"/estadisticas",
    controller:"EstadisticasCtlr",
    templateUrl : "views/templates/estadisticas.html"
  })
  
  .state('adminProf',{
    url:"admin-prof",
    controller:"AdminProfCtlr",
    templateUrl:"views/templates/adminProf.html"
  })
  .state('contenido',{
        parent:"adminProf",
        url:"/contenido",
        views :{
        '@':{
            controller: "ContenidoCtlr",
            templateUrl :"views/templates/contenido.html"
        }
      }
        
  })
    .state('tabs', {
      url: "/tab",
      abstract: true,
      templateUrl: "views/templates/tabs.html"
    })
    .state('tabs.home', {
      url: "/home",
      
      views: {
        'home-tab': {
          templateUrl: "views/templates/home.html",
          controller: 'MainCtlr'
        }
          
      }
    })
  .state('tabs.suscripcion',{
  url: "/suscripcion",
      views: {
        'suscripcion-tab': {
            controller:'SuscripcionCtlr',
          templateUrl: "views/templates/suscripcion.html"
        }
      }
  })
  
    .state('tabs.facts', {
      url: "/facts",
      views: {
        'home-tab': {
          templateUrl: "views/templates/facts.html"
        }
      }
    })
  
    .state('tabs.mensajes', {
      url: "/mensajes",
      views: {
        'mensajes-tab': {
          templateUrl: "views/templates/mensajes.html",
          controller:'MensajesCtlr'
        }
      }
    })
    .state('tabs.detailVideo', {
    url: "/detail/:id",
    views: {
      'home-tab': {
        controller:'DetailVideoCtrl',
    templateUrl: "views/templates/detailsVideo.html"
      }
    }
  })
  .state('tabs.detailUser', {
    url: "/detailUser/:id",
    views: {
      'profesor-tab': {
        controller:'DetailUserCtrl',
        templateUrl: "views/templates/detalleUser.html"
      }
    }
  })
  .state('tabs.detailMensaje', {
    url: "/detailMensaje/:id/:chat",
    views: {
      'mensajes-tab': {
        controller:'DetailMensajeCtrl',
        templateUrl: "views/templates/detalleMensaje.html"
      } 
    }
  })
    .state('tabs.profesor', {
      url: "/profesor",
      views: {
        'profesor-tab': {
          controller:'ProfesorCtrl',
          templateUrl: "views/templates/profesor.html"
        }
      }
    })
     
  
    ;


   $urlRouterProvider.otherwise("/inicio");

}).config(['$ionicConfigProvider', function($ionicConfigProvider) {

    $ionicConfigProvider.tabs.position('bottom'); 
    $ionicConfigProvider.navBar.alignTitle('center');
}])
.controller('MainCtlr', function($http, $scope, $ionicPopover, $ionicPopup, $resource, $ionicHistory,  $ionicModal, $state) {
    $scope.profesor = true;
    $scope.admin = true;
    if(localStorage.getItem('tipo') == 3){
        $scope.admin = false;
    }
    if(localStorage.getItem('tipo') == 2){
        $scope.profesor = false;
    }
    
$scope.blockUser = function(){
    $scope.profesor = true;
    $scope.admin = true;
    
    if(localStorage.getItem('tipo') == 3){
        $scope.admin = false;
    }
    if(localStorage.getItem('tipo') == 2){
        $scope.profesor = false;
    }
  }
    
    //$scope.hostName= "http://tegjeanrobles.ddns.net:20560";
    $scope.hostName= "http://192.168.0.170:20560";
    // $scope.hostName = "http://localhost:20539";
     $scope.BackButtonPopup = function() {
        $ionicHistory.goBack();
    };
    
    //Busqueda de Videos por suscripción 
    var email = "";
    $scope.videos = [];
    Videos = $resource($scope.hostName+'/Videos/BuscarVideosSuscritos',{email:'@email', id:'@id'});   
    $scope.videos = Videos.query({email:email, id:localStorage.getItem('user')});
    
    $scope.doRefreshVideos = function() {
            Videos.query({email:email, id:localStorage.getItem('user')},function(response){
                $scope.videos = response;
                $scope.$broadcast('scroll.refreshComplete');
            });
        
    };
    
    //cerrar sesion
    $scope.cerrarSesion = function(){
        $state.go('inicio');
        localStorage.removeItem("user");
        localStorage.removeItem("tipo");
    }
    
    
 
  $ionicPopover.fromTemplateUrl('views/templates/popover.html', {
    scope: $scope,
  }).then(function(popover) {
    $scope.popover = popover;
      
  })

  
  

  
$scope.showNotify = function(titulo, mensaje){
    var alertNotify = $ionicPopup.alert({
        title:titulo,
        template:mensaje
    });
    
    alertNotify.then(function(res){
        console.log(res);
    });
}

$scope.goMensajes = function(){
    $state.go('tabs.mensajes');
}

$scope.goHome = function(){
    $state.go('tabs.home');
}

$scope.goSuscripcion = function(){
    $state.go('tabs.suscripcion');   
}

$scope.goProfesor = function(){
    $state.go('tabs.profesor');
}

$scope.cargaVideo = false;
$scope.tipoUsuario = localStorage.getItem("tipo");

//Upload Video
$scope.uploadVideo = function(d){
        var files = $("#inputFile").get(0).files;
        var data = new FormData();
            for(i = 0; i< files.length; i++){
                data.append("file" + i , files[i]);
            }
            data.append("nombre", $("#nombre-subir").val());
            data.append("descripcion",$("#descripcion-subir").val());
            data.append("userId",localStorage.getItem("user"));
            data.append("status",$('#enableVideo').prop("checked"));

            $.ajax({
                
                type: "POST",
                url : $scope.hostName + "/api/UploadVideo/VideoUpload",
                contentType: false,
                processData: false,
                data : data,
                success : function(result){
                    
                    if(result){
                        d = "Nada seleccionado!!!"
                        $("#nombre-subir").val(" ");       
                        $("#descripcion-subir").val(" ");
                        $("#loader-upload").css("display","none");
                        $("#label-upload").css("display","none");
                        $scope.showNotify("Felicidades!!!","Su archivo se cargo correctamente");
                    }
                        
                },
                beforeSend: function(vd){
                    $("#loader-upload").css("display","block");
                    $("#label-upload").css("display","block");
                },
                error : function(e){
                    console.log(e);
                }
            })
}

$scope.data = {
    archivoNombre : ""
}
$scope.fileChanged = function(d){
    $scope.data.archivoNombre = d.files[0].name;
    $(".value-file-input").val($scope.data.archivoNombre);
}

   $ionicModal.fromTemplateUrl('views/templates/subirVideos.html', {
      scope: $scope,
      animation: 'slide-in-up'
    }).then(function(modal) {
      $scope.modal = modal;
    });
  $scope.openModal = function(){
    $scope.modal.show();
  }
  $scope.closeModal = function(){
    $(".value-file-input").val('');
    $scope.modal.hide();
    $("#nombre-subir").val(" ");       
    $("#descripcion-subir").val(" ");
  }
   $scope.onItemDelete = function(item) {
       var temp = "Seguro desea eliminar esta suscripcion? <br>Video: "+ item.Descripcion + "<br>Docente: " +item.NombreProfesor + " " + item.ApellidoProfesor + ".";
        var confirmPopup = $ionicPopup.confirm({
            title: 'Confirmacion',
            template: temp
        });
        confirmPopup.then(function(res) {
                if(res) {                        
                   Delete = $resource($scope.hostName + '/Suscripcion/DeleteSuscripcion?videoId=:videoId&userId=:userId',{videoId:'@videoId',userId:'@userId'});
                    $scope.deleteSus = Delete.query({videoId:item.VideoID,userId:localStorage.getItem('user')},function(e){
                        $scope.videos.splice($scope.videos.indexOf(item), 1);
                        $scope.showNotify(e[0],e[1]);
                    })                        
                }else {
                    console.log('Canceló');
                }
        });
  }
   $scope.data = {
    showDelete: false
  }
    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.numberOfPages=function(){
        return Math.ceil($scope.videos.length/$scope.pageSize);                
    }  
    
    
    $scope.goBackFunction = function(){
        $ionicHistory.goBack();
    }
    
   
  
    
    
    
})
.controller('MensajesCtlr',function($scope, $routeParams, $resource, $stateParams, $ionicPopup){
    //Busqueda de Mensajes por usuario 
     Mensajes = $resource($scope.hostName+'/Mensajes/BuscarMensajes?id=:id',{id:'@id'});
    $scope.buscarMensajes = function(){
        var mensajes = [];  
        $scope.mensajes = Mensajes.query({id:localStorage.getItem('user')});
    }
    
    $scope.doRefreshMensajes = function(){
        $scope.mensajes = Mensajes.query({id:localStorage.getItem('user')},function(response){
                $scope.mensajes = response;
                $scope.$broadcast('scroll.refreshComplete');
            });
    }
    $scope.DeleteMensaje = function(item) {
    console.log(item[0].chat);
    
       var temp = "¿Seguro desea eliminar esta conversación?.";
        var confirmPopup = $ionicPopup.confirm({
            title: 'Confirmacion',
            subTitle: 'Esta acción es permanente',
            template: temp
        });
        confirmPopup.then(function(res) {
                if(res) {                        
                   Delete = $resource($scope.hostName + '/Mensajes/DeleteConversacion?id=:id',{id:'@id'});
                    $scope.deleteSus = Delete.query({id:item[0].chat},function(e){
                        $scope.mensajes.splice($scope.mensajes.indexOf(item), 1);
                        $scope.showNotify(e[0],e[1]);
                    })                        
                }
        });
  }
    
})
.controller('DetailVideoCtrl',function($scope, $routeParams, $resource, $stateParams, $sce){
    $scope.lpos = 0;
    $scope.lneg = 0;
    $scope.com = 0;
    $scope.repro = 0;
    $scope.comentario = "";
    //Registrar Reproducción
    Reproduccion = $resource($scope.hostName + '/Videos/RegistrarReproduccion?videoId=:videoId&userId=:userId',{videoID:'@videoId',userId:'@userId'});
    $scope.reproduccion = Reproduccion.query({videoId:$stateParams.id, userId:localStorage.getItem('user')});
    
    //Busqueda de Videos por ID
    $scope.videos = [];
    Item = $resource($scope.hostName + '/Videos/BuscarVideosPorID?id=:id',{id:'@id'});   
    $scope.item = Item.get({id:$stateParams.id});
    $scope.GenerateUrl = function(id){
        $scope.mob = "";

            if(window.innerWidth <= 800 && window.innerHeight <= 600){
                $scope.mob = "_sm";
            }
            else {
                $scope.mob = "_xl";
            }
        
        return $sce.trustAsResourceUrl($scope.hostName + id + $scope.mob + ".mp4");
    } 
    
    Likes = $resource($scope.hostName + '/Likes/GetLikes?videoId=:videoId',{videoId:'@videoId'});
    $scope.getLikes = function(){
        Likes.query({videoId:$stateParams.id},function(e){
            $scope.lpos = e[0];
            $scope.lneg = e[1];
            $scope.com = e[2];
            $scope.repro = e[3];
        })
    }
    $scope.getLikes();
    
    $scope.like = function(tipo){
        Positivos = $resource($scope.hostName + '/Likes/AddLike?videoId=:videoId&userId=:userId&tipo=:tipo',{videoId:'@videoId',userId:'userId',tipo:'@tipo'});
        $scope.addLike = Positivos.query({videoId:$stateParams.id,userId:localStorage.getItem('user'),tipo:tipo},function(d){
                $scope.getLikes(); 
        });
    } 


   
        ComentariosLista = $resource($scope.hostName + '/Comentarios/GetComentariosWithId?videoId=:videoId',{videoId:'@videoId'});
        $scope.listadoComentarios = ComentariosLista.query({videoId:$stateParams.id});
   
    
    

    $scope.enviarComentario = function(c){
        Comentario = $resource($scope.hostName + '/Comentarios/AddComentario?userId=:userId&videoId=:videoId&descripcion=:descripcion',{userId:'@userId',videoId:'@videoId',comentario:'@comentario'});
        $scope.comentariosend = Comentario.query({userId:localStorage.getItem('user'),videoId:$stateParams.id,descripcion:c},function(r){
            var d = new Date();
        var day = d.getDate();
        var month = d.getMonth()+1;
        var output = ((''+day).length<2 ? '0' : '') + day + "/" +((''+month).length<2 ? '0' : '') + month + "/" + d.getFullYear();    
        
        $scope.listadoComentarios.unshift({
            Fecha: output,
            User:r[1],
            Thumb: r[0],
            Descripcion : c,
            Status: true
        })
        
        
        });
        $('#foro-input').val('');
    }
    
    $scope.enviarPrivado = function(d){
        Privado = $resource($scope.hostName + '/Mensajes/InsertarMensaje?id=:id&body=:body&videoID=:videoID&chat=:chat',{id:'@id', body:'@body', videoID:'@bvideoID',chat:'@chat'});//Preparar petición
        $scope.msj = Privado.query({id:localStorage.getItem('user'),body:d,videoID:$stateParams.id,chat:"" },function(f){
            $scope.showNotify(f[0],f[1]);
            $("#privadoInput").val('');
        });
    }

})
.controller('InitController',['$scope','$http','$ionicPopup','$timeout', '$state', function($scope, $http, $ionicPopup, $timeout, $state){   
        $scope.data = {}
    if(localStorage.getItem('user') != null){
        $state.go('tabs.home');
    }
        $scope.showAlert = function(titulo, mensaje) {
   var alertPopup = $ionicPopup.alert({
     title: titulo,
     template: mensaje
   });    
 };
    $scope.LoginObj = {};
    
    
    $http.defaults.headers.put = {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
        'Access-Control-Allow-Headers': 'Content-Type, X-Requested-With'
        };
        $http.defaults.useXDomain = true;
    
    
    //Login
    $scope.Login = function(){
        delete $http.defaults.headers.common['X-Requested-With'];
        $http({
            url:$scope.hostName + '/Login/LoginMain',
            method: "GET",
            params: {email:$scope.LoginObj.email,pass:$scope.LoginObj.pass}
        }).success(function(data){
            if(data.result == true){
                
                localStorage.setItem("user",data.user);
                localStorage.setItem("tipo",data.tipo);
                $scope.LoginObj.email = "";
                $scope.LoginObj.pass = "";
                $scope.blockUser();
                $state.go('tabs.home');
                
            }else{
                $scope.showAlert('Disculpe', data.mensaje);
            }
        });
    }
    
     
}])
.controller('RegisterController',['$scope','$http','$ionicPopup','$ionicScrollDelegate','$timeout', function($scope, $http, $ionicPopup,$ionicScrollDelegate, $timeout){
    
        $scope.RegistrarObj = {
                nombre:"",
                apellido:"",
                email:"",
                cedula:"",
                password:"",
                password2:"",
                carrera:"",
                preg1:"",
                resp1:"",
                preg2:"",
                resp2:""
        };
    $scope.showAlert2 = function(titulo, mensaje) {
        $
   var alertPopup2 = $ionicPopup.alert({
     title: titulo,
     template: mensaje
   });    
 };
   $scope.scrollTop = function(){
    window.scrollTo(0,0);
   };
            $scope.user = {
            tipo:"0"
        };
        $http.defaults.headers.put = {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
        'Access-Control-Allow-Headers': 'Content-Type, X-Requested-With'
        };
        $http.defaults.useXDomain = true;
        
        //Registrar
        $scope.RegistrarF = function(){
        $scope.scrollTop();  
            var selectp1 = $('#preg1').val();
            var selectp2 = $('#preg2').val();
            var pregunta1 = $('#preg1 option[value="'+selectp1+'"]').text();
            var pregunta2 = $('#preg2 option[value="'+selectp2+'"]').text();
        delete $http.defaults.headers.common['X-Requested-With'];    
        $http({
            url:$scope.hostName + '/Register/Register',
            method:'GET',
            params: {
                nombre : $scope.RegistrarObj.nombre,
                apellido:$scope.RegistrarObj.apellido,
                email:$scope.RegistrarObj.email,
                cedula:$scope.RegistrarObj.cedula,
                password:$scope.RegistrarObj.password,
                password2:$scope.RegistrarObj.password2,
                carrera:$scope.RegistrarObj.carrera,
                preg1:pregunta1,
                resp1:$scope.RegistrarObj.resp1,
                preg2:pregunta2,
                resp2:$scope.RegistrarObj.resp2,
                foto:"",
                type:$scope.user.tipo
            }
        }).success(function(data){
            console.log(data);
            $scope.scrollTop();
            $scope.showAlert2('Mensaje', data);
        }).error(function(error){
            $scope.showAlert2('Disculpe', error);
        });
        
    }
    
       
}])
.controller('SuscripcionCtlr',function($scope,$stateParams,$resource,$ionicPopup,$ionicScrollDelegate){
    
    $scope.agregarSuscripcion = function(codeSuscripcion){
        Suscripcion = $resource($scope.hostName + '/Suscripcion/CreateSuscriptionWithCode?code=:code&userId=:userId',{code:'@code',         userId:'@userId'});
        $scope.suscripcion = Suscripcion.query({code:codeSuscripcion, userId:localStorage.getItem('user')},function(){
            $scope.showNotify($scope.suscripcion[0] , $scope.suscripcion[1]);
        });
    }
    $scope.buscarDataSuscripciones = function(){
    //Buscar Suscripciones Pendientes
    Pendiente = $resource($scope.hostName + '/Suscripcion/BuscarSuscripcionesPendientes?id=:id',{id:'@id'});
    $scope.pendientes  = Pendiente.query({id:localStorage.getItem('user')});

    //Buscar para la lista inteligente
    SuscripcionList = $resource($scope.hostName + '/Suscripcion/BuscarSuscripcionesDisponibles?id=:id',{id:'@id'});
    $scope.suscripcionList = SuscripcionList.query({id:localStorage.getItem('user')});
    
    }
    
    $scope.doRefreshSuscripcion = function(){
                //Buscar Suscripciones Pendientes
                Pendiente = $resource($scope.hostName + '/Suscripcion/BuscarSuscripcionesPendientes?id=:id',{id:'@id'});
                $scope.pendientes  = Pendiente.query({id:localStorage.getItem('user')});

                //Buscar para la lista inteligente
                SuscripcionList = $resource($scope.hostName + '/Suscripcion/BuscarSuscripcionesDisponibles?id=:id',{id:'@id'});
                $scope.suscripcionList = SuscripcionList.query({id:localStorage.getItem('user')});
                $scope.$broadcast('scroll.refreshComplete');
    }
    
    $scope.PopupSuscripcion = function(codeSuscripcion,Vnombre,Vdescripcion,Unombre,Uapellido) {      
        
        $scope.temp = "Agregar suscripcion a video <br><br>Nombre: " + Vnombre + "<br>Descripción: " + Vdescripcion + "<br>Profesor: "+Unombre + " " + Uapellido;
        $scope.showAgregateSus = function(temp, codeSuscripcion) {
                var confirmPopup = $ionicPopup.confirm({
                    title: 'Confirmacion',
                    template: temp
                });
            confirmPopup.then(function(res) {
                    if(res) {                        
                            Suscripcion = $resource($scope.hostName + '/Suscripcion/CreateSuscriptionWithIA?code=:code&userId=:userId',{code:'@code',         userId:'@userId'});
                            $scope.suscripcion = Suscripcion.query({code:codeSuscripcion, userId:localStorage.getItem('user')},function(){
                                $scope.showNotify($scope.suscripcion[0] , $scope.suscripcion[1]);
                            });
                                     
                    }else {
                        console.log('Canceló');
                    }
            });
        };
        $scope.showAgregateSus($scope.temp, codeSuscripcion);      
    };  
})
.controller('DetailMensajeCtrl',function($scope, $ionicScrollDelegate, $stateParams, $resource, $sce){
    
    //Busqueda de Chat 
    Chat = $resource($scope.hostName+ '/Mensajes/BuscarMensajesPorVideo?user=:user&chat=:chat',{user:'@user',chat:'@chat'});   
    $scope.chat = Chat.query({user:localStorage.getItem('user'), chat:$stateParams.chat},function(e){
    });

    //Colocar foto docente de la conversación 
    Foto = $resource($scope.hostName + '/Mensajes/BuscarFotoProfesorChat?id=:id',{id:'@id'});
    $scope.fotoProfesor = Foto.get({id:$stateParams.id});
    //$scope.host = "http://tegjeanrobles.ddns.net:20560";

    //Enviar Mensajes tomando en cuenta tipo de usuario
    $scope.sender = "";
    $scope.enviarMensaje = function(){
    Send = $resource($scope.hostName + '/Mensajes/InsertarMensaje?id=:id&body=:body&videoID=:videoID&chat:=chat',
                     {id:'@id', body:'@body', videoID:'@videoID', chat:'@chat'});//Preparar petición
    $scope.msj = Send.query({id:localStorage.getItem('user'),body:$scope.sender,videoID:$stateParams.id,chat:$stateParams.chat});//Hacer petición GET
    var d = new Date();
    var day = d.getDate();
    var month = d.getMonth()+1;
    var output = ((''+day).length<2 ? '0' : '') + day + "/" +((''+month).length<2 ? '0' : '') + month + "/" + d.getFullYear();    
        $scope.chat.push({
        Descripcion : $scope.sender,
        Fecha: output,
        Status: true
    })
        
     $scope.chat.$promise.then(function(data) {
       $ionicScrollDelegate.scrollBottom();
    });
        
    }
    
    //Scroll Bottom en lista de videos
    $scope.chat.$promise.then(function(data) {
       $ionicScrollDelegate.scrollBottom();
   });

  })
.controller('ProfesorCtrl',function($scope, $resource){
    $scope.buscarDataProfesor = function(){
        Profesores = $resource($scope.hostName + '/Grupos/BuscarProfesoresEnGrupos?id=:id',{id:'@id'});
        $scope.profesores = Profesores.query({id:localStorage.getItem('user')});//Hacer petición GET
    } 
    
    $scope.doRefreshProfesor = function(){
                Profesores = $resource($scope.hostName + '/Grupos/BuscarProfesoresEnGrupos?id=:id',{id:'@id'});
                $scope.profesores = Profesores.query({id:localStorage.getItem('user')});//Hacer petición GET
                $scope.$broadcast('scroll.refreshComplete');
           
    }
})
.controller('ConfigCtlr',function($scope, $resource){
   
    
    $scope.GetFotoActual = function(){
        FotoActual = $resource($scope.hostName + '/User/GetFoto?id=:id',{id:'@id'});
        $scope.foto = FotoActual.query({id:localStorage.getItem('user')},function(e){
            $scope.fotoUrl = $scope.hostName + e[0];
        });//Hacer petición GET
    }
    
    $scope.GetEmailActual = function(){
        EmailActual = $resource($scope.hostName + '/User/GetEmail?id=:id',{id:'@id'});
        $scope.foto = EmailActual.query({id:localStorage.getItem('user')},function(e){
            $scope.email = e[0];
            $scope.nombre = e[1];
            $scope.apellido = e[2];
        });//Hacer petición GET
    }
    
    
    $scope.GetFotoActual();
    $scope.GetEmailActual();
    
    $scope.UpdateEmail = function(e){
        UpdateE = $resource($scope.hostName + '/User/UpdateEmail?id=:id&email=:email',{id:'@id', email:'@email'});
        $scope.foto = UpdateE.query({id:localStorage.getItem('user'),email:e},function(e){
            $scope.showNotify(e[0], e[1]); 
        });
    }
   $scope.fileChanged2 = function(element){
        $scope.currentFile = element.files[0];
        var reader = new FileReader();
        reader.onload = function(event) {
            $scope.fotoUrl = event.target.result
            $scope.$apply()
        }
        reader.readAsDataURL(element.files[0]);
   }
   
   $scope.uploadFoto = function(){
        var files = $("#custom-file-input2").get(0).files;
        var data = new FormData();
            for(i = 0; i< files.length; i++){
                data.append("file" + i , files[i]);
            }
            data.append("id",localStorage.getItem('user'));


            $.ajax({
                
                type: "POST",
                url : $scope.hostName + "/api/UploadVideo/FotoUpload",
                contentType: false,
                processData: false,
                data : data,
                success : function(result){
                    $scope.showNotify("Felicidades!!!", "Sus datos se actualizaron correctamente.");       
                },
                
                error : function(e){
                            console.log(e);
                        }
            })
   }
   
})
.controller('EstadisticasCtlr',function($scope, $resource, $ionicSlideBoxDelegate){
    $scope.dataGrafico1 = [];
    
    GralProfesor = $resource($scope.hostName + '/Estadisticas/EstadisticaGralProfesor?id=:id',{id:"@id"});
    $scope.datosGlobales = GralProfesor.query({id:localStorage.getItem('user')});
    
    /*GralProfesor30 = $resource($scope.hostName + '/Estadisticas/EstadisticaGralProfesorUltMes?id=:id',{id:'@id'});
    $scope.ultimos30dias = GralProfesor30.query({id:localStorage.getItem('user')});
    console.log($scope.ultimos30dias);*/
    
    DetalleVideo = $resource($scope.hostName + '/Estadisticas/EstadisticasVideosPorUsuario?id=:id',{id:'@id'});
    $scope.detalleVideos = DetalleVideo.query({id:localStorage.getItem('user')});
    
    $scope.buscarDatosEstadisticos = function(){
    
    GraficoTresMeses = $resource($scope.hostName + '/Estadisticas/GraficoGralProfesorUltMes?id=:id',{id:'@id'});
    $scope.graficoTresMeses = GraficoTresMeses.get({id:localStorage.getItem('user')},function(e){
        var general = [];
        angular.forEach(e.SeriesMes, function(value, key) {
                var obj = {};
                obj['name'] = value.name;
                obj['y'] = value.y;
                obj['drilldown'] = value.drilldown;
                general.push(obj);
        });
        var suscripciones = [];
        angular.forEach(e.Suscripciones.data, function(value, key){
            var obj = [value.Mes,value.Valor];
            suscripciones.push(obj);    
        });
        var reproducciones = [];
        angular.forEach(e.Reproducciones.data, function(value, key){
            var obj = [value.Mes,value.Valor];
            reproducciones.push(obj);    
        });
        var likes = [];
        angular.forEach(e.Likes.data, function(value, key){
            var obj = [value.Mes,value.Valor];
            likes.push(obj);    
        });
        var dislikes = [];
        angular.forEach(e.Dislikes.data, function(value, key){
            var obj = [value.Mes,value.Valor];
            dislikes.push(obj);    
        });
        var mensajes = [];
        angular.forEach(e.Mensajes.data, function(value, key){
            var obj = [value.Mes,value.Valor];
            mensajes.push(obj);    
        });
        var sesiones = [];
        angular.forEach(e.Sesiones.data, function(value, key){
            var obj = [value.Mes,value.Valor];
            sesiones.push(obj);    
        });
        

        
        
        $scope.highchartsNG1 = {
       options: {
            chart: {
                type: 'column'
            },
            lang: {
                drillUpText: 'Volver a la {series.name}'
            },
           plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:.1f}'
                }
            }
            
        },
           legend: {
            enabled: false
        },
           
        },
        
        credits: {
            enabled: false
        },
        title: {
            text: 'Gráfico de detalles'
        },
        subtitle: {
            text: 'Presione las columnas para obtener más información.'
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Porcentajes (%) del último més'
            }
        },

        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> del total.<br/>'
        },
            
        series: [{
       
        name: "Data",
        colorByPoint: true,
        data: general
        }],
        drilldown: {
            
            series: [{
                name: e.Suscripciones.id,
                id: e.Suscripciones.id,
                data: suscripciones,
                
            }, {
                name: e.Reproducciones.id,
                id: e.Reproducciones.id,
                data: reproducciones
            }, {
                name: e.Likes.id, 
                id: e.Likes.id,
                data: likes
            }, {
                name: e.Dislikes.id, 
                id: e.Dislikes.id,
                data: dislikes
            }, {
                name: e.Mensajes.id, 
                id: e.Mensajes.id,
                data: mensajes
            }, {
                name: e.Sesiones.id, 
                id: e.Sesiones.id,
                data: sesiones
            }]
        }
    }
        
    });
        
    }
     
})
.controller('ContenidoCtlr',function($scope, $resource, $ionicPopup){
    
    $scope.data = {
        Nombre : "",
        Descripcion : ""
    };
    
    Estadistica = $resource($scope.hostName + '/Estadisticas/CountEstadisticasProfesor?id=:id',{id:'@id'});
    $scope.estadistica = Estadistica.query({id:localStorage.getItem('user')});//Buscar estadísticas
    
    //Buscar Suscripciones Pendientes
    Listado = $resource($scope.hostName + '/Videos/GetAllVideos?id=:id',{id:'@id'});
    $scope.videos  = Listado.query({id:localStorage.getItem('user')});
    
    $scope.doRefreshContenido = function(){
        Listado.query({id:localStorage.getItem('user')},function(e){
            $scope.videos  = e;
            $scope.$broadcast('scroll.refreshComplete'); 
        });
    }
    
    $scope.modificarItem = function(item){
        $scope.data.Descripcion = item.Descripcion;
        $scope.data.Nombre = item.Nombre;
        $scope.data.Id = item.VideoID;
        $scope.data.Status = item.Status;
        

        var confirmPopup = $ionicPopup.show({
            templateUrl: 'views/templates/editVideo.html',
            scope: $scope,
            title:"Editar vídeotutorial",
            subTitle: "Si deshabilita la visualización los demas no podrán ver sus vídeos.",
            buttons:[
                { text: 'Cancelar',
                    onTap: function(e) {
                    return 'cancel button'
                    }
                },
                {
                    text: 'Ok',
                    type: 'button-positive',
                    onTap: function(e) {
                        EnviarEditar = $resource($scope.hostName + '/Videos/UpdateVideo?id=:id&nombre=:nombre&descripcion=:descripcion&status=:status',{id:'@id', nombre:'@nombre', descripcion:'@descripcion',status:'@status'});
                        $scope.edit = EnviarEditar.query({id: $scope.data.Id, nombre: $scope.data.Nombre, descripcion:$scope.data.Descripcion, status:$scope.data.Status},function(e){
                            $scope.showNotify(e[0],e[1]);
                            Listado.query({id:localStorage.getItem('user')},function(e){
                                $scope.videos  = e;
                            })
                                         
                        });
                    return 'ok button'
                    }
                },
                ]
            
        });
        confirmPopup.then(function(res) {
            //console.log(res);
        });
    }
    
    
})
.controller('AdminProfCtlr',function($scope, $resource, $ionicPopup){
    //Buscar Suscripciones Pendientes
    Pendientes = $resource($scope.hostName+ '/Profesor/BuscarSuscripcionesSinAprobar?id=:id',{id:'@id'});
    $scope.BuscarTodosPendientes = function(){
        $scope.pendientes  = Pendientes.query({id:localStorage.getItem('user')});
        
    }
    $scope.BuscarTodosPendientes();
    
    $scope.aprobarSuscripcion = function(item){
        var temp = "<div class='aprobacion-container'>Seguro desea aprobar esta suscripción? <br><br><div class='ua-container'><b>Usuario:</b> " +item.NombreProfesor +" "+item.ApellidoProfesor + "<br><b>Cédula:</b> " + item.Cedula + "</b></div></div>";
        var confirmPopup = $ionicPopup.confirm({
            title: 'Confirmacion',
            template: temp
        });
        confirmPopup.then(function(res) {
                if(res) {                        
                    AprobarS = $resource($scope.hostName + '/Profesor/AprobarSuscripcionesPendintes?suscripcionId=:suscripcionId&userId=:userId',{suscripcionId:'@suscripcionId',userId:'@userId'});
                    $scope.deleteSus = AprobarS.query({suscripcionId:item.Suscripcion,userId:localStorage.getItem('user') },function(e){
                        $scope.showNotify(e[0],e[1]);
                        $scope.BuscarTodosPendientes();
                    })                        
                }else {
                    console.log('Canceló');
                }
        });
    }
    
    $scope.doRefreshPendientes = function(){
        Pendientes = $resource($scope.hostName+ '/Profesor/BuscarSuscripcionesSinAprobar?id=:id',{id:'@id'});
        $scope.pendientes  = Pendientes.query({id:localStorage.getItem('user')});
            /*
            Pendientes.query({id:localStorage.getItem('user')},function(e){
            $scope.pendientes  = e;*/
        console.log($scope.pendientes);
            $scope.$broadcast('scroll.refreshComplete');
        
    }
})
.controller('TareasCtlr',function($scope, $resource,$ionicPopup){
    $scope.tipoUser = localStorage.getItem('user');
    
    Aprobar = $resource($scope.hostName + '/User/GetUsersPendientes');
    $scope.sinAprobar = Aprobar.query();
    
    $scope.aprobarRegistro = function(item) {
       var temp = "Seguro desea aprobar este registro? <br>Video: <br>Docente: " +item.Nombre + " <br>Cédula" + item.Cedula + "<br>Tipo:" + item.Tipo + ".";
        var confirmPopup = $ionicPopup.confirm({
            title: 'Confirmacion',
            template: temp
        });
        confirmPopup.then(function(res) {
                if(res) {                        
                    Aprobar = $resource($scope.hostName + '/User/AprobarRegistro?id=:id',{id:'@id'});
                    $scope.deleteSus = Aprobar.query({id:item.Id},function(e){
                        $scope.showNotify(e[0],e[1]);
                    })                        
                }else {
                    console.log('Canceló');
                }
        });
  }
})
.controller('DetailUserCtrl',function($scope, $resource, $stateParams){
    
    User = $resource($scope.hostName + '/User/GetUser?id=:id',{id:'@id'});   
    $scope.usuario = User.query({id:$stateParams.id});
    
    Grupos = $resource($scope.hostName + '/Grupos/GetGroupsById?id=:id',{id:'@id'});
    $scope.grupos = Grupos.query({id:$stateParams.id});
     
})
.controller('RecuperarCtlr',function($scope, $resource, $ionicPopup){
    $scope.cambiar = false;
    $scope.BuscarPreguntas = function(d){
        Buscador = $resource($scope.hostName + '/User/GetPreguntasDeSeguridad?email=:email',{email:'@email'});   
        $scope.preguntas = Buscador.query({email:d},function(e){
            if(e[0] == "true"){
                $scope.pregunta1 = e[1];
                $scope.pregunta2 = e[2];
                $scope.cambiar = true;
            }else{
                $scope.showNotify(e[1],e[2]);
            }
        });
    }
    $scope.CancelarPreguntas = function(){
        $scope.cambiar = false;
    }
    
    $scope.data.pcont= "";
    $scope.data.scont = "";
    
    $scope.NuevaPass = function(d){
    var confirmPopup = $ionicPopup.show({
            templateUrl: 'views/templates/cambiarcontrasena.html',
            scope: $scope,
            title:"Felicidades!!!",
            subTitle:"Ahora puede cambiar su contraseña, recuerde que la nueva clave no debe contener caracteres especiales, solo letras a-z A-Z y ",
            buttons:[
                { text: 'Cancelar',
                    onTap: function(e) {
                      $scope.data.pcont= "";
                      $scope.data.scont = "";  
                    return 'no'
                    }
                },
                {
                    text: 'Ok',
                    type: 'button-positive',
                    onTap: function(e) {
                            Cambiar = $resource($scope.hostName + '/User/CambiarClave?email=:email&pcont=:pcont&scont=:scont',{email:'@email',pcont:'@pcont',scont:'@scont'});   
                            $scope.cambiar = Cambiar.query({email: d, pcont:$scope.data.pcont, scont:$scope.data.scont},function(f){
                            $scope.showNotify(f[1],f[2]);
                        });                                        
                    return 'ok'
                    }
                },
                ]
        });
        confirmPopup.then(function(res) {
            //console.log(res);
        });
    }
    $scope.VerificarRespuestas = function(e,d,f){
        Verificar = $resource($scope.hostName + '/User/VerificarRespuestas?email=:email&presp=:presp&sresp=:sresp',{email:'@email',presp:'@presp',sresp:'@sresp'});
        $scope.CambiarContrasena = Verificar.query({email:f, presp:e, sresp: d},function(g){
            if(g[0] == "false"){
                $scope.showNotify(g[1],g[2]);
            }else{
                $scope.NuevaPass(f);
            }
        }) 
    }
})
.controller('GruposCtlr',function($scope, $resource, $ionicPopup, $ionicHistory){
    $scope.addButtonG = false;
 
    GruposUser = $resource($scope.hostName + '/Grupos/GetGroupsById?id=:id',{id:'@id'});
    $scope.gruposUser = GruposUser.query({id:localStorage.getItem("user")});
    
    Carreras = $resource($scope.hostName + '/Grupos/BuscarCarreras');
    $scope.carreras = Carreras.query();
    
    Semestre = $resource($scope.hostName + '/Grupos/BuscarSemestre?carrera=:carrera',{carrera:'@carrera'});
    $('#sCarreras').change(function(){
        var car = $('#sCarreras').val();
        $scope.semestre = Semestre.query({carrera:car});
    });
    
    Turno = $resource($scope.hostName + '/Grupos/BuscarTurno?carrera=:carrera&semestre=:semestre',{carrera:'@carrera',semestre:'@semestre'});
    $('#sSemestre').change(function(){
        var car = $('#sCarreras').val();
        var sem = $('#sSemestre').val();
        $scope.turno = Turno.query({carrera:car, semestre:sem});       
    });
    
    Seccion = $resource($scope.hostName + '/Grupos/BuscarSeccion?carrera=:carrera&semestre=:semestre&turno=:turno',{carrera:'@carrera', semestre:'@semestre', turno:'@turno'});
    $('#sTurno').change(function(){
        var car = $('#sCarreras').val();
        var sem = $('#sSemestre').val(); 
        var tur = $('#sTurno').val();
        $scope.seccion = Seccion.query({carrera:car, semestre:sem, turno:tur}); 
        
    })
    
    $('#sSeccion').change(function(){
              
        if($('#sSeccion').val() != ""){
            $('#addGBtn').prop( "disabled", false );
        }
        else{
            $('#addGBtn').prop( "disabled", true );
        }
    })
    
    $scope.agregarGrupo = function(){
        Grupo = $resource($scope.hostName + '/Grupos/RegistrarGrupo?carrera=:carrera&semestre=:semestre&turno=:turno&seccion=:seccion&id=:id',{carrera:'@carrera', semestre:'@semestre', turno:'@turno', seccion:'@seccion',id:'@id'});
        var car = $('#sCarreras').val();
        var sem = $('#sSemestre').val(); 
        var tur = $('#sTurno').val();
        var sec = $('#sSeccion').val();
        var id = localStorage.getItem('user');
        $scope.addGrupo = Grupo.query({carrera:car, semestre:sem, turno:tur,seccion:sec, id:id},function(d){
            $scope.showNotify(d[0],d[1]);
        });
        
    }
    
    $scope.eliminarGrupo = function(id){
        Gr = $resource($scope.hostName + '/Grupos/GetGroupForDelete?id=:id',{id:'@id'});
        $scope.grupoEliminar = Gr.query({id:id},function(c){
            var content = "Esta seguro de que desea eliminar su union al grupo: <br><br>Carrera : " + c[0].Carrera + "<br>Semestre : " + c[0].Semestre + " <br>Sección : " + c[0].Seccion + "<br>Turno : " + c[0].Turno ;
            $scope.showConfirm(content, id);
        });
        
        
        
    }
    
    $scope.showConfirm = function(content,id) {
   var confirmPopup = $ionicPopup.confirm({
     title: 'Confirmacion',
     template: content
   });
   confirmPopup.then(function(res) {
     if(res) {
       Eliminar = $resource($scope.hostName + '/Grupos/DeleteSuscription?id=:id&user=:user',{id:'@id', user:'@user'});
        $scope.Eliminar = Eliminar.query({id:id, user:localStorage.getItem('user')},function(c){
            $scope.showNotify(c[0],c[1]);
        });
     } else {
       console.log('Canceló al eliminar');
     }
   });
 };
    
});






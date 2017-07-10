var app = angular.module("candidatosApp", []);

app.controller("CadastroController", function ($scope, $http) {
    $scope.address = {
        zipcode: null,
        street: null,
        neighborhood: null,
        uf: null,
        city: null,
        unit: null,
        ibge: null,
        gia: null,
    }

    
    /*  $http.get('http://apps.widenet.com.br/busca-cep/api/' + $scope.cep + '.json').then(successCallback, errorCallback);
       function successCallback(response) {
           $scope.local_encontrado = response;//success code
       }
       function errorCallback(error) {
           //error code
       }*/

    function busca() {
        $http.get('http://api.postmon.com.br/v1/cep/' + $scope.cep).then(successCallback, errorCallback);
       function successCallback(response) {
            $scope.local_encontrado = response.data;//success code
        }
        function errorCallback(error) {
            $scope.response.error = { message: error, status: status };//error code
        }
    }
  
    $scope.busca = busca;
 
    $scope.enter = function (e) {
        if (e.keyCode == 13) {
            $scope.busca();
        };
    };

    


    $scope.save = function () {
        var Candidato = {
            nome: $scope.nome,
            cpf: $scope.cpf,
            telefone: $scope.telefone,
            celular: $scope.celular,
            email: $scope.email,
            idPerfil: $scope.idPerfil,
            curriculum: $scope.myFile.name
        };
        $scope.id = 0;
        var Indata = { candidato: Candidato, especialidade: $scope.especialidades}
        $http({
            method: 'POST',
            url: '/Cadastro/Salvar',
            data: Indata
        }).then(successCallback, errorCallback);
        function successCallback(response) {
            $scope.id = response.data;//success code
        }
        function errorCallback(error) {
            $scope.response.error = { message: error, status: status };//error code
        }
       
       // $scope.isViewLoading = false;
      /*  var promisePost = SPACRUDService.post(Student);

        promisePost.then(function (pl) {
            alert("Student Saved Successfully.");
        },
            function (errorPl) {
                $scope.error = 'failure loading Student', errorPl;
            });*/

    };  


  $scope.submitForm = function () {
        $scope.isViewLoading = true;
       // console.log('Form is submitted with:', $scope.cust);

        //$http service that send or receive data from the remote server
        $http({
            method: 'POST',
            url: '/Cadastro/Cadastro1',
            data: $scope.cust
        }).success(function (data, status, headers, config) {
            $scope.errors = [];
            if (data.success === true) {
                $scope.cust = {};
                $scope.message = 'Form data Submitted!';
                $scope.result = "color-green";
                $location.path(data.redirectUrl);
                $window.location.reload();
            }
            else {
                $scope.errors = data.errors;
            }
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
        });
        $scope.isViewLoading = false;
    }

    /*$scope.submitForm = function () {
        $scope.isViewLoading = true;
        console.log('Form is submitted with:', $scope.cust);

        //$http service that send or receive data from the remote server
        $http({
            method: 'POST',
            url: '/Cadastro/Cadastro1',
            data: $scope.cust
        }).success(function (data, status, headers, config) {
            $scope.errors = [];
            if (data.success === true) {
                $scope.cust = {};
                $scope.message = 'Form data Submitted!';
                $scope.result = "color-green";
                $location.path(data.redirectUrl);
                $window.location.reload();
            }
            else {
                $scope.errors = data.errors;
            }
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
        });
        $scope.isViewLoading = false;
    }*/
    $scope.perfis = [];
    $scope.especialidades = [];
   
    $http.get('/Candidato/GetPerfil/').then(successCallback, errorCallback);
    function successCallback(response) {
        $scope.perfis = response.data;//success code
    }
    function errorCallback(error) {
        //error code
    }

    $http.get('/Candidato/GetEspecialidade/').then(successCallback, errorCallback);
    function successCallback(response1) {
        $scope.especialidades = response1.data;//success code
    }
    function errorCallback(error) {
        //error code
    }
    $scope.user = {
        especialidades: []
    };
    //teste
    $scope.perfis1 = [["1", "Lider"], ["2", "Senior"], ["3", "Pleno"], ["4", "Junior"]];

    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {
            switch (args.field) {
                case "myFile":
                    $scope.myFile = args.file;
                    break;
                default:
                    break;
            }
        });
    });
  //  $scope.GetLocList();

         /*   $http({
            method: 'GET',
            url: '/Candidato/Get',
            data: $scope.cust
        }).success(function (data, status, headers, config) {
            $scope.errors = [];
            if (data.success === true) {*/
               /* $scope.cust = {};
                $scope.message = 'Form data Submitted!';
                $scope.result = "color-green";
                $location.path(data.redirectUrl);
                $window.location.reload();*/
         /*   }
            else {
                $scope.errors = data.errors;
            }
        }).error(function (data, status, headers, config) {
            $scope.errors = [];
            $scope.message = 'Unexpected Error while saving data!!';
        });*/

    
   
});


app.directive('fileUpload', function () {
    return {
        scope: true,
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;

                //iterate files since 'multiple' may be specified on the element
                if (files.length == 0) {
                    scope.$emit("fileSelected", { file: null, field: event.target.name });
                } else {
                    for (var i = 0; i < files.length; i++) {
                        var ext = files[i].name.toUpperCase().split('.').pop();
                       // if (ext == "PDF" || ext == "DOCX" || ext == "DOC") {
                            scope.$emit("fileSelected", { file: files[i], field: event.target.name });
                       /* } else {
                            alert('Arquivo inválido');
                            //scope.$emit("fileSelected", { file: null, field: null });
                            //return;
                        }
                      */
                    }
                }
            });
        }
    };
});
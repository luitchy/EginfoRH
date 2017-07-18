var app = angular.module("consultaApp", []);

app.controller("ConsultaController", function ($scope, $http) {

    function busca() {
        if ($scope.cep == undefined) {
            $scope.userForm.$invalid = true;
            $scope.mostrarmsg = false;
            return;
        }
        else {
            $http.get('http://api.postmon.com.br/v1/cep/' + $scope.cep).then(successCallback, errorCallback);
            $scope.userForm.$invalid = true;
            $scope.mensagem = "";
        }

        function successCallback(response) {
            $scope.local_encontrado = response.data;//success code
        }
        function errorCallback(error) {
            $scope.response.error = { message: error, status: status };//error code
        }
    }
    $scope.mensagem = null;
    $scope.emailFormat = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
    $scope.busca = busca;
    $scope.enter = function (e) {
        if (e.keyCode == 13) {
            $scope.busca();
        };
    };
    $scope.candidatos = [];
    $scope.buscar = function () {
        
        $scope.id = 0;
        var Indata = { idPerfil: $scope.idPerfil, especialidade: $scope.especialidadeSelecionada }
        $http({
            method: 'POST',
            url: '/Consulta/Buscar',
            data: Indata
        }).then(function (response) {
            if (response.statusText == "OK") {
                $scope.candidatos = response.data;
            }
        }).catch(function (data, status) {
            $scope.mensagem = "Erro no cadastro";
        })
    };

   

    $scope.getExportFiles = function (fileKey) {
        var req = exportSyncFileReq;
        req.params.fileKey = fileKey;
        var data = RESTCaller.getConfig(req);
        data.then(function (result) {
            if (result) {
                var link = document.createElement('a');
                //link.download = 'filename.ext';
                link.href = result;
                link.click();
            }
        })
    }

    $scope.perfis = [];
    $scope.especialidades = [];

    $http.get('/Consulta/GetPerfil/').then(successCallback, errorCallback);
    function successCallback(response) {
        $scope.perfis = response.data;//success code
    }
    function errorCallback(error) {
        //error code
    }

    $http.get('/Consulta/GetEspecialidade/').then(successCallback, errorCallback);
    function successCallback(response1) {
        $scope.especialidades = response1.data;//success code
    }
    function errorCallback(error) {
        //error code
    }

    $scope.especialidadeSelecionada = [];
    $scope.toggleSelection = function toggleSelection(especialidade) {
        var idx = $scope.especialidadeSelecionada.indexOf(especialidade);
        if (idx > -1) {
            $scope.especialidadeSelecionada.splice(idx, 1);
        }

        else {
            $scope.especialidadeSelecionada.push(especialidade);
        }
    };

    //teste
    $scope.perfis1 = [["0", "TODOS"], ["1", "Sênior"], ["2", "Pleno"], ["3", "Júnior"]];

    $scope.$on("fileSelected", function (event, args) {
        $scope.$apply(function () {
            switch (args.field) {
                case "arquivo":
                    $scope.arquivo = args.file;
                    $scope.userForm.$invalid = false;
                    $scope.mostrarmsg = false;
                    break;
                default:
                    break;
            }
        });
    });

});


app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.factory("rfc4122", function () {
    return {
        newuuid: function () {
            // http://www.ietf.org/rfc/rfc4122.txt
            var s = [];
            var hexDigits = "0123456789abcdef";
            for (var i = 0; i < 36; i++) {
                s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
            }
            s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
            s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
            s[8] = s[13] = s[18] = s[23] = "-";
            return s.join("");
        }
    }
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
                        //$scope.userForm.$valid = true;
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


app.directive('onlyLettersInput', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z\s]/g, '');
                //console.log(transformedInput);
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});


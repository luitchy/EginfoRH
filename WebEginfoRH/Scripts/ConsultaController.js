var app = angular.module('consultaApp', ['ngFileSaver']);

app.controller("ConsultaController", function ($scope, $http, FileSaver) {
    
    $scope.mensagem = null;
    $scope.candidatos = [];
    $scope.buscar = function () {
        
        $scope.id = 0;
        var Indata = { idPerfil: $scope.idPerfil, especialidade: $scope.especialidadeSelecionada , cidade: $scope.cidade}
        $http({
            method: 'POST',
            url: '/Candidato/Buscar',
            data: Indata
        }).then(function (response) {
            if (response.statusText == "OK") {
                $scope.candidatos = response.data;
            }
        }).catch(function (data, status) {
            $scope.mensagem = "Erro no consulta";
        })
    };
    
  

    $http.get('/Candidato/GetEspecialidade/').then(successCallback, errorCallback);
    function successCallback(response1) {
        $scope.especialidades = response1.data;//success code
    }
    function errorCallback(error) {
        //error code
    }

    $scope.obterperfil = function (id) {
         switch (id)
        {
            case 1:
                return "Sênior";
                break;
            case 2:
                return "Pleno";
            case 3:
                return "Júnior";

        }
        return nome;

    }

    $scope.downloadFile = function (name) {
        $http({
            method: 'GET',
            url: '/Candidato/Download?FileName=' + name,
            responseType: 'arraybuffer'
        }).then(function (response) {
            var linkElement = document.createElement('a');
            if (typeof response.data === 'object') {
                var data = JSON.stringify(response, undefined, 2);
            }
            var byteArray = new Uint8Array(response.data);
            try {
                var blob = new Blob([byteArray], { type: 'application/octet-stream' });
                var blobUrl = URL.createObjectURL(blob);
                FileSaver.saveAs(blob, name);
                
            } catch (ex) {
                console.log(ex);
            }
            //}
        }).catch(function (data, status) {
            $scope.mensagem = "Erro no cadastro";
        })
    };

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

    $scope.perfis1 = [["0", "TODOS"], ["1", "Sênior"], ["2", "Pleno"], ["3", "Júnior"]];



});






﻿var app = angular.module("candidatosApp", ['ui.utils.masks']);

app.controller("CadastroController", function ($scope, $http) {

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
    $scope.mostrarmsg = false;
    $scope.save = function () {
        if ($scope.arquivo == undefined) {
            $scope.mensagem = " Por favor seleccione arquivo, Somente serão aceitos arquivos no formato PDF, DOC e DOCX.";
            return;
        }
        var Candidato = {
            nome: $scope.nome,
            cpf: $scope.cpf,
            telefone: $scope.telefone,
            celular: $scope.celular,
            email: $scope.email,
            idPerfil: $scope.idPerfil,
            curriculo: $scope.arquivo.name
        };
        var Endereco = {
            cep: $scope.cep,
            logradouro: $scope.local_encontrado.logradouro,
            bairro: $scope.local_encontrado.bairro,
            cidade: $scope.local_encontrado.cidade,
            estado: $scope.local_encontrado.estado,
            numero: $scope.local_encontrado.numero
        }
        $scope.id = 0;
        var Indata = { candidato: Candidato, especialidade: $scope.especialidadeSelecionada, endereco: Endereco }
        $http({
            method: 'POST',
            url: '/Candidato/Salvar',
            data: Indata
        }).then(function (response) {
            if (response.statusText == "OK") {

                if ($scope.arquivo.name.toUpperCase().split('.').pop() == "DOC" ||
                    $scope.arquivo.name.toUpperCase().split('.').pop() == "DOCX" ||
                    $scope.arquivo.name.toUpperCase().split('.').pop() == "PDF") {
                    var file = $scope.arquivo;
                    var idCandidato = response.data;
                    var candidato = Candidato;
                    var especialidade = $scope.especialidadeSelecionada;
                    console.log('file is ');
                    console.dir(file);
                    var uploadUrl = "/Candidato/FileUpload";
                    var fd = new FormData();
                    fd.append('file', file);
                    fd.append('idCandidato', idCandidato);
                    fd.append('candidatoNome', Candidato.nome);
                    fd.append('candidatoCelular', Candidato.celular);
                    fd.append('candidatoEmail', Candidato.email);
                    fd.append('candidatoIdPerfil', Candidato.idPerfil);
                    fd.append('especialidade', especialidade);
                    $http.post(uploadUrl, fd, {
                        transformRequest: angular.identity,
                        headers: { 'Content-Type': undefined }
                    }).then(function (response) {
                        if (response.statusText == "OK") {
                            $scope.mensagem = "E-mail enviado com sucesso!";
                            window.location.reload();
                        }
                    }).catch(function (e) {
                        $scope.mensagem = "Erro no envio do E-mail ";

                    });
                }

                else {
                    $scope.mensagem = "Somente serão aceitos arquivos no formato PDF, DOC e DOCX.";
                }
            }
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
    
    $scope.perfis = [["1", "Sênior"], ["2", "Pleno"], ["3", "Júnior"]];

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

app.directive('fileUpload', function () {
    return {
        scope: true,
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                if (files.length == 0) {
                    scope.$emit("fileSelected", { file: null, field: event.target.name });
                } else {
                    for (var i = 0; i < files.length; i++) {
                        var ext = files[i].name.toUpperCase().split('.').pop();
                        // if (ext == "PDF" || ext == "DOCX" || ext == "DOC") {
                        scope.$emit("fileSelected", { file: files[i], field: event.target.name });
                       
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


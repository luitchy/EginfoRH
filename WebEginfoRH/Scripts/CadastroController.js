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

    //teste
    $scope.perfis1 = [["1", "Lider"], ["2", "Senior"], ["3", "Pleno"]];

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

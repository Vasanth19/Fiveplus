'use strict';

/// <reference path="../data/Gigs.js" />
/// <reference path="directives.js" />
/// <reference path="../_partials/hmGigItem.html" />
var myDirectives = angular.module("myDirectives", ['ngMessages']);

myDirectives.directive('easyBlock', function() {
    return {
        restrict: "E",
        replace: true,
        scope: {
            gig: '='
        },
        templateUrl: "/ng-App/directives/_html/easyBlock.html",
        controller: function($scope) {
            //Variables
            $scope.displayProps = [];
            $scope.displayProps.favorite = false;

            //http://jsfiddle.net/EQmSN/ for more zooming
            $(".zoomfancybox").fancybox({
                arrows: false,
                openEffect: 'elastic',
                openSpeed: 150,
                closeEffect: 'elastic',
                closeSpeed: 150,
                closeClick: true
            });

            //Methods

        },
        link:function(scope,element) {
           $("#gig1media1").addClass("active");
        }
    };
});



myDirectives.directive('hmCategorySearch', function () {
    return {
        restrict: "E",
        replace: true,
        scope: {
            categories: '=',
            selected: '&'
        },
        templateUrl: "/ng-App/directives/_html/hmCategorySearch.html",
        controller: function ($scope) {
            //Variables

            //Methods

        }
    };
});


//https://github.com/banafederico/angularjs-country-select
myDirectives.directive('countrySelect', ['$parse', function ($parse)
  {
      var countries = [
        "United States", "United States Minor Outlying Islands", "India", "Afghanistan", "Aland Islands", "Albania", "Algeria", "American Samoa", "Andorra", "Angola",
        "Anguilla", "Antarctica", "Antigua And Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria",
        "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin",
        "Bermuda", "Bhutan", "Bolivia, Plurinational State of", "Bonaire, Sint Eustatius and Saba", "Bosnia and Herzegovina",
        "Botswana", "Bouvet Island", "Brazil",
        "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia",
        "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China",
        "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo",
        "Congo, the Democratic Republic of the", "Cook Islands", "Costa Rica", "Cote d'Ivoire", "Croatia", "Cuba",
        "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt",
        "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)",
        "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia",
        "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece",
        "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guernsey", "Guinea",
        "Guinea-Bissau", "Guyana", "Haiti", "Heard Island and McDonald Islands", "Holy See (Vatican City State)",
        "Honduras", "Hong Kong", "Hungary", "Iceland", "Indonesia", "Iran, Islamic Republic of", "Iraq",
        "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya",
        "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Kyrgyzstan",
        "Lao People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya",
        "Liechtenstein", "Lithuania", "Luxembourg", "Macao", "Macedonia, The Former Yugoslav Republic Of",
        "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique",
        "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of",
        "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru",
        "Nepal", "Netherlands", "New Caledonia", "New Zealand", "Nicaragua", "Niger",
        "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau",
        "Palestinian Territory, Occupied", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines",
        "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation",
        "Rwanda", "Saint Barthelemy", "Saint Helena, Ascension and Tristan da Cunha", "Saint Kitts and Nevis", "Saint Lucia",
        "Saint Martin (French Part)", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "Samoa", "San Marino",
        "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore",
        "Sint Maarten (Dutch Part)", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa",
        "South Georgia and the South Sandwich Islands", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname",
        "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic",
        "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Timor-Leste",
        "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan",
        "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom",
        "Uruguay", "Uzbekistan", "Vanuatu",
        "Venezuela, Bolivarian Republic of", "Viet Nam", "Virgin Islands, British", "Virgin Islands, U.S.",
        "Wallis and Futuna", "Western Sahara", "Yemen", "Zambia", "Zimbabwe"
      ];

      return {
          restrict: 'EA',
          template: '<select><option>' + countries.join('</option><option>') + '</option></select>',
          replace: true,
          link: function (scope, elem, attrs)
          {
              if (!!attrs.ngModel) {
                  var assignCountry = $parse(attrs.ngModel).assign;

                  elem.bind('change', function (e)
                  {
                      assignCountry(elem.val());
                  });

                  scope.$watch(attrs.ngModel, function (country)
                  {
                      elem.val(country);
                  });
              }
          }
      };
}]);



var compareTo = function ()
{
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel)
        {

            ngModel.$validators.compareTo = function (modelValue)
            {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function ()
            {
                ngModel.$validate();
            });
        }
    };
};
myDirectives.directive("compareTo", compareTo);


myDirectives.directive('usernameAvailableValidator', [
    '$http', function($http) {
        return {
            require: 'ngModel',
            link: function($scope, element, attrs, ngModel) {
                ngModel.$asyncValidators.usernameAvailable = function(username) {
                    return $http.get('/manage/userNamecheck/ ' + username);
                };
            }
        };
    }
]);


myDirectives.directive('hideErrors',
    function ()
    {
        return {
            restrict: 'A',
            require: '^form',
            link: function ($scope, element, attrs, formCtrl)
            {
             $scope.$on('hide-errors-event', function () {
                 console.log(element);

                 //element.removeClass('state-error');
                 //element.removeClass('state-success');

                 var label = element.find('label');
                 label.removeClass('state-error');
                 label.removeClass('state-success');

               
                 var span = element.find('span');
                 span.addClass('hide');


                 //angular.forEach(element.find('span'), function (spanElement)
                 //{
                 //    spanElement = angular.element(spanElement);
                 //    spanElement.attr('class', 'hide');
                 //});


             });
            }
        };
    }
);


myDirectives.directive('ncgInput', function ($compile, $http, $templateCache, $interpolate)
{
    return {
        restrict: "E",
        replace: true,
        priority: 100,        // We need this directive to happen before ng-model
        terminal: true,       // We are going to deal with this element
        require: '?^form',
        compile: function compile(element, attrs) {

            var statusIcon = false; // default
            if (element.attr('statusicon')) {
                statusIcon = element.attr('statusicon');
                element[0].removeAttribute('statusicon');
            }


            // Load up the template for this kind of field
            var template = attrs.template || 'input';   // Default to the simple input if none given
            var getFieldElement = $http.get('/ng-App/directives/_html/ncgInput.html', { cache: $templateCache }).then(function (response)
            {
                var newElement = angular.element(response.data);
                var inputElement = angular.element(newElement.find('input')[0]);

                // Copy over the attributes to the input element
                // At least the ng-model attribute must be copied because we can't use interpolation in the template
                angular.forEach(element[0].attributes, function (attribute)
                {
                    var value = attribute.value;
                    var key = attribute.name;
                    inputElement.attr(key, value);
                });

                var labelContent = '';
                if (element.attr('label')) {
                    labelContent = element.attr('label');
                    element[0].removeAttribute('label');
                    var labelElement = newElement.find('.control-label');
                    labelContent = labelContent + labelElement[0].innerHTML;
                    labelElement.html(labelContent);
                } else {
                   newElement.find('.control-label').remove();
                }
             
             
                
                return newElement;
            });

            return function (scope, element, attrs, formController)
            {
                // We have to wait for the field element template to be loaded
                getFieldElement.then(function (newElement)
                {
                    // Our template will have its own child scope
                    var childScope = scope.$new();
                    childScope.statusIcon = statusIcon;

                    // Generate an id for the input from the ng-model expression
                    // (we need to replace dots with something to work with browsers and also form scope)
                    // (We couldn't do this in the compile function as we need the scope to
                    // be able to calculate the unique id)
                    childScope.$modelId = attrs.ngModel.replace('.', '_').toLowerCase() + '_' + childScope.$id;

                    // Wire up the input (id and name) and its label (for)
                    // (We need to set the input element's name here before we compile.
                    // If we leave it to interpolation, the formController doesn't pick it up)
                    var inputElement = angular.element(newElement.find('input')[0]);
                    inputElement.attr('name', childScope.$modelId);
                    inputElement.attr('id', childScope.$modelId);
                    newElement.find('label').attr('for', childScope.$modelId);

                    // Added by Vasanth
                    var iconContent = '';
                    if (element.attr('icon')) {
                        iconContent = element.attr('icon');
                        element[0].removeAttribute('icon');
                        newElement.find('#displayIcon').attr('class', iconContent);
                    }

                    var tooltipContent = '';
                    if (element.attr('tooltip')) {
                        tooltipContent = element.attr('tooltip');
                        element[0].removeAttribute('tooltip');
                        newElement.find('.tooltip').html(tooltipContent);
                    } else {
                        newElement.find('.tooltip').attr('class', 'Hide');
                    }

                    //Copy Custum ngMessage to the directive

                    var ngMessageContent = element[0].innerHTML.trim();
                    if (ngMessageContent != '')
                    { newElement.find('.ngMessages').html(ngMessageContent); }
                    

                    // We must compile our new element in the postLink function rather than in the compile function
                    // (i.e. after any parent form element has been linked)
                    // otherwise the new input won't pick up the FormController
                    $compile(newElement)(childScope, function (clone)
                    {
                        // Place our new element after the original element
                        element.after(clone);
                        // Remove our original element
                        element.remove();
                    });

                    // Only after the new element has been compiled do we have access to the ngModelController
                    // (i.e. formController[childScope.name])
                    if (formController) {
                        childScope.$form = formController;
                        childScope.$field = formController[childScope.$modelId];
                    }
                });
            };
        }

    };

});
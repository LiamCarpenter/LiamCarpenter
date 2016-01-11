angular.module('OnlineMI.DashboardController', ['ngSanitize'])
    .controller('DashboardCtrl', ['$scope', '$http', '$sce', function ($scope, $http, $sce) {

        $scope.dashboard;

        $scope.displayComplaints;
        $scope.complaints;

        $scope.displayFlights;
        $scope.flights;

        $scope.displayUkRail;
        $scope.ukrail;

        $scope.displayIntRail;
        $scope.intrail;

        $scope.displayAccommodation;
        $scope.accommodation;

        $scope.displayConference;
        $scope.conference;

        $scope.displayFeedback;
        $scope.feedback;

        $scope.displayConferenceFeedback;
        $scope.conferencefeedback;

        $scope.displayCorporateFeedback;
        $scope.corporatefeedback;
        
        $scope.uoycompanies = [];
        $scope.flightsSelectedDept = '';

        $scope.states = {
            showComplaints: false,
            showFlights: false,
            showRail: false,
            showAccommodation: false,
            showConference: false,
            showFeedback: false,
            clientUOY: false,
            isLoading: false,
            feedbackCount: 0,
            flightCount: 0,
            railCount: 0,
            accommodationCount: 0,
            complaintCount: 0,
            conferenceCount: 0
        };
        $scope.pageSize = 15;
        $scope.utilities = {
            DetailsTitle: 'Not Set'
        }

        $scope.states.isLoading = true;

        //Get UOY companies
        //$http.get('/Home/GetUOYCompanies').success(function (data) {
        //    $scope.uoycompanies = data;
        //})

        $http.get('/Home/GetIndexData').success(function (data) {
            $scope.dashboard = data;

            if (data[0].ClientName == "University of York")
            {
                $scope.states.clientUOY = true;
            }

            $scope.states.feedbackCount = +data[0].ConferenceFeedback + +data[0].CorporateFeedback;
            $scope.states.conferenceCount = data[0].Conference;
            $scope.states.accommodationCount = data[0].Accom;
            $scope.states.flightCount = data[0].Flights;
            $scope.states.railCount = data[0].Rail + data[0].IR;
            $scope.states.complaintCount = data[0].Complaints;

            $scope.states.isLoading = false;
        }).error(function (error) {
            $scope.states.isLoading = false;
            alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
        });



        $scope.getRevisedDashboardData = function () {

            $scope.states.isLoading = true;

            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                url = '/Home/GetIndexData';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                url = '/Home/GetIndexData?FromDate=' + fromDate + '&ToDate=' + toDate;
            }


            $http.get(url).success(function (data) {

                $scope.dashboard = data;

                $scope.complaints = {};
                $scope.flights = {};
                $scope.ukrail = {};
                $scope.intrail = {};
                $scope.accommodation = {};
                $scope.conference = {};
                $scope.feedback = {};

                $scope.states.feedbackCount = +data[0].conferencefeedback + +data[0].corporatefeedback;
                $scope.states.conferenceCount = data[0].Conference;
                $scope.states.accommodationCount = data[0].Accom;
                $scope.states.flightCount = data[0].Flights;
                $scope.states.railCount = data[0].Rail + data[0].IR;
                $scope.states.complaintCount = data[0].Complaints;

                $scope.states.isLoading = false;

            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });
        }

        $scope.getComplaints = function () {

            $scope.states.isLoading = true;

                var url;
                if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                    //Empty so use default
                    url = '/Home/GetComplaints';
                }
                else if ($('#FromDate').val() == '' || $('#ToDate').val() == '')
                {
                    alert("Please complete both date fields before continuing.");
                }
                else {
                    //Use the dates
                    var fromDate = $('#FromDate').val();
                    var toDate = $('#ToDate').val();
                    url = '/Home/GetComplaints?FromDate=' + fromDate + '&ToDate=' + toDate;
                }


                $http.get(url).success(function (data) {
       
                    for (var i = 0; i < data.length; i++) {
                        for (var j in data[i]) {
                            if (data[i][j] != null) {
                                if (data[i][j].toString().indexOf("Date") > 0) {

                                    data[i][j] = CorrectAngularJsDate(data[i][j]);
                                }
                            }
                        }
                    }

                    $scope.complaints = data;
                    
                    $scope.displayComplaints = [].concat($scope.complaints);

                    $scope.states.showComplaints = true;
                    $scope.states.showFlights = false;
                    $scope.states.showRail = false;
                    $scope.states.showAccommodation = false;
                    $scope.states.showConference = false;
                    $scope.states.showFeedback = false;

                    $scope.utilities.DetailsTitle = 'Complaint Details';

                    $('#modDetailsPopup').modal('show');

                    $scope.states.isLoading = false;
                }).error(function (error) {
                    $scope.states.isLoading = false;
                    alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
                });
        }

        $scope.downloadComplaints = function () {
            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                url = '/Home/DownloadComplaints';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                url = '/Home/DownloadComplaints?FromDate=' + fromDate + '&ToDate=' + toDate;
            }

            window.location.replace(url);

        }

        $scope.getFlights = function () {

            $scope.states.isLoading = true;

                var url;
                if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                    //Empty so use default
                    url = '/Home/GetFlights';
                }
                else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                    alert("Please complete both date fields before continuing.");
                }
                else {
                    //Use the dates
                    var fromDate = $('#FromDate').val();
                    var toDate = $('#ToDate').val();
                    url = '/Home/GetFlights?FromDate=' + fromDate + '&ToDate=' + toDate;
                }


                $http.get(url).success(function (data) {

                    for (var i = 0; i < data.length; i++)
                    {
                        for (var j in data[i])
                        {
                            if (data[i][j] != null)
                            {
                                if (data[i][j].toString().indexOf("Date") > 0) {

                                    data[i][j] = CorrectAngularJsDate(data[i][j]);
                                }
                            }
                        }
                    }

                    $scope.flights = data;

                    $scope.displayFlights = [].concat($scope.flights);

                    $scope.states.showFlights = true;
                    $scope.states.showComplaints = false;
                    $scope.states.showRail = false;
                    $scope.states.showAccommodation = false;
                    $scope.states.showConference = false;
                    $scope.states.showFeedback = false;

                    $scope.utilities.DetailsTitle = 'Flight Details';

                    $('#modDetailsPopup').modal('show');

                    $scope.states.isLoading = false;
                }).error(function (error) {
                    $scope.states.isLoading = false;
                    alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
                });

            }

        $scope.downloadFlights = function () {
            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default

                if ($('#FlightDepartment').val() == '') {
                    url = '/Home/DownloadFlights';
                } else {
                    url = '/Home/DownloadFlights?FlightDepartment=' + $('#FlightDepartment').val();
                }
                
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();

                if ($('#FlightDepartment').val() == '') {
                    url = '/Home/DownloadFlights?FromDate=' + fromDate + '&ToDate=' + toDate;
                } else {
                    url = '/Home/DownloadFlights?FromDate=' + fromDate + '&ToDate=' + toDate + '&FlightDepartment=' + $('#FlightDepartment').val();
                }

            }

            window.location.replace(url);

        }

        $scope.getRail = function () {

            $scope.states.isLoading = true;

            var ukRailUrl;
            var intRailUrl;

            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                ukRailUrl = '/Home/GetUKRail';
                intRailUrl = '/Home/GetIntRail';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                ukRailUrl = '/Home/GetUKRail?FromDate=' + fromDate + '&ToDate=' + toDate;
                intRailUrl = '/Home/GetIntRail?FromDate=' + fromDate + '&ToDate=' + toDate;
            }


            $http.get(ukRailUrl).success(function (data) {

                for (var i = 0; i < data.length; i++) {
                    for (var j in data[i]) {
                        if (data[i][j] != null) {
                            if (data[i][j].toString().indexOf("Date") > 0) {

                                data[i][j] = CorrectAngularJsDate(data[i][j]);
                            }
                        }
                    }
                }

                $scope.ukrail = data;
                $scope.displayUkRail = [].concat($scope.ukrail);

                $scope.states.isLoading = false;
            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });

            $http.get(intRailUrl).success(function (data) {

                for (var i = 0; i < data.length; i++) {
                    for (var j in data[i]) {
                        if (data[i][j] != null) {
                            if (data[i][j].toString().indexOf("Date") > 0) {

                                data[i][j] = CorrectAngularJsDate(data[i][j]);
                            }
                        }
                    }
                }

                $scope.intrail = data;

                $scope.displayIntRail = [].concat($scope.intrail);

                $scope.states.isLoading = false;
            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });

            $scope.states.showComplaints = false;
            $scope.states.showFlights = false;
            $scope.states.showAccommodation = false;
            $scope.states.showRail = true;
            $scope.states.showConference = false;
            $scope.states.showFeedback = false;

            $scope.utilities.DetailsTitle = 'Rail Details';

            $('#modDetailsPopup').modal('show');
        }

        $scope.downloadRail = function () {
            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
               
                if ($('#UKRailDepartment').val() == '' && $('#IntRailDepartment').val == ''){
                    url = '/Home/DownloadRail';
                } else if ($('#UKRailDepartment').val() != '' && $('#IntRailDepartment').val != '') {
                    url = '/Home/DownloadRail?UKRailDepartment=' + $('#UKRailDepartment').val() + '&IntRailDepartment=' + $('#IntRailDepartment').val();
                } else if ($('#UKRailDepartment').val() != '' && $('#IntRailDepartment').val == '') {
                    url = '/Home/DownloadRail?UKRailDepartment=' + $('#UKRailDepartment').val();
                } else if ($('#UKRailDepartment').val() == '' && $('#IntRailDepartment').val != '') {
                    url = '/Home/DownloadRail?IntRailDepartment=' + $('#IntRailDepartment').val();
                }

            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                
                if ($('#UKRailDepartment').val() == '' && $('#IntRailDepartment').val == '') {
                    url = '/Home/DownloadRail?FromDate=' + fromDate + '&ToDate=' + toDate;
                } else if ($('#UKRailDepartment').val() != '' && $('#IntRailDepartment').val != '') {
                    url = '/Home/DownloadRail?FromDate=' + fromDate + '&ToDate=' + toDate + '&UKRailDepartment=' + $('#UKRailDepartment').val() + '&IntRailDepartment=' + $('#IntRailDepartment').val();
                } else if ($('#UKRailDepartment').val() != '' && $('#IntRailDepartment').val == '') {
                    url = '/Home/DownloadRail?FromDate=' + fromDate + '&ToDate=' + toDate + '&UKRailDepartment=' + $('#UKRailDepartment').val();
                } else if ($('#UKRailDepartment').val() == '' && $('#IntRailDepartment').val != '') {
                    url = '/Home/DownloadRail?FromDate=' + fromDate + '&ToDate=' + toDate + '&IntRailDepartment=' + $('#IntRailDepartment').val();
                }

            }

            window.location.replace(url);

        }

        $scope.getAccommodation = function () {
            $scope.states.isLoading = true;

            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                url = '/Home/GetAccommodation';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                url = '/Home/GetAccommodation?FromDate=' + fromDate + '&ToDate=' + toDate;
            }


            $http.get(url).success(function (data) {

                for (var i = 0; i < data.length; i++) {
                    for (var j in data[i]) {
                        if (data[i][j] != null) {
                            if (data[i][j].toString().indexOf("Date") > 0) {

                                data[i][j] = CorrectAngularJsDate(data[i][j]);
                            }
                        }
                    }
                }

                $scope.accommodation = data;
                $scope.displayAccommodation = [].concat($scope.accommodation);

                $scope.states.showFlights = false;
                $scope.states.showComplaints = false;
                $scope.states.showRail = false;
                $scope.states.showConference = false;
                $scope.states.showAccommodation = true;
                $scope.states.showFeedback = false;

                $scope.utilities.DetailsTitle = 'Accommodation Details';

                $('#modDetailsPopup').modal('show');

                $scope.states.isLoading = false;
            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });

        }

        $scope.downloadAccommodation = function () {
            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                if ($('#AccommodationDepartment').val() == '') {
                    url = '/Home/DownloadAccommodation';
                } else {
                    url = '/Home/DownloadAccommodation?AccommodationDepartment=' + $('#AccommodationDepartment').val();
                }

            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();

                if ($('#AccommodationDepartment').val() == '') {
                    url = '/Home/DownloadAccommodation?FromDate=' + fromDate + '&ToDate=' + toDate;
                } else {
                    url = '/Home/DownloadAccommodation?FromDate=' + fromDate + '&ToDate=' + toDate + '&AccommodationDepartment=' + $('#AccommodationDepartment').val();
                }
            }

            window.location.replace(url);

        }

        $scope.getConferences = function () {
            $scope.states.isLoading = true;

            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                url = '/Home/GetConferences';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                url = '/Home/GetConferences?FromDate=' + fromDate + '&ToDate=' + toDate;
            }


            $http.get(url).success(function (data) {

                for (var i = 0; i < data.length; i++) {
                    for (var j in data[i]) {
                        if (data[i][j] != null) {
                            if (data[i][j].toString().indexOf("Date") > 0) {

                                data[i][j] = CorrectAngularJsDate(data[i][j]);
                            }
                        }
                    }
                }

                $scope.conference = data;
                $scope.displayConference = [].concat($scope.conference);

                $scope.states.showFlights = false;
                $scope.states.showComplaints = false;
                $scope.states.showRail = false;
                $scope.states.showAccommodation = false;
                $scope.states.showConference = true;
                $scope.states.showFeedback = false;

                $scope.utilities.DetailsTitle = 'Conference Details';

                $('#modDetailsPopup').modal('show');

                $scope.states.isLoading = false;
            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });

        }

        $scope.downloadConferences = function () {
            //var url;
            //if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
            //    //Empty so use default
            //    url = '/Home/DownloadConferences';
            //}
            //else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
            //    alert("Please complete both date fields before continuing.");
            //}
            //else {
            //    //Use the dates
            //    var fromDate = $('#FromDate').val();
            //    var toDate = $('#ToDate').val();
            //    url = '/Home/DownloadConferences?FromDate=' + fromDate + '&ToDate=' + toDate;
            //}

            //window.location.replace(url);
            alert('To finalise');

        }

        $scope.getFeedback = function () {

            $scope.states.isLoading = true;

            var conferencefeedbackUrl;
            var corporatefeedbackUrl;

            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                conferencefeedbackUrl = '/Home/GetConferenceFeedback';
                corporatefeedbackUrl = '/Home/GetCorporateFeedback';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                conferencefeedbackUrl = '/Home/GetConferenceFeedback?FromDate=' + fromDate + '&ToDate=' + toDate;
                corporatefeedbackUrl = '/Home/GetCorporateFeedback?FromDate=' + fromDate + '&ToDate=' + toDate;
            }


            $http.get(conferencefeedbackUrl).success(function (data) {

                for (var i = 0; i < data.length; i++) {
                    for (var j in data[i]) {
                        if (data[i][j] != null) {
                            if (data[i][j].toString().indexOf("Date") > 0) {

                                data[i][j] = CorrectAngularJsDate(data[i][j]);
                            }
                        }
                    }
                }

                $scope.conferencefeedback = data;
                $scope.displayConferenceFeedback = [].concat($scope.conferencefeedback);

                $scope.conferencefeedback.length + $scope.corporatefeedback.length;

                $scope.states.isLoading = false;
            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });

            $http.get(corporatefeedbackUrl).success(function (data) {

                for (var i = 0; i < data.length; i++) {
                    for (var j in data[i]) {
                        if (data[i][j] != null) {
                            if (data[i][j].toString().indexOf("Date") > 0) {

                                data[i][j] = CorrectAngularJsDate(data[i][j]);
                            }
                        }
                    }
                }

                $scope.corporatefeedback = data;

                $scope.displayCorporateFeedback = [].concat($scope.corporatefeedback);

                $scope.states.isLoading = false;
            }).error(function (error) {
                $scope.states.isLoading = false;
                alert('An error occurred retreiving the data, if the problem persists please contact the NYS IT Helpdesk.');
            });

            $scope.states.showComplaints = false;
            $scope.states.showFlights = false;
            $scope.states.showAccommodation = false;
            $scope.states.showRail = false;
            $scope.states.showConference = false;
            $scope.states.showFeedback = true;

            $scope.utilities.DetailsTitle = 'Feedback Details';

            $('#modDetailsPopup').modal('show');
        }

        $scope.downloadFeedback = function () {
            var url;
            if ($('#FromDate').val() == '' && $('#ToDate').val() == '') {
                //Empty so use default
                url = '/Home/DownloadFeedback';
            }
            else if ($('#FromDate').val() == '' || $('#ToDate').val() == '') {
                alert("Please complete both date fields before continuing.");
            }
            else {
                //Use the dates
                var fromDate = $('#FromDate').val();
                var toDate = $('#ToDate').val();
                url = '/Home/DownloadFeedback?FromDate=' + fromDate + '&ToDate=' + toDate;
            }

            window.location.replace(url);

        }

        function CorrectAngularJsDate(angularDateString)
        {
            var d = new Date(parseInt(angularDateString.toString().substr(6)));
            var curr_date = d.getDate();
            var curr_month = d.getMonth() + 1; //Months are zero based
            var curr_year = d.getFullYear();

            return d;
        }


        $scope.getTravellerTracking = function () {

            $('#modTravellerTrackingPopup').modal('show');



        }

    }]);
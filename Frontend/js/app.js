"use strict";

ko.observableArray.fn.url = function (url) {
    this.url = url;
}

ko.observableArray.fn.fetchData = function () {
    var self = this;
    jQuery.support.cors = true;
    $.ajax({
        url: this.url,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            self.removeAll();
            for (var i = 0; i < data.length; i++) {
                self.push(ko.mapping.fromJS(data[i]));
            }
        }
    });
}

ko.observableArray.fn.add = function (item) {
    var self = this;
    jQuery.support.cors = true;
    $.ajax({
        url: this.url,
        type: 'POST',
        data: JSON.stringify(item),
        contentType: 'application/json',
        success: function (data) {
            self.push(ko.mapping.fromJS(item));
        }
    });
}

ko.observableArray.fn.delete = function (item) {
    var self = this;
    jQuery.support.cors = true;
    $.ajax({
        url: this.url + '/' + item.Id(),
        type: 'DELETE',
        success: function (data) {
            self().splice(index, 1);
            console.log(data);
        }
    });
}

ko.observableArray.fn.update = function (item) {
    var self = this;
    jQuery.support.cors = true;
    $.ajax({
        url: this.url + '/' + item.Id(),
        type: 'PUT',
        dataType: 'json',
        contentType: 'application/json',
        data: ko.toJSON(item),
        success: function (data) {
            console.log(data);
        }
    });
}

var viewModel = {
    courses: ko.observableArray(),
    students: ko.observableArray(),
    studentsGrades: ko.observableArray(),
    getStudentsGrades: function (item) {
        jQuery.support.cors = true;
        $.ajax({
            url: 'http://localhost:54472/api/students/' + item.Id() + "/grades",
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                viewModel.studentsGrades.removeAll();
                for (var i = 0; i < data.length; i++) {
                    viewModel.studentsGrades.push(ko.mapping.fromJS(data[i]));
                }
                console.log(viewModel.studentsGrades());
                window.location.replace('/index.html#students-grades');
            }
        });
    }
};

$(document).ready(function () {
    ko.applyBindings(viewModel);
    viewModel.courses.url('http://localhost:54472/api/courses');
    viewModel.students.url('http://localhost:54472/api/students');
    viewModel.studentsGrades.url('http://localhost:54472/api/grades');
    viewModel.courses.fetchData();
    viewModel.students.fetchData();
    viewModel.studentsGrades.fetchData();

    setInterval(function () {
        //        console.log(viewModel.courses()[0].Lecturer());
        //        console.log(viewModel.studentsGrades()[2].Course());
    }, 1000);

});
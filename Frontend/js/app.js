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

ko.observableArray.fn.add = function (array, item) {
    var self = this;
    jQuery.support.cors = true;
    $.ajax({
        url: this.url,
        type: 'POST',
        data: JSON.stringify(item),
        contentType: 'application/json',
        success: function (data) {
            //            self.push(ko.mapping.fromJS(item));
            self.fetchData();
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
            self.remove(function (i) {
                return i.Id() == item.Id();
            });
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
        success: function (data) {}
    });
}

var getStudentGrades = function (item) {
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
            viewModel.newGrade.student(item.Id());
            viewModel.currentStudent.firstName(item.FirstName());
            viewModel.currentStudent.secondName(item.SecondName());
            window.location.replace('/index.html#students-grades');
        }
    });
}

var addNewGrade = function () {
    console.log(viewModel.newGrade.student());
    var self = this;
    jQuery.support.cors = true;
    $.ajax({
        url: 'http://localhost:54472/api/grades',
        type: 'POST',
        data: JSON.stringify(viewModel.newGrade),
        contentType: 'application/json',
        success: function (data) {
            //            self.push(ko.mapping.fromJS(item));
            self.fetchData();
        }
    });
}

var viewModel = {
    courses: ko.observableArray(),
    students: ko.observableArray(),
    studentsGrades: ko.observableArray(),
    currentStudent: {
        id: new ko.observable(),
        firstName: new ko.observable(),
        secondName: new ko.observable(),
        fullName: function () {
            return this.firstName() + " " + this.secondName();
        }
    },
    studentGrades: getStudentGrades,
    newCourse: {
        name: new ko.observable(),
        lecturer: new ko.observable()
    },
    newStudent: {
        firstName: new ko.observable(),
        secondName: new ko.observable(),
        birthDate: new ko.observable()
    },
    newGrade: {
        value: new ko.observable(),
        course: new ko.observable(),
        issued: new ko.observable(),
        student: new ko.observable()
    },
    addStudentGrade: addNewGrade
};

$(document).ready(function () {
    ko.applyBindings(viewModel);
    viewModel.courses.url('http://localhost:54472/api/courses');
    viewModel.students.url('http://localhost:54472/api/students');
    viewModel.studentsGrades.url('http://localhost:54472/api/grades');
    viewModel.courses.fetchData();
    viewModel.students.fetchData();
    viewModel.studentsGrades.fetchData();
});
//
// Piranha CMS
// Copyright (c) 2014, Håkan Edling, All rights reserved.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library.
//

if (!manager.models)
	manager.models = {};

//
// Alias view model
//
manager.models.alias = function (locale) {
	'use strict';

	var self = window.alias = this;

	// Labels & texts
	self.addTitle = locale.addTitle;
	self.editTitle = locale.editTitle;

	// Members
	self.title = ko.observable(self.addTitle);
	self.id = ko.observable('');
	self.oldUrl = ko.observable('');
	self.oldUrlValid = ko.observable(true);
	self.newUrl = ko.observable('');
	self.newUrlValid = ko.observable(true);
	self.isPermanent = ko.observable(false);
	self.items = ko.observableArray([]);

	// Initializes the model
	self.init = function () {
		$.ajax({
			url: baseUrl + 'manager/aliases/get',
			type: 'GET',
			dataType: 'json',
			success: function (result) {
				if (result.success)
					self.items(result.data);
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Edits the specified author
	self.edit = function (id) {
		$.ajax({
			url: baseUrl + 'manager/alias/get/' + id,
			type: 'GET',
			dataType: 'json',
			success: function (result) {
				if (result.success) {
					self.title(self.editTitle);
					self.id(result.data.Id);
					self.oldUrl(result.data.OldUrl);
					self.oldUrlValid(true);
					self.newUrl(result.data.NewUrl);
					self.newUrlValid(true);
					self.isPermanent(result.data.IsPermanent);
					$('.collapse').collapse('show');
				}
				$('.table tr').removeClass('active');
				$('.table tr[data-id="' + id + '"]').addClass('active');
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Validates the current model
	self.validate = function () {
		var ret = true;

		if (self.oldUrl() == null || self.oldUrl() == '' || self.oldUrl().length > 255) {
			self.oldUrlValid(false);
			ret = false;
		}
		if (self.newUrl() == null || self.newUrl() == '' || self.newUrl().length > 255) {
			self.newUrlValid(false);
			ret = false;
		}
		return ret;
	};

	// Saves the current model
	self.save = function () {
		if (self.validate()) {
			$.ajax({
				url: baseUrl + 'manager/alias/save',
				type: 'POST',
				dataType: 'json',
				contentType: 'application/json',
				data: JSON.stringify({
					Id: self.id(),
					OldUrl: self.oldUrl(),
					NewUrl: self.newUrl(),
					IsPermanent: self.isPermanent()
				}),
				success: function (result) {
					if (result.success) {
						self.items(result.data);
						self.clear();
					}
				},
				error: function (result) {
					console.log('error');
				}
			});
		}
	};

	// Deletes the specified author
	self.delete = function (id) {
		$.ajax({
			url: baseUrl + 'manager/alias/delete/' + id,
			type: 'GET',
			contentType: 'application/json',
			success: function (result) {
				if (result.success)
					self.items(result.data);
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Clears the model and collapses the form
	self.clear = function () {
		self.title(self.addTitle);
		self.id('');
		self.oldUrl('');
		self.oldUrlValid(true);
		self.newUrl('');
		self.newUrlValid(true);
		self.isPermanent(false);
		$('.collapse').collapse('hide');
		$('.table tr').removeClass('active');
	}

	// Initialze after everything is created.
	self.init();
};
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
// Config view model
//
manager.models.config = function (locale) {
	'use strict';

	var self = window.config = this;

	// Members
	self.active = ko.observable('general');
	self.siteTitle = ko.observable('');
	self.siteDescription = ko.observable('');
	self.siteArchivePageSize = ko.observable(0);
	self.cacheExpires = ko.observable(0);
	self.cacheMaxAge = ko.observable(0);
	self.commentModerateAnonymous = ko.observable(false);
	self.commentModerateAuthorized = ko.observable(false);

	// Initializes the model
	self.init = function () {
		$.ajax({
			url: baseUrl + 'manager/config/get',
			type: 'GET',
			dataType: 'json',
			success: function (result) {
				if (result.success)
					self.bind(result.data);
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Sets the currently active tab
	self.setActive = function (tab) {
		self.active(tab);
	};

	// Saves the site config
	self.saveSite = function () {
		$.ajax({
			url: baseUrl + 'manager/config/site/save',
			type: 'POST',
			dataType: 'json',
			contentType: 'application/json',
			data: JSON.stringify({
				Title: self.siteTitle(),
				Description: self.siteDescription(),
				ArchivePageSize: self.siteArchivePageSize()
			}),
			success: function (result) {
				if (result.success) {
					self.bind(result.data);
				}
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Saves the cache config
	self.saveCache = function () {
		$.ajax({
			url: baseUrl + 'manager/config/cache/save',
			type: 'POST',
			dataType: 'json',
			contentType: 'application/json',
			data: JSON.stringify({
				Expires: self.cacheExpires(),
				MaxAge: self.cacheMaxAge()
			}),
			success: function (result) {
				if (result.success) {
					self.bind(result.data);
				}
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Saves the comment config
	self.saveComments = function () {
		$.ajax({
			url: baseUrl + 'manager/config/comments/save',
			type: 'POST',
			dataType: 'json',
			contentType: 'application/json',
			data: JSON.stringify({
				ModerateAnonymous: self.commentModerateAnonymous(),
				ModerateAuthorized: self.commentModerateAuthorized()
			}),
			success: function (result) {
				if (result.success) {
					self.bind(result.data);
				}
			},
			error: function (result) {
				console.log('error');
			}
		});
	};

	// Binds the given data to the model.
	self.bind = function (data) {
		self.siteTitle(data.Site.Title);
		self.siteDescription(data.Site.Description);
		self.siteArchivePageSize(data.Site.ArchivePageSize);
		self.cacheExpires(data.Cache.Expires);
		self.cacheMaxAge(data.Cache.MaxAge);
		self.commentModerateAnonymous(data.Comments.ModerateAnonymous);
		self.commentModerateAuthorized(data.Comments.ModerateAuthorized);
	};

	// Initialze after everything is created.
	self.init();
};
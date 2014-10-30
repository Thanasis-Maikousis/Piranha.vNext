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

//
// Char counter
$(document).on('keyup', '.count-me', function () {
	var length = $(this).val().length;
	$(this).next().find('span').text(length);
});

// 
// Create base object
var manager = {};

//
// Comment dialog handler
manager.commentDialog = function (domElm, baseUrl, postId) {
	'use strict';

	var self = this;

	self.domElm = domElm;
	self.baseUrl = baseUrl;
	self.postId = postId;

	$(document).on('change', self.domElm + ' .comment-approved', function () {
		var row = $(this).parent().parent();
		var commentId = $(row).find('.comment-id').val();

		$.ajax({
			url: baseUrl + '/approve',
			type: 'POST',
			dataType: 'html',
			contentType: 'application/json',
			data: JSON.stringify({
				PostId: self.postId,
				CommentId: commentId,
				Status: $(this).is(':checked')
			}),
			success: function (result) {
				$(domElm).html(result);
			}
		});
	});

	// Updated spam status for comment
	$(document).on('change', self.domElm + ' .comment-spam', function () {
		var row = $(this).parent().parent();
		var commentId = $(row).find('.comment-id').val();

		$.ajax({
			url: self.baseUrl + '/spam',
			type: 'POST',
			dataType: 'html',
			contentType: 'application/json',
			data: JSON.stringify({
				PostId: self.postId,
				CommentId: commentId,
				Status: $(this).is(':checked')
			}),
			success: function (result) {
				$(domElm).html(result);
			}
		});
	});
};
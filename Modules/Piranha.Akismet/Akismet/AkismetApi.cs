/*
 * Copyright (c) 2014 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.vnext
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace Piranha.Akismet
{
	/// <summary>
	/// Akismet API.
	/// </summary>
	public class AkismetApi
	{
		#region Members
		/// <summary>
		/// The private verification url
		/// </summary>
		private const string verifyKeyUrl = "http://rest.akismet.com/1.1/verify-key";

		/// <summary>
		/// The private comment check url
		/// </summary>
		private const string commentCheckUrl = "http://{0}.rest.akismet.com/1.1/comment-check";
		#endregion

		#region Properties
		/// <summary>
		/// Gets the key verification API url.
		/// </summary>
		public string VerifyKeyUrl {
			get { return verifyKeyUrl;  }
		}

		/// <summary>
		/// Gets the comment check API url.
		/// </summary>
		public string CommentCheckUrl {
			get { return String.Format(commentCheckUrl, Config.Akismet.ApiKey); }
		}
		#endregion

		/// <summary>
		/// Verifies the configured api key.
		/// </summary>
		/// <returns>If the key passed verification.</returns>
		public bool VerifyKey() {
			var value = Call(VerifyKeyUrl, new {
				key = Config.Akismet.ApiKey,
				blog = HttpUtility.UrlEncode(Config.Akismet.SiteUrl)
			});
			return value == "valid";
		}

		/// <summary>
		/// Checks if the provided comment should be regarded as spam.
		/// </summary>
		/// <param name="comment">The comment</param>
		/// <returns>True if the comment should be regarded as spam</returns>
		public bool CommentCheck(Models.Comment comment) {
			var post = Client.Models.PostModel.GetById(comment.PostId);

			if (post != null) {
				var ui = new Client.Helpers.UIHelper();

				var value = Call(CommentCheckUrl, new {
					blog = HttpUtility.UrlEncode(Config.Akismet.ApiKey),
					user_ip = HttpUtility.UrlEncode(comment.IP),
					user_agent = HttpUtility.UrlEncode(comment.UserAgent),
					referrer = "",
					permalink = HttpUtility.UrlEncode(App.Env.AbsoluteUrl(ui.Permalink(post))),
					comment_type = "comment",
					comment_author = HttpUtility.UrlEncode(comment.Author),
					comment_author_email = HttpUtility.UrlEncode(comment.Email),
					comment_author_url = HttpUtility.UrlEncode(comment.WebSite),
					comment_content = HttpUtility.UrlEncode(comment.Body)
				});
				return Convert.ToBoolean(value);
			}
			return false;
		}

		#region Private methods
		/// <summary>
		/// Calls the given url with the given data formatted as
		/// HTTP post data.
		/// </summary>
		/// <param name="url">The api url</param>
		/// <param name="data">The post data</param>
		/// <returns>The server response</returns>
		private string Call(string url, object data) {
			var request = (HttpWebRequest)WebRequest.Create(url);
			var formData = ObjectToFormData(data);

			request.Method = "POST";
			request.UserAgent = "Piranha.Akismet";
			request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
			request.ContentLength = formData.Length;

			// Write post data
			using (var writer = new StreamWriter(request.GetRequestStream())) {
				writer.Write(formData);
			}

			// Read response
			var response = (HttpWebResponse)request.GetResponse();
			using (var reader = new StreamReader(response.GetResponseStream())) {
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Converts the given object to form post data.
		/// </summary>
		/// <param name="data">The post data</param>
		/// <returns>The data formatted as HTTP form data</returns>
		private string ObjectToFormData(object data) {
			var sb = new StringBuilder();

			foreach (var prop in data.GetType().GetProperties()) {
				if (sb.Length > 0)
					sb.Append("&");

				sb.Append(prop.Name);
				sb.Append("=");
				sb.Append(Convert.ToString(prop.GetValue(data)));
			}
			return sb.ToString();
		}
		#endregion
	}
}
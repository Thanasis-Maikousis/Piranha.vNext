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
using System.IO;
using System.Text;
using System.Web;
using Piranha.Server;

namespace Piranha.AspNet.Web
{
	/// <summary>
	/// Response for returning a content stream to the client.
	/// </summary>
	public class StreamResponse : IStreamResponse
	{
		#region Members
		private readonly HttpContext context;
		#endregion

		#region Properties
		/// <summary>
		/// Gets/sets the content type.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Gets/sets the content encoding.
		/// </summary>
		public Encoding ContentEncoding { get; set; }

		/// <summary>
		/// Gets the output stream.
		/// </summary>
		public Stream OutputStream { get; private set; }
		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		public StreamResponse(HttpContext context) {
			this.context = context;

			OutputStream = context.Response.OutputStream;
		}

		/// <summary>
		/// Executes the response.
		/// </summary>
		public void Execute() {
			context.Response.StatusCode = 200;
			context.Response.ContentType = ContentType;
			context.Response.ContentEncoding = ContentEncoding;

			if (OutputStream.CanWrite) {
				OutputStream.Flush();
				OutputStream.Close();
			}
			context.Response.End();
		}
	}
}

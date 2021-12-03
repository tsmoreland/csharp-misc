//
// Copyright © 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System.Web.Optimization;

namespace OwinSample.WebApp
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BundleConfig
    {
        protected BundleConfig()
        {
            
        }

        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles
                .Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/lib/jquery/jquery.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/jqueryval")
                .Include(
                    "~/lib/jquery-validate/jquery.validate.js",
                    "~/lib/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles
                .Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/lib/modernizr/modernizr.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/lib/twitter-bootstrap/js/bootstrap.js"));

            bundles
                .Add(new StyleBundle("~/Content/css")
                .Include("~/lib/twitter-bootstrap/css/bootstrap.css", "~/Content/site.css"));
        }
    }
}

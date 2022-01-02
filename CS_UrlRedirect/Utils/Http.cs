// ============================================================================
//    Author: Kenneth Perkins
//    Date:   May 11, 2021
//    Taken From: http://programmingnotes.org/
//    File:  Utils.cs
//    Description: Handles general utility functions
// ============================================================================
using System;

namespace Utils {
    public static class Http {
        /// <summary>
        /// Determines if a url is valid or not
        /// </summary>
        /// <param name="url">The url to check</param>
        /// <returns>True if a url is valid, false otherwise</returns>
        public static bool IsValidURL(string url) {
            var period = url.IndexOf(".");
            if (period > -1 && !url.Contains("@")) {
                // Check if there are remnants where the url scheme should be.
                // Dont modify string if so
                var colon = url.IndexOf(":");
                var slash = url.IndexOf("/");
                if ((colon == -1 || period < colon) &&
                    (slash == -1 || period < slash)) {
                    url = $"http://{url}";
                }
            }

            System.Uri uriResult = null;
            var result = System.Uri.TryCreate(url, System.UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == System.Uri.UriSchemeHttp || uriResult.Scheme == System.Uri.UriSchemeHttps);
            return result;
        }
    }
}// http://programmingnotes.org/
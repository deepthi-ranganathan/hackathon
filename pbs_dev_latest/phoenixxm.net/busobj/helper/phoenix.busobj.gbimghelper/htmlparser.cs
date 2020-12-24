/* $Header: C:\\cvsroot\\yole/syndirella/HTMLParser.cs,v 1.6 2003/01/19 14:47:28 yole Exp $
 * Syndirella: simple regexp-based functions for HTML parsing
 * (C) 2002-03 Dmitry Jemerov <yole@yole.ru>
 * Released under the GNU General Public License; see the file COPYING for full license text
 */

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Phoenix.BusObj.GbHelper
{
	public class HTMLParser
	{
		private static Regex rxComments = new Regex(@"<!--[^>]*>", RegexOptions.Singleline);
		private static Regex fRxUnicodeEntity = new Regex("&#(\\d{3,4});");
		private static Regex fRxStripHTML = new Regex("<[^<>]+>");
		private static Regex fRxScript = new Regex("<script[^>]*>.*?</script>", RegexOptions.IgnoreCase);

		internal static string RemoveComments(string doc)
		{
			return rxComments.Replace(doc, "");
		}

		internal static Regex BuildTagRx(string tagName)
		{
			//Original Code
			//return new Regex(@"<\s*" + tagName + @"\s[^>]*>",
			//	RegexOptions.Singleline | RegexOptions.IgnoreCase);
			//The following thing lists every tag in the HTML....
			//Regex regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
			//<TAG\b[^>]*>(.*?)</TAG>  matches the opening and closing pair of a specific HTML tag. Anything between the tags is captured into the first backreference. The question mark in the regex makes the star lazy, to make sure it stops before the first closing tag rather than before the last, like a greedy star would do. This regex will not properly match tags nested inside themselves, like in <TAG>one<TAG>two</TAG>one</TAG>.
			////<([A-Z][A-Z0-9]*)\b[^>]*>(.*?)</\1>  will match the opening and closing pair of any HTML tag. Be sure to turn off case sensitivity. The key in this solution is the use of the backreference \1 in the regex. Anything between the tags is captured into the second backreference. This solution will also not match tags nested in themselves.
			//return new Regex((@"<" + tagName + @"\b[^>]*>(.*?)</" + tagName + @">"),
			//	RegexOptions.Singleline | RegexOptions.IgnoreCase);
			return new Regex ("<a\\s+(?:(?:\\w+\\s*=\\s*)(?:\\w+|\"[^\"]*\"|'[^']*'))*?\\s*href\\s*=\\s*(?<url>\\w+|\"[^\"]*\"|'[^']*')(?:(?:\\s+\\w+\\s*=\\s*)(?:\\w+|\"[^\"]*\"|'[^']*'))*?>[^<]+</a>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
			
		}
		internal static MatchCollection GetTagMatches(string doc, string tagName)
		{
			return BuildTagRx(tagName).Matches(doc);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		internal static MatchCollection GetTagMatches(string doc)
		{
			Regex regExp = new Regex("href\\s*=\\s*(?:(?:\\\"(?[^\\\"]*)\\\")|(?[^\\s]* ))", RegexOptions.Singleline | RegexOptions.IgnoreCase); 
			return regExp.Matches(doc);
		}

		#region Commented Code
		/*
		 * 
		internal static string GetTitle(string doc)
		{
			Regex rxTitle = new Regex("<title>([^>]+)</title>", RegexOptions.IgnoreCase);
			Match m = rxTitle.Match(doc);
			if (m.Success)
				return m.Groups [1].Value;
			return "";
		}

		internal static Regex BuildAttrRx(string attrName)
		{
			return new Regex(attrName + "=\"?([^\"\\s>]*)[\" >]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		}

		internal static string GetAttrValue(string tag, string attrName)
		{
			Match m = BuildAttrRx(attrName).Match(tag);
			if (m.Success)
				return m.Groups [1].Value;
			return null;
		}

		internal static Encoding GetContentType(Stream stream)
		{
			Encoding result = null;
			stream.Seek(0, SeekOrigin.Begin);
			TextReader reader = new StreamReader(stream);
			string doc = reader.ReadToEnd();
			string commentLess = RemoveComments(doc);
			MatchCollection metaTags = GetTagMatches(doc, "meta");
			foreach(Match m in metaTags)
			{
				string httpEquiv = GetAttrValue(m.Value, "http-equiv");
				if (httpEquiv == null || httpEquiv != "content-type")
					continue;
				string content = GetAttrValue(m.Value, "content");
				if (content == null)
					continue;
				int pos = content.IndexOf("charset");
				if (pos < 0)
					continue;
				string charset = content.Substring(pos+8);  // charset=
				try
				{
					result = Encoding.GetEncoding(charset);
					break;
				}
				catch(Exception)
				{
					break;
				}
			}
			return result;
		}

		internal static string StripHTML(string text)
		{
			return fRxStripHTML.Replace(text, "");
		}

		internal static string StripScripts(string text)
		{
			return fRxScript.Replace(text, "");
		}

		internal static string ProcessEntities(string text)
		{
			string s = fRxUnicodeEntity.Replace(text, new MatchEvaluator(ProcessUnicodeEntity));
			s = s.Replace("&nbsp;", " ");
			s = s.Replace("&gt;", ">");
			s = s.Replace("&lt;", "<");
			s = s.Replace("&quot;", "\"");
			s = s.Replace("&amp;", "&");
			return s;
		}

		private static string ProcessUnicodeEntity(Match match)
		{
			Char c = (char) Int32.Parse(match.Groups [1].Value);
			return c.ToString();
		}


		internal static string FixupRelativeLinks(string doc, Uri baseUrl)
		{
			return new LinkFixupProcessor(baseUrl).FixupRelativeLinks(doc);
		}
		*/
		#endregion 
	}

	#region Commented Calss
	/*
	internal class LinkFixupProcessor
	{
		private Uri fBaseUrl;
		private static Regex fImgTagRx   = HTMLParser.BuildTagRx("img");
		private static Regex fATagRx     = HTMLParser.BuildTagRx("a");
		private static Regex fSrcAttrRx  = HTMLParser.BuildAttrRx("src");
		private static Regex fHrefAttrRx = HTMLParser.BuildAttrRx("href");
		private Regex fCurAttrRx;

		internal LinkFixupProcessor(Uri baseUrl)
		{
			fBaseUrl = baseUrl;
		}

		internal string FixupRelativeLinks(string doc)
		{
			fCurAttrRx = fSrcAttrRx;
			doc = fImgTagRx.Replace(doc, new MatchEvaluator(FixupTag));
			fCurAttrRx = fHrefAttrRx;
			doc = fATagRx.Replace(doc, new MatchEvaluator(FixupTag));
			return doc;
		}

		private string FixupTag(Match tagMatch)
		{
			string oldValue = tagMatch.Value;
			Match attrMatch = fCurAttrRx.Match(oldValue);
			if (attrMatch.Success)
			{
				Group attrMatchGroup = attrMatch.Groups [1];
				string url = attrMatchGroup.Value;
				if (url.Length > 0 && url.IndexOf("://") < 0 && !url.StartsWith("mailto:"))
				{
					try
					{
						url = new Uri(fBaseUrl, url).ToString();
						return oldValue.Substring(0, attrMatchGroup.Index) + url +
							oldValue.Substring(attrMatchGroup.Index + attrMatchGroup.Length);
					}
					catch(Exception)
					{
						// ignore the link if the URL parsing fails
						return oldValue;
					}
				}
			}
			return tagMatch.Value;
		}
		
	}
	*/
	#endregion 
}

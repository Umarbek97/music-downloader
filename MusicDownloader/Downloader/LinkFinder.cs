using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;

namespace MusicDownloader.Downloader
{
    class LinkFinder
    {
        private string _url;
        private HtmlWeb _web;
        private HtmlDocument _doc;
        private string[] _sites = { "hitmo", "drivemusic" };
        private string songTitle;
        private string path = Path.Combine(Environment.CurrentDirectory, "output");

        public string GetPath
        {
            get { return path; }
        }

        public string GetSongTitle
        {
            get { return songTitle; }
        }

        public LinkFinder()
        {
            _url = "https://c.hitmos.com/search?q=";
            _web = new HtmlWeb();
            _web.PreRequest += request =>
            {
                request.CookieContainer = new System.Net.CookieContainer();
                return true;
            };

            CreateFolder();
        }

        private void CreateFolder()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private string GetSiteUrl(string songName)
        {
            songName = songName.Replace(" ", "+");
            songName = songName.Contains("&") ? songName.Replace("&", "and") : songName;
            _url += songName;
            List<string> linksList = new List<string>();
            _doc = _web.Load(_url);
            var links = _doc.DocumentNode.SelectNodes("//a[@class='track__download-btn']");

            foreach (HtmlNode link in links)
            {
                string hrefVal = link.GetAttributeValue("href", String.Empty);
                linksList.Add(hrefVal);
                break;
            }

            if (linksList.Count > 0)
            {
                return linksList.ToArray()[0];
            }

            return "";
        }

        public string GetDownloadString(string songName)
        {
            /*List<string> songs = new List<string>();
            _doc = _web.Load(GetSiteUrl(songName));*/
            /*var links = _doc.DocumentNode.SelectNodes("//a");
            songTitle = _doc.DocumentNode.SelectSingleNode("//h1[@class='p-track-title']").InnerText;

            foreach (var item in links)
            {
                songs.Add(item.Attributes["href"]?.Value);
            }

            foreach (string song in songs)
            {
                if (song.Contains(".mp3"))
                    return song;
            }*/

            return GetSiteUrl(songName);
        }
    }
}

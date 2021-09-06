using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;

namespace MusicDownloader.Downloader
{
    class LinkFinder
    {
        private string _url;
        private HtmlWeb _web;
        private HtmlDocument _doc;
        private string _songName;

        private readonly string path = Path.Combine(Environment.CurrentDirectory, "output");

        public List<Song> List = new List<Song>();

        public string GetPath
        {
            get { return path; }
        }

        public LinkFinder(string songName)
        {
            _songName = songName;

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

            return;
        }

        public string GetSiteUrl()
        {
            _songName = _songName.Replace(" ", "+");
            _songName = _songName.Contains("&") ? _songName.Replace("&", "and") : _songName;
            _url += _songName;
            _doc = _web.Load(_url);

            AddSongName(_doc);

            var links = _doc.DocumentNode.SelectNodes("//a[@class='track__download-btn']");
            List<string> linksList = new List<string>();

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

        public void AddSongName(HtmlDocument doc)
        {
            string songTitle = doc.DocumentNode.SelectSingleNode("//div[@class='track__title']").InnerText;
            string songDesc = doc.DocumentNode.SelectSingleNode("//div[@class='track__desc']").InnerText;
            if (!String.IsNullOrEmpty(songTitle) && !String.IsNullOrEmpty(songDesc))
            {
                List.Add(new Song { Name = songTitle, Author = songDesc });
            }
        }
    }

    class Song
    {
        public string Name { get; set; }

        public string Author { get; set; }
    }
}

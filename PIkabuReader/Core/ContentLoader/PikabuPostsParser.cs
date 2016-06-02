#region

using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

#endregion

namespace PIkabuReader.Core.ContentLoader
{
    public class PikabuPost 
    {
        /// <summary>
        ///     Id поста
        /// </summary>
        /// <remarks>
        ///     data-story-id
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        ///     Ссылка на пост
        /// </summary>
        /// <remarks>
        ///     post_title page_title, href
        /// </remarks>
        public string PostHref { get; set; }

        ///// <summary>
        /////     Был ли посещен
        ///// </summary>
        ///// <remarks>
        /////     data-visited
        ///// </remarks>
        //public bool Visited { get; set; }

        /// <summary>
        ///     Заголовок поста
        /// </summary>
        /// <remarks>
        ///     post_title page_title
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        ///     Автор поста
        /// </summary>
        /// <remarks>
        ///     post_author  boldlabel
        /// </remarks>
        public string Autor { get; set; }


        /// <summary>
        ///     Рейтинг поста
        /// </summary>
        /// <remarks>
        ///     post_rating_count control label
        /// </remarks>
        public int Rating { get; set; }

        /// <summary>
        ///     Кол-во комментариев
        /// </summary>
        /// <remarks>
        ///     post_comments_count label to-comments
        /// </remarks>
        public int CommentsCount { get; set; }

        /// <summary>
        ///     Теги
        /// </summary>
        /// <remarks>
        ///     post_tag
        /// </remarks>
        public string[] Tags { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as PikabuPost;
            if (other != null)
            {
                return this.Id == other.Id;
            }

            return false;
        }
    }


    internal class PikabuPostsParser
    {
        private readonly string storiesContainer = @"stories_container";


        public PikabuPost[] GetPosts(string htmlText)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlText);

            var container =
                html.GetElementbyId(storiesContainer)
                    .ChildNodes.Where(cn => cn.Attributes.Any(a => a.Value == "post" && a.Name == "class")).ToArray();

            var posts = new List<PikabuPost>(container.Length);
            foreach (var post in container)
            {
                var t  = post;

                try
                {
                    var titleNode = GetByClassName(post, @"post_title page_title");
                    var href = titleNode.ChildNodes.First().Attributes["href"].Value;
                    var author = GetByClassName(post, @"post_author  boldlabel", true).InnerText;
                    var rating = GetByClassName(post, @"post_rating_count control label", true);
                    var id = int.Parse(post.Attributes["data-story-id"].Value);

                    var pikabuPost = new PikabuPost
                    {
                        Id = id,
                        Title = titleNode.InnerText,
                        PostHref = href,
                        Autor = author,
                        Rating = int.Parse(rating?.InnerText ?? "0")
                    };

                    posts.Add(pikabuPost);
                }
                catch
                {
                    // ignored, mb advertisement
                }
            }


            return posts.ToArray();
        }


        private HtmlNode GetByClassName(HtmlNode parent, string className, bool recursive = false)
        {
            var htmlNode = parent.ChildNodes.FirstOrDefault(
                cn => cn.Attributes.Any(a => a.Value.Contains(className) && a.Name == "class"));

            if (htmlNode == null)
            {
                if (recursive && parent.ChildNodes.Any())
                {
                    foreach (var childNode in parent.ChildNodes)
                    {
                        htmlNode = GetByClassName(childNode, className, true);
                        if (htmlNode != null)
                            return htmlNode;
                    }
                }
            }

            return htmlNode;
        }
    }
}
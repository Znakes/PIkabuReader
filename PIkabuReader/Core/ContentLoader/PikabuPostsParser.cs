#region

using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

#endregion

namespace PIkabuReader.Core.ContentLoader
{
    public enum PostType
    {
        None, 
        OnlyText,
        BigImage,
        CompositeText // coub, youtube, text, etc
    }
    
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

        /// <summary>
        /// Тип поста
        /// </summary>
        public PostType PostType { get; set; }

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
                    var titleNode = GetByClassName(post,"h2", @"post_title page_title");
                    var href = titleNode.First().ChildNodes.First().Attributes["href"].Value;
                    var author = GetByClassName(post, "a", @"post_author  boldlabel");
                    var rating = GetByClassName(post, "span", @"post_rating_count control label");
                    var id = int.Parse(post.Attributes["data-story-id"].Value);

                    var pikabuPost = new PikabuPost
                    {
                        Id = id,
                        Title = titleNode.First().InnerText,
                        PostHref = href,
                        Autor = author?.First().InnerText,
                        Rating = int.Parse(rating.First().InnerText ?? "0")
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



        private HtmlNode[] GetByClassName(HtmlNode parent, string type, string className )
        {
            return parent.Descendants(type).Where(
                d =>d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains(className)
                    ).ToArray();
        }
    }
}
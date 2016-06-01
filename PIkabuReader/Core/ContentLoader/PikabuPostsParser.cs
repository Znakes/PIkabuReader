using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIkabuReader.Core.ContentLoader
{
    public class PikabuPost
    {
        /// <summary>
        /// Id поста
        /// </summary>
        /// <remarks>
        /// data-story-id
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// Ссылка на пост
        /// </summary>
        /// <remarks>
        /// post_title page_title, href
        /// </remarks>
        public string PostHref { get; set; }

        /// <summary>
        /// Был ли посещен
        /// </summary>
        /// <remarks>
        /// data-visited
        /// </remarks>
        public bool Visited { get; set; }

        /// <summary>
        /// Заголовок поста
        /// </summary>
        /// <remarks>
        /// post_title page_title
        /// </remarks>
        public string Title { get; set; }

        /// <summary>
        /// Автор поста
        /// </summary>
        /// <remarks>
        /// post_author  boldlabel
        /// </remarks>
        public string Autor { get; set; }


        /// <summary>
        /// Рейтинг поста
        /// </summary>
        /// <remarks>
        /// post_rating_count control label
        /// </remarks>
        public int Rating { get; set; }

        /// <summary>
        /// Кол-во комментариев
        /// </summary>
        /// <remarks>
        /// post_comments_count label to-comments
        /// </remarks>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Теги
        /// </summary>
        /// <remarks>
        /// post_tag
        /// </remarks>
        public string[] Tags { get; set; }
    }



    class PikabuPostsParser
    {
    }
}

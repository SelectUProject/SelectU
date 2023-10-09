using SelectU.Contracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid? UserRatingId { get; set; }
        public string Content { get; set; }
        public bool Editted { get; set; }
        public DateTime? DateCreated { get; set; }
        [ForeignKey("UserRatingId")]
        public virtual UserRating UserRating { get; set; }
        public Comment() { }
        public Comment(CommentDTO comment)
        {
            Id = comment.Id;
            UserRatingId = comment.UserRatingId;
            Content = comment.Content;
            Editted = comment.Editted;
            DateCreated = comment.DateCreated;
        }
        public List<Comment> CommentDTOsToComments(ICollection<CommentDTO>? comments)
        {
            if (comments == null) return new List<Comment>();
            List<Comment> list = new List<Comment>();
            comments.ToList().ForEach(x => list.Add(new Comment(x)));
            return list;
        }
    }
}

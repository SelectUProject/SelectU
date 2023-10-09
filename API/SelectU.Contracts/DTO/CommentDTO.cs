using SelectU.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Contracts.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid UserRatingId { get; set; }
        public string Content { get; set; }
        public bool Editted { get; set; }
        public DateTime? DateCreated { get; set; }
        public CommentDTO() { }
        public CommentDTO(Comment comment) {
            Id = comment.Id;
            UserRatingId = (Guid)comment.UserRatingId;
            Content = comment.Content;
            Editted = comment.Editted;
            DateCreated = comment.DateCreated;
        }
        public List<CommentDTO> CommentsToCommentDTOs(ICollection<Comment>? comments) {
            if (comments == null) return new List<CommentDTO>();
            List<CommentDTO> list = new List<CommentDTO>();
            comments.ToList().ForEach(x => list.Add(new CommentDTO(x)));
            return list;
        }
    }
}

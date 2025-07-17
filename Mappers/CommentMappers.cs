using StocksWebApi.DTOs.Comment;
using StocksWebApi.Models;

namespace StocksWebApi.Mappers
{
    /// <summary>
    /// Класс для маппинга данных между моделями и DTO для комментариев.
    /// </summary>
    public static class CommentMappers
    {
        /// <summary>
        /// Преобразует модель комментария в DTO.
        /// </summary>
        /// <param name="commentModel">Модель комментария</param>
        /// <returns>DTO комментария</returns>
        public static CommentDTO ToCommentDTO(this Comment commentModel)
        {
            return new CommentDTO
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                Created = commentModel.Created,
                CreatedBy = commentModel.AppUser.UserName, // Имя пользователя, создавшего комментарий
                StockId = commentModel.StockId // ID акции, к которой относится комментарий
            };
        }

        /// <summary>
        /// Преобразует DTO создания комментария в модель.
        /// </summary>
        /// <param name="commentModel">DTO создания комментария</param>
        /// <param name="stockId">ID акции</param>
        /// <returns>Модель комментария</returns>
        public static Comment ToCommentFromCreate(this CreateCommentDTO commentModel, Guid stockId)
        {
            return new Comment
            {
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = stockId // Привязка комментария к акции
            };
        }

        /// <summary>
        /// Преобразует DTO обновления комментария в модель.
        /// </summary>
        /// <param name="commentModel">DTO обновления комментария</param>
        /// <returns>Модель комментария</returns>
        public static Comment ToCommentFromUpdate(this UpdateCommentReqDTO commentModel)
        {
            return new Comment
            {
                Title = commentModel.Title,
                Content = commentModel.Content
            };
        }
    }
}
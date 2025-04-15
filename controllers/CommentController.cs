using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCommentById(int id)
        {

            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());

        }

        [HttpPost]
        [Route("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentDto createCommentDto)
        {
            if (!await _stockRepository.StockExists(stockId))
            {
                return BadRequest("Stock not found");
            }
            var commentModel = createCommentDto.ToCommentModel(stockId);
            await _commentRepository.CreateCommentAsync(commentModel);
            return CreatedAtAction(nameof(GetCommentById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }
    }
}
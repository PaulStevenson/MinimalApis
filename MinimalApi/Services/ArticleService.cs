using AutoMapper;
using MinimalApiDemo.Entities;

namespace MinimalApiDemo.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ArticleService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<Article>> GetAll()
        {
            var entities = await _context.Articles.ToListAsync();
            var results = _mapper.Map<IList<Article>>(entities);

            return results;
        }

        public async Task<Article> GetById(int id)
        {
            var articleEntity = await _context.Articles.FindAsync(id);
            var result = _mapper.Map<Article>(articleEntity);

            return result;
        }

        public async Task<Article> Post(ArticleRequest article)
        {
            var title = article.Title.IsNullOrEmpty() ? "Default Title" : article.Title;

            var newArticle = new Article
            {
                Title = title,
                Content = article.Content,
                PublishedAt = article.PublishedAt,
                MyNumber = article.MyNumber
            };

            var entity = _mapper.Map<ArticleEntity>(newArticle);

            var createdArticle = _context.Articles.Add(entity);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<Article>(createdArticle.Entity);

            return response;
        }

        public async Task<Article> PostWithValidation(Article newArticle)
        {
            var entity = _mapper.Map<ArticleEntity>(newArticle);

            var createdArticle = _context.Articles.Add(entity);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<Article>(createdArticle.Entity);

            return response;
        }

        public async Task Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();
        }

        public async  Task<Article> Put(int id, ArticleRequest article)
        {
            var articleToUpdate = await _context.Articles.FindAsync(id);
            
            if (article.Title != null)
                articleToUpdate.Title = article.Title;

            if (article.Content != null)
                articleToUpdate.Content = article.Content;

            if (article.PublishedAt != null)
                articleToUpdate.PublishedAt = article.PublishedAt;

            if (articleToUpdate.MyNumber != null)
                articleToUpdate.MyNumber = article.MyNumber;

            await _context.SaveChangesAsync();

            var updatedArticle = await _context.Articles.FindAsync(id);
            var response = _mapper.Map<Article>(updatedArticle);

            return response;
        }
    }
}


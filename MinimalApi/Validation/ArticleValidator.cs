using FluentValidation;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Validation
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(2).MaximumLength(10);
            RuleFor(x => x.MyNumber).NotNull().GreaterThan(0);
        }
    }
}
